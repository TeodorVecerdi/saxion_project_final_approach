using game.ui;
using GXPEngine;

namespace game {
    public class CoffeeBarScene : Scene {
        private ChatElement chatInstance;
        
        public CoffeeBarScene() {
            SceneName = "Coffee Fellows-Bar";
        }
                
        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/coffee_fellows/bar.jpg")));
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