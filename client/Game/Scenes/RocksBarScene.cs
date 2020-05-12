using game.ui;
using GXPEngine;

namespace game {
    public class RocksBarScene : Scene {
        private ChatElement chatInstance;
        
        public RocksBarScene() {
            SceneName = "Rock's-Bar";
        }
                
        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/bar.jpg")));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH/3f, Globals.HEIGHT));
            ChatElement.ActiveChat = chatInstance;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
        }
    }
}