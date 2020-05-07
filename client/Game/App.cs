using System;
using System.Collections.Generic;
using System.Drawing;
using game.ui;
using GXPEngine;
using GXPEngine.Core;

namespace game {
    public class App : Game {
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            SetupInput();
            
            var button = new Button(10, 10, 200, 40, "Hello, world!", () => {
                Debug.LogInfo("Button Clicked");
            });
            
            var textField = new TextField(10, 60, 300, 40, "Enter your username");
            var spriteTextField = new TextField(10, 110, 300, 40, "Enter your password");
            
            AddChild(button);
            AddChild(textField);
            AddChild(spriteTextField);
            var style = new ButtonStyle(textSizeNormal: 16f, textSizeHover: 24f, textSizePressed: 32f);
            AddChild(new Button(320, 50, 250, 100, "Click me!", style));
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