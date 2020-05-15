using GXPEngine;
using GXPEngine.Core;
using Image = game.ui.Image;

namespace game {
    public class LoadingScene : Scene {

        public LoadingScene() {
            SceneName = "Loading";
        }
        
        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new game.Sprite("data/sprites/loading.png")));
            var animSprite = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png"), 12, 1, 0.083F) {x = Globals.WIDTH / 2f - 64f, y = Globals.HEIGHT / 2f - 64f};
            Root.AddChild(animSprite);
            NetworkManager.Instance.Initialize();
            IsLoaded = true;
        }
    }
}