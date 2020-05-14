using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;

namespace game {
    public class CoffeeBarScene : Scene {
        private ChatElement chatInstance;
        private JukeboxElement jukeboxInstance;

        public CoffeeBarScene() {
            SceneName = "Coffee Fellows-Bar";
        }

        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/coffee_fellows/bar.jpg")));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH / 3f, Globals.HEIGHT));
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/coffee_fellows/jukebox.jpg",
                new []{"Song1","Song2","Song3","Song4","Song5"}));
            Root.AddChild(new Button(916, 420, 255, 110, "Open jukebox", ButtonStyle.Transparent, () => {jukeboxInstance.Initialize();}));
            Root.AddChild(new Button(10, 10, Globals.WIDTH/3f - 20f, 40, "Leave room", () => {
                SoundManager.Instance.StopPlaying(jukeboxInstance.CurrentlyPlaying);
                NetworkManager.Instance.LeaveRoom();
            }));
            ChatElement.ActiveChat = chatInstance;
            JukeboxElement.ActiveJukebox = jukeboxInstance;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
        }
    }
}