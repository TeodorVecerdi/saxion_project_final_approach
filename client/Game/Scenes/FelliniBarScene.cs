using game.ui;
using GXPEngine;

namespace game {
    public class FelliniBarScene : Scene {
        private ChatElement chatInstance;
        
        public FelliniBarScene() {
            SceneName = "Fellini-Bar";
        }
                
        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/fellini/bar.png")));
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