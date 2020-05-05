using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace GXPEngine {
    /// <summary>
    ///     A class that can be used to create "sub windows" (e.g. mini-map, splitscreen, etc).
    ///     This is not a gameobject. Instead, subscribe the RenderWindow method to the main game's
    ///     OnAfterRender event.
    /// </summary>
    public class Window {
//    internal class Window { <-- Why internal
        private readonly Transformable window;
        private bool _dirty = true;
        private int _width, _height;

        // private variables:
        private int _windowX, _windowY;

        /// <summary>
        ///     The game object (which should be in the hierarchy!) that determines the focus point, rotation and scale
        ///     of the viewport window.
        /// </summary>
        public GameObject camera;

        /// <summary>
        ///     Creates a render window in the rectangle given by x,y,width,height.
        ///     The camera determines the focal point, rotation and scale of this window.
        /// </summary>
        public Window(int x, int y, int width, int height, GameObject camera) {
            _windowX = x;
            _windowY = y;
            _width = width;
            _height = height;
            this.camera = camera;
            window = new Transformable();
        }

        /// <summary>
        ///     The x coordinate of the window's left side
        /// </summary>
        public int windowX {
            get => _windowX;
            set {
                _windowX = value;
                _dirty = true;
            }
        }
        /// <summary>
        ///     The y coordinate of the window's top
        /// </summary>
        public int windowY {
            get => _windowY;
            set {
                _windowY = value;
                _dirty = true;
            }
        }
        /// <summary>
        ///     The window's width
        /// </summary>
        public int width {
            get => _width;
            set {
                _width = value;
                _dirty = true;
            }
        }
        /// <summary>
        ///     The window's height
        /// </summary>
        public int height {
            get => _height;
            set {
                _height = value;
                _dirty = true;
            }
        }

        /// <summary>
        ///     To render the scene in this window, subscribe this method to the main game's OnAfterRender event.
        /// </summary>
        public void RenderWindow(GLContext glContext) {
            if (_dirty) {
                window.x = _windowX + _width / 2;
                window.y = _windowY + _height / 2;
                _dirty = false;
            }

            glContext.PushMatrix(window.matrix);

            var pushes = 1;
            var current = camera;
            Transformable cameraInverse;
            while (true) {
                cameraInverse = current.Inverse();
                glContext.PushMatrix(cameraInverse.matrix);
                pushes++;
                if (current.parent == null)
                    break;
                current = current.parent;
            }

            if (current is Game) {
                // otherwise, the camera is not in the scene hierarchy, so render nothing - not even a black background
                var main = Game.main;
                SetRenderRange();
                main.SetViewport(_windowX, _windowY, _width, _height);
                GL.Clear(GL.COLOR_BUFFER_BIT);
                current.Render(glContext);
                main.SetViewport(0, 0, Game.main.width, Game.main.height);
                main.RenderRange = new Rectangle(0, 0, main.width, main.height);
            }

            for (var i = 0; i < pushes; i++) glContext.PopMatrix();
        }

        private void SetRenderRange() {
            var worldSpaceCorners = new Vector2[4];
            worldSpaceCorners[0] = camera.TransformPoint(-_width / 2, -_height / 2);
            worldSpaceCorners[1] = camera.TransformPoint(-_width / 2, _height / 2);
            worldSpaceCorners[2] = camera.TransformPoint(_width / 2, _height / 2);
            worldSpaceCorners[3] = camera.TransformPoint(_width / 2, -_height / 2);

            var maxX = float.MinValue;
            var maxY = float.MinValue;
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            for (var i = 0; i < 4; i++) {
                if (worldSpaceCorners[i].x > maxX) maxX = worldSpaceCorners[i].x;
                if (worldSpaceCorners[i].x < minX) minX = worldSpaceCorners[i].x;
                if (worldSpaceCorners[i].y > maxY) maxY = worldSpaceCorners[i].y;
                if (worldSpaceCorners[i].y < minY) minY = worldSpaceCorners[i].y;
            }

            Game.main.RenderRange = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}