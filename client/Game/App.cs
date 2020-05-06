using System.Collections.Generic;
using game.network;
using GXPEngine;

namespace game {
    public class App : Game {
        public static App Instance;
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            Instance = this;
            NetworkManager.Instance = new NetworkManager();
            AddChild(NetworkManager.Instance);
            targetFps = 60;

            SetupInput();
            
            var player = new Player(Rand.Int.ToString().Substring(0, 8), new Vector2(400, 400));
            AddChild(player);
        }

        private void SetupInput() {
            Input.AddAxis("Horizontal", new List<int>{Key.LEFT, Key.A}, new List<int>{Key.RIGHT, Key.D});
            Input.AddAxis("Vertical", new List<int>{Key.DOWN, Key.S}, new List<int>{Key.UP, Key.W}, true);
        }

        public static void Main(string[] args) {
            new App().Start();
        }
    }
}