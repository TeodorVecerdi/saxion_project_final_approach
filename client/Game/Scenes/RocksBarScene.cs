using game.ui;
using GXPEngine;
using Button = game.ui.Button;

namespace game {
    public class RocksBarScene : Scene {
        private ChatElement chatInstance;
        private JukeboxElement jukeboxInstance;

        public RocksBarScene() {
            SceneName = "Rock's-Bar";
        }

        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/bar.jpg")));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH / 3f, Globals.HEIGHT));
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/rocks/jukebox.jpg"));
            Root.AddChild(new Button(788, 207, 295, 300, "Open jukebox", ButtonStyle.Transparent, () => { jukeboxInstance.Initialize(); }));
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