using GXPEngine;

namespace game {
    public class App : Game {
        private readonly MouseCursor mouseCursor;
        private readonly NetworkManager networkManager;
        private readonly SceneManager sceneManager;

        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            ShowMouse(true);
            SetupInput();

            networkManager = NetworkManager.Instance;
            sceneManager = SceneManager.Instance;
            mouseCursor = MouseCursor.Instance;

            AddChild(networkManager);
            AddChild(sceneManager);
            AddChild(mouseCursor);

            sceneManager.LoadScene("Login");
        }

        private void SetupInput() {
            // Input.AddAxis("Horizontal", new List<int>{Key.LEFT, Key.A}, new List<int>{Key.RIGHT, Key.D});
            // Input.AddAxis("Vertical", new List<int>{Key.DOWN, Key.S}, new List<int>{Key.UP, Key.W}, true);
        }

        public static void Main(string[] args) {
            new App().Start();
        }
    }
}