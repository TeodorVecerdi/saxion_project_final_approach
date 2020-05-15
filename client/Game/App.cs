using System;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Debug = game.utils.Debug;

namespace game {
    public class App : Game {
        private readonly MouseCursor mouseCursor;
        private readonly NetworkManager networkManager;
        private readonly SceneManager sceneManager;
        private readonly SoundManager soundManager;

        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            // Debug.LogWarning("TODO: Enable logging on server");
            // Debug.LogWarning("TODO: Add website to server (using express)");
            // Debug.LogWarning("TODO: Change the loading assets text to something like `Go grab a beer, get comfortable and bla bla bla...`");
            // Debug.LogWarning("TODO: Change scenes to keep static assets loaded and just hidden off-screen instead of reloading assets every time there is a scene change");
            // Environment.Exit(0);
            // targetFps = 60;
            Debug.EnableFileLogger(true);
            ShowMouse(false);
            SetupInput();
            

            networkManager = NetworkManager.Instance;
            sceneManager = SceneManager.Instance;
            mouseCursor = MouseCursor.Instance;
            soundManager = SoundManager.Instance;

            AddChild(networkManager);
            AddChild(sceneManager);
            AddChild(soundManager);
            AddChild(mouseCursor);
            
            sceneManager.LoadScene("Loading");
        }

        private void SetupInput() {
            // Input.AddAxis("Horizontal", new List<int>{Key.LEFT, Key.A}, new List<int>{Key.RIGHT, Key.D});
            // Input.AddAxis("Vertical", new List<int>{Key.DOWN, Key.S}, new List<int>{Key.UP, Key.W}, true);
        }

        public static void Main(string[] args) {
            try {
                new App().Start();
            } catch (Exception e) {
                Debug.LogError($"An error occured: {e.Message}.\nYou can contact the developer Teodor Vecerdi (475884@student.saxion.nl).\nStacktrace:\n{e.StackTrace}");
                throw;
            } finally {
                Debug.FinalizeLogger();
            }
        }
    }
}