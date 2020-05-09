using System;
using System.Drawing;
using System.Threading.Tasks;
using game.Scenes;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Button = game.ui.Button;
using Debug = GXPEngine.Debug;

namespace game {
    public class App : Game {
        private NetworkManager networkManager;
        private SceneManager sceneManager;
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            SetupInput();

            networkManager = NetworkManager.Instance;
            sceneManager = SceneManager.Instance;
            
            AddChild(networkManager);
            AddChild(sceneManager);
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