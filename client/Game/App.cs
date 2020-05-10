using GXPEngine;

namespace game {
    public class App : Game {
        private NetworkManager networkManager;
        private SceneManager sceneManager;
        private MouseCursor mouseCursor;
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            ShowMouse(false);
            SetupInput();

            networkManager = NetworkManager.Instance;
            sceneManager = SceneManager.Instance;
            mouseCursor = MouseCursor.Instance;

            AddChild(networkManager);
            AddChild(sceneManager);
            AddChild(mouseCursor);
            
            sceneManager.LoadScene("0");
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