using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine.Core;

namespace game.Scenes {
    public class LoadingScene : Scene {

        public LoadingScene() {
            SceneID = "Loading";
        }
        
        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 24f, FontLoader.CenterCenterAlignment);

            Root.AddChild(new Label(0,Globals.HEIGHT/2f - 128, Globals.WIDTH, 50, "Connecting to server...", primaryTitleStyle));
            Root.AddChild(new Label(0,Globals.HEIGHT/2f + 64, Globals.WIDTH, 50, "Please wait.", secondaryTitleStyle));
            var animSprite = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png"), 12, 1, 0.083F) {x = Globals.WIDTH / 2f - 64f, y = Globals.HEIGHT / 2f - 64f};
            Root.AddChild(animSprite);
            IsLoaded = true;
        }

        public override void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}