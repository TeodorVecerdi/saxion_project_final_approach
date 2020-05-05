using System.Drawing;
using Rectangle = GXPEngine.Core.Rectangle;

namespace GXPEngine {
    /// <summary>
    ///     Animated Sprite. Has all the functionality of a regular sprite, but supports multiple animation frames/subimages.
    /// </summary>
    public class AnimationSprite : Sprite {
        protected int _cols;
        protected int _currentFrame;
        protected float _frameHeight;
        protected int _frames;
        protected float _frameWidth;

        //------------------------------------------------------------------------------------------------------------------------
        //														AnimSprite()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="GXPEngine.AnimSprite" /> class.
        /// </summary>
        /// <param name='filename'>
        ///     The name of the file to be loaded. Files are cached internally.
        ///     Texture sizes should be a power of two: 1, 2, 4, 8, 16, 32, 64 etc.
        ///     The width and height don't need to be the same.
        ///     If you want to load transparent sprites, use .PNG with transparency.
        /// </param>
        /// <param name='cols'>
        ///     Number of columns in the animation.
        /// </param>
        /// <param name='rows'>
        ///     Number of rows in the animation.
        /// </param>
        /// <param name='frames'>
        ///     Optionally, indicate a number of frames. When left blank, defaults to width*height.
        /// </param>
        /// <param name="keepInCache">
        ///     If <c>true</c>, the sprite's texture will be kept in memory for the entire lifetime of the game.
        ///     This takes up more memory, but removes load times.
        /// </param>
        /// <param name="addCollider">
        ///     If <c>true</c>, this sprite will have a collider that will be added to the collision manager.
        /// </param>
        public AnimationSprite(string filename, int cols, int rows, int frames = -1, bool keepInCache = false, bool addCollider = true) :
            base(filename, keepInCache, addCollider) {
            name = filename;
            initializeAnimFrames(cols, rows, frames);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GXPEngine.AnimSprite" /> class.
        /// </summary>
        /// <param name='bitmap'>
        ///     The Bitmap object to be used to create the sprite.
        ///     Texture sizes should be a power of two: 1, 2, 4, 8, 16, 32, 64 etc.
        ///     The width and height don't need to be the same.
        ///     If you want to load transparent sprites, use .PNG with transparency.
        /// </param>
        /// <param name='cols'>
        ///     Number of columns in the animation.
        /// </param>
        /// <param name='rows'>
        ///     Number of rows in the animation.
        /// </param>
        /// <param name='frames'>
        ///     Optionally, indicate a number of frames. When left blank, defaults to width*height.
        /// </param>
        /// <param name="addCollider">
        ///     If <c>true</c>, this sprite will have a collider that will be added to the collision manager.
        /// </param>
        public AnimationSprite(Bitmap bitmap, int cols, int rows, int frames = -1, bool addCollider = true) : base(bitmap, addCollider) {
            name = "BMP " + bitmap.Width + "x" + bitmap.Height;
            initializeAnimFrames(cols, rows, frames);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														width
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the sprite's width in pixels.
        /// </summary>
        public override int width {
            get {
                if (_texture != null) return (int) Mathf.Abs(_texture.width * _scaleX * _frameWidth);
                return 0;
            }
            set {
                if (_texture != null && _texture.width != 0) scaleX = value / (_texture.width * _frameWidth);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														height
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the sprite's height in pixels.
        /// </summary>
        public override int height {
            get {
                if (_texture != null) return (int) Mathf.Abs(_texture.height * _scaleY * _frameHeight);
                return 0;
            }
            set {
                if (_texture != null && _texture.height != 0) scaleY = value / (_texture.height * _frameHeight);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														currentFrame
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the current frame.
        /// </summary>
        public int currentFrame {
            get => _currentFrame;
            set => SetFrame(value);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														frameCount
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the number of frames in this animation.
        /// </summary>
        public int frameCount => _frames;

        //------------------------------------------------------------------------------------------------------------------------
        //														initializeAnimFrames()
        //------------------------------------------------------------------------------------------------------------------------
        protected void initializeAnimFrames(int cols, int rows, int frames = -1) {
            if (frames < 0) frames = rows * cols;
            if (frames > rows * cols) frames = rows * cols;
            if (frames < 1) return;
            _cols = cols;
            _frames = frames;

            _frameWidth = 1.0f / cols;
            _frameHeight = 1.0f / rows;
            _bounds = new Rectangle(0, 0, _texture.width * _frameWidth, _texture.height * _frameHeight);

            _currentFrame = -1;
            SetFrame(0);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														SetFrame()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets the current animation frame.
        ///     Frame should be in range 0...frameCount-1
        /// </summary>
        /// <param name='frame'>
        ///     Frame.
        /// </param>
        public void SetFrame(int frame) {
            if (frame == _currentFrame) return;
            if (frame < 0) frame = 0;
            if (frame >= _frames) frame = _frames - 1;
            _currentFrame = frame;
            setUVs();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														setUVs
        //------------------------------------------------------------------------------------------------------------------------
        protected override void setUVs() {
            if (_cols == 0) return;

            var frameX = _currentFrame % _cols;
            var frameY = _currentFrame / _cols;

            var left = _frameWidth * frameX;
            var right = left + _frameWidth;

            var top = _frameHeight * frameY;
            var bottom = top + _frameHeight;

            if (!game.PixelArt) {
                //fix1
                var wp = .5f / _texture.width;
                left += wp;
                right -= wp;

                //end fix1

                //fix2
                var hp = .5f / _texture.height;
                top += hp;
                bottom -= hp;

                //end fix2
            }

            var frameLeft = _mirrorX ? right : left;
            var frameRight = _mirrorX ? left : right;

            var frameTop = _mirrorY ? bottom : top;
            var frameBottom = _mirrorY ? top : bottom;

            _uvs = new float[8] {
                frameLeft, frameTop, frameRight, frameTop,
                frameRight, frameBottom, frameLeft, frameBottom
            };
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														NextFrame()
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Goes to the next frame. At the end of the animation, it jumps back to the first frame. (It loops)
        /// </summary>
        public void NextFrame() {
            var frame = _currentFrame + 1;
            if (frame >= _frames) frame = 0;
            SetFrame(frame);
        }
    }

    //legacy, sorry Hans
    public class AnimSprite : AnimationSprite {
        public AnimSprite(string filename, int cols, int rows, int frames = -1) : base(filename, cols, rows, frames) { }
    }
}