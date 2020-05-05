using System.Drawing;
using GXPEngine.Core;

namespace GXPEngine {
    /// <summary>
    ///     The Canvas object can be used for drawing 2D visuals at runtime.
    /// </summary>
    public class Canvas : Sprite {
        //------------------------------------------------------------------------------------------------------------------------
        //														alpha
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Draws a Sprite onto this Canvas.
        ///     It will ignore Sprite properties, such as color and animation.
        /// </summary>
        /// <param name='sprite'>
        ///     The Sprite that should be drawn.
        /// </param>
        private readonly PointF[] destPoints = new PointF[3];
        protected System.Drawing.Graphics _graphics;
        protected bool _invalidate;

        //------------------------------------------------------------------------------------------------------------------------
        //														Canvas()
        //------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Initializes a new instance of the Canvas class.
        ///     It is a regular GameObject that can be added to any displaylist via commands such as AddChild.
        ///     It contains a
        ///     <a href="http://msdn.microsoft.com/en-us/library/system.drawing.graphics(v=vs.110).aspx">System.Drawing.Graphics</a>
        ///     component.
        /// </summary>
        /// <param name='width'>
        ///     Width of the canvas in pixels.
        /// </param>
        /// <param name='height'>
        ///     Height of the canvas in pixels.
        /// </param>
        public Canvas(int width, int height, bool addCollider = true) : this(new Bitmap(width, height), addCollider) {
            name = width + "x" + height;
        }

        public Canvas(Bitmap bitmap, bool addCollider = true) : base(bitmap, addCollider) {
            _graphics = System.Drawing.Graphics.FromImage(bitmap);
            _invalidate = true;
        }

        public Canvas(string filename, bool addCollider = true) : base(filename, addCollider) {
            _graphics = System.Drawing.Graphics.FromImage(texture.bitmap);
            _invalidate = true;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														graphics
        //------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the graphics component. This interface provides tools to draw on the sprite.
        ///     See:
        ///     <a href="http://msdn.microsoft.com/en-us/library/system.drawing.graphics(v=vs.110).aspx">System.Drawing.Graphics</a>
        /// </summary>
        public System.Drawing.Graphics graphics {
            get {
                _invalidate = true;
                return _graphics;
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Render()
        //------------------------------------------------------------------------------------------------------------------------
        protected override void RenderSelf(GLContext glContext) {
            if (_invalidate) {
                _texture.UpdateGLTexture();
                _invalidate = false;
            }

            base.RenderSelf(glContext);
        }

        public void DrawSprite(Sprite sprite) {
            var halfWidth = sprite.texture.width / 2.0f;
            var halfHeight = sprite.texture.height / 2.0f;
            var p0 = sprite.TransformPoint(-halfWidth, -halfHeight);
            var p1 = sprite.TransformPoint(halfWidth, -halfHeight);
            var p2 = sprite.TransformPoint(-halfWidth, halfHeight);
            destPoints[0] = new PointF(p0.x, p0.y);
            destPoints[1] = new PointF(p1.x, p1.y);
            destPoints[2] = new PointF(p2.x, p2.y);
            graphics.DrawImage(sprite.texture.bitmap, destPoints);
        }

        // Called by the garbage collector
        ~Canvas() {
            _graphics.Dispose();
        }
    }
}