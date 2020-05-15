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
            // READ NOTES
            Debug.LogError($"Never have I ever yes/no icon doesn't validate input and if someone leaves the room while the minigame is active results in .../Icon.png");
            Debug.LogError($"^^^^^^^^^^^^^^^^^^^^ Also check in other places where this might happen!");
            Debug.LogError($"OutOfMemory exception in CoffeeMenuScene line 45 with backgroundImage3. Possibly move static elements from being created on load to being shown in load and hidden in unload instead.");
            Environment.Exit(0);
            // targetFps = 60;
            Debug.EnableFileLogger(true);
            // ShowMouse(true);
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