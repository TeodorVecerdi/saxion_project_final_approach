namespace GXPEngine {
	/// <summary>
	///     A Camera gameobject, that owns a rectangular render window, and determines the focal point, rotation and scale
	///     of what's rendered in that window.
	///     (Don't forget to add this as child somewhere in the hierarchy.)
	/// </summary>
	public class Camera : GameObject {
//	internal class Camera : GameObject { <-- Why was this internal?
        private readonly Window _renderTarget;

        /// <summary>
        ///     Creates a camera game object and a sub window to render to.
        ///     Add this camera as child to the object you want to follow, or
        ///     update its coordinates directly in an update method.
        ///     The scale of the camera determines the "zoom factor" (High scale = zoom out)
        /// </summary>
        /// <param name="windowX">Left x coordinate of the render window.</param>
        /// <param name="windowY">Top y coordinate of the render window.</param>
        /// <param name="windowWidth">Width of the render window.</param>
        /// <param name="windowHeight">Height of the render window.</param>
        public Camera(int windowX, int windowY, int windowWidth, int windowHeight) {
            _renderTarget = new Window(windowX, windowY, windowWidth, windowHeight, this);
            game.OnAfterRender += _renderTarget.RenderWindow;
        }

        protected override void OnDestroy() {
            game.OnAfterRender -= _renderTarget.RenderWindow;
        }
    }
}