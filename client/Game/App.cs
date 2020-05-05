using GXPEngine;

namespace game {
    public class App : Game{
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
        }

        public static void Main(string[] args) {
            new App().Start();
        }
    }
}