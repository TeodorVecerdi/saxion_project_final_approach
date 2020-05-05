using System;
using System.Drawing;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace GXPEngine {
    /// <summary>
    ///     The Sprite class holds 2D images that can be used as objects in your game.
    /// </summary>
    public class Sprite : GameObject {
        protected Rectangle _bounds;

        private uint _color = 0xFFFFFF;

        protected bool _mirrorX;
        protected bool _mirrorY;
        protected Texture2D _texture;
        protected float[] _uvs;

        public BlendMode blendMode = null;

        /// <summary>
        ///     Initializes a new instance of the <see cref="GXPEngine.Sprite" /> class.
        ///     Specify a System.Drawing.Bitmap to use. Bitmaps will not be cached.
        /// </summary>
        /// <param name='bitmap'>
        ///     Bitmap.
        /// </param>
        /// <param name="addCollider">
        ///     If <c>true</c>, this sprite will have a collider that will be added to the collision manager.
        /// </param>
        public Sprite(Bitmap bitmap, bool addCollider = true) : base(addCollider) {
            if (Game.main == null) throw new Exception("Sprites cannot be created before creating a Game instance.");
            name = "BMP" + bitmap.Width + "x" + bitmap.Height;
            initializeFromTexture(new Texture2D(bitmap));
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Sprite()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="GXPEngine.Sprite" /> class.
        ///     Specify an image file to load. Please use a full filename. Initial path is the application folder.
        ///     Images will be cached internally. That means once it is loaded, the same data will be used when
        ///     you load the file again.
        /// </summary>
        /// <param name='filename'>
        ///     The name of the file that should be loaded.
        /// </param>
        /// <param name="keepInCache">
        ///     If <c>true</c>, the sprite's texture will be kept in memory for the entire lifetime of the game.
        ///     This takes up more memory, but removes load times.
        /// </param>
        /// <param name="addCollider">
        ///     If <c>true</c>, this sprite will have a collider that will be added to the collision manager.
        /// </param>
        public Sprite(string filename, bool keepInCache = false, bool addCollider = true) : base(addCollider) {
            if (Game.main == null) throw new Exception("Sprites cannot be created before creating a Game instance.");
            name = filename;
            initializeFromTexture(Texture2D.GetInstance(filename, keepInCache));
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														texture
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the texture that is used to create this sprite.
        ///     If no texture is used, null will be returned.
        ///     Use this to retreive the original width/height or filename of the texture.
        /// </summary>
        public Texture2D texture => _texture;

        //------------------------------------------------------------------------------------------------------------------------
        //														width
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the sprite's width in pixels.
        /// </summary>
        public virtual int width {
            get {
                if (_texture != null) return (int) Mathf.Abs(_texture.width * _scaleX);
                return 0;
            }
            set {
                if (_texture != null && _texture.width != 0) scaleX = value / (float) _texture.width;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														height
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the sprite's height in pixels.
        /// </summary>
        public virtual int height {
            get {
                if (_texture != null) return (int) Mathf.Abs(_texture.height * _scaleY);
                return 0;
            }
            set {
                if (_texture != null && _texture.height != 0) scaleY = value / (float) _texture.height;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														color
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the color filter for this sprite.
        ///     This can be any value between 0x000000 and 0xFFFFFF.
        /// </summary>
        public uint color {
            get => _color;
            set => _color = value & 0xFFFFFF;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														alpha
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the alpha value of the sprite.
        ///     Setting this value allows you to make the sprite (semi-)transparent.
        ///     The alpha value should be in the range 0...1, where 0 is fully transparent and 1 is fully opaque.
        /// </summary>
        public float alpha { get; set; } = 1.0f;

        //------------------------------------------------------------------------------------------------------------------------
        //														OnDestroy()
        //------------------------------------------------------------------------------------------------------------------------
        protected override void OnDestroy() {
            if (_texture != null) _texture.Dispose();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														initializeFromTexture()
        //------------------------------------------------------------------------------------------------------------------------
        protected void initializeFromTexture(Texture2D texture) {
            _texture = texture;
            _bounds = new Rectangle(0, 0, _texture.width, _texture.height);
            setUVs();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setUVs
        //------------------------------------------------------------------------------------------------------------------------
        protected virtual void setUVs() {
            var left = _mirrorX ? 1.0f : 0.0f;
            var right = _mirrorX ? 0.0f : 1.0f;
            var top = _mirrorY ? 1.0f : 0.0f;
            var bottom = _mirrorY ? 0.0f : 1.0f;
            _uvs = new float[8] {left, top, right, top, right, bottom, left, bottom};
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														createCollider
        //------------------------------------------------------------------------------------------------------------------------
        protected override Collider createCollider() {
            return new BoxCollider(this);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        protected override void RenderSelf(GLContext glContext) {
            if (game != null) {
                var bounds = GetExtents();
                var maxX = float.MinValue;
                var maxY = float.MinValue;
                var minX = float.MaxValue;
                var minY = float.MaxValue;
                for (var i = 0; i < 4; i++) {
                    if (bounds[i].x > maxX) maxX = bounds[i].x;
                    if (bounds[i].x < minX) minX = bounds[i].x;
                    if (bounds[i].y > maxY) maxY = bounds[i].y;
                    if (bounds[i].y < minY) minY = bounds[i].y;
                }

                var test = maxX < game.RenderRange.left || maxY < game.RenderRange.top || minX >= game.RenderRange.right ||
                           minY >= game.RenderRange.bottom;
                if (test == false) {
                    if (blendMode != null) blendMode.enable();
                    _texture.Bind();
                    glContext.SetColor((byte) ((_color >> 16) & 0xFF),
                        (byte) ((_color >> 8) & 0xFF),
                        (byte) (_color & 0xFF),
                        (byte) (alpha * 0xFF));
                    glContext.DrawQuad(GetArea(), _uvs);
                    glContext.SetColor(1, 1, 1, 1);
                    _texture.Unbind();
                    if (blendMode != null) BlendMode.NORMAL.enable();
                }
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetArea()
        //------------------------------------------------------------------------------------------------------------------------
        internal float[] GetArea() {
            return new float[8] {
                _bounds.left, _bounds.top,
                _bounds.right, _bounds.top,
                _bounds.right, _bounds.bottom,
                _bounds.left, _bounds.bottom
            };
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														GetExtents()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the four corners of this object as a set of 4 Vector2s.
        /// </summary>
        /// <returns>
        ///     The extents.
        /// </returns>
        public Vector2[] GetExtents() {
            var ret = new Vector2[4];
            ret[0] = TransformPoint(_bounds.left, _bounds.top);
            ret[1] = TransformPoint(_bounds.right, _bounds.top);
            ret[2] = TransformPoint(_bounds.right, _bounds.bottom);
            ret[3] = TransformPoint(_bounds.left, _bounds.bottom);
            return ret;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetOrigin()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets the origin, the pivot point of this Sprite in pixels.
        /// </summary>
        /// <param name='x'>
        ///     The x coordinate.
        /// </param>
        /// <param name='y'>
        ///     The y coordinate.
        /// </param>
        public void SetOrigin(float x, float y) {
            _bounds.x = -x;
            _bounds.y = -y;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Mirror
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     This function can be used to enable mirroring for the sprite in either x or y direction.
        /// </summary>
        /// <param name='mirrorX'>
        ///     If set to <c>true</c> to enable mirroring in x direction.
        /// </param>
        /// <param name='mirrorY'>
        ///     If set to <c>true</c> to enable mirroring in y direction.
        /// </param>
        public void Mirror(bool mirrorX, bool mirrorY) {
            _mirrorX = mirrorX;
            _mirrorY = mirrorY;
            setUVs();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														color
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets the color of the sprite.
        /// </summary>
        /// <param name='r'>
        ///     The red component, range 0..1
        /// </param>
        /// <param name='g'>
        ///     The green component, range 0..1
        /// </param>
        /// <param name='b'>
        ///     The blue component, range 0..1
        /// </param>
        public void SetColor(float r, float g, float b) {
            r = Mathf.Clamp(r, 0, 1);
            g = Mathf.Clamp(g, 0, 1);
            b = Mathf.Clamp(b, 0, 1);
            var rr = (byte) Mathf.Floor(r * 255);
            var rg = (byte) Mathf.Floor(g * 255);
            var rb = (byte) Mathf.Floor(b * 255);
            color = rb + (uint) (rg << 8) + (uint) (rr << 16);
        }
    }
}