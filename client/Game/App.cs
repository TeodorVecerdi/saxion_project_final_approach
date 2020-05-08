using System.Collections.Generic;
using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.OpenGL;
using Button = game.ui.Button;
using Debug = GXPEngine.Debug;

namespace game {
    public class App : Game {
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            GL.ClearColor(255, 255, 255, 255);
            SetupInput();
            
            var button = new Button(10, 110, 200, 40, "LOG IN", () => {
                Debug.LogInfo("Log in");
            });
            
            var username = new TextField(10, 10, 300, 40, "Enter your username", onKeyTyped:key => Debug.LogInfo($"Key: {key}"));
            var password = new TextField(10, 60, 300, 40, "Enter your password");
            
            AddChild(button);
            AddChild(username);
            AddChild(password);
            var style = new ButtonStyle(textSizeNormal: 16f, textSizeHover: 18f, textSizePressed: 20f,
                backgroundNormal: Color.Aqua, backgroundHover: Color.Chocolate, backgroundPressed: Color.Firebrick,
                textNormal: Color.Indigo, textHover: Color.Turquoise, textPressed: Color.Navy,
                borderSizeNormal: 8f, borderSizeHover: 8f, borderSizePressed:8f);
            AddChild(new Button(320, 50, 250, 100, "You can even\nchange styles", style));
            var style2 = new ButtonStyle(fontLoaderInstance: FontLoader.FiraCode);
            AddChild(new Button(10, 210, 300, 50, "Source Code"));
            AddChild(new Button(320, 210, 300, 50, "Fira Code", style2));
            AddChild(new Label(10, 170, "Hello, World!"));
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