using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;

namespace game {
    public class RocksBarScene : Scene {
        private ChatElement chatInstance;
        private JukeboxElement jukeboxInstance;
        private Minigame2Element minigame2Element;

        public RocksBarScene() {
            SceneName = "Rock's-Bar";
        }

        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/bar.jpg")));
            Root.AddChild(new Button(Globals.WIDTH / 3f + 20, 40, 200, 40, "Start minigame", () => { NetworkManager.Instance.StartMinigame2(); }));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH / 3f, Globals.HEIGHT));
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/rocks/jukebox.jpg"));
            Root.AddChild(minigame2Element = new Minigame2Element(730f, 314f, 1100f, 450f, LabelStyle.Default.Alter(textSizeNormal: 32f, textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textSizeNormal: 12f, textColorNormal: Color.Yellow, textAlignmentNormal: FontLoader.CenterTopAlignment), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.LeftCenterAlignment, textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.Yellow), ButtonStyle.Default, ButtonStyle.Default));
            Root.AddChild(new Button(788, 207, 295, 300, "Open jukebox", ButtonStyle.Transparent, () => { jukeboxInstance.Initialize(); }));
            ChatElement.ActiveChat = chatInstance;
            JukeboxElement.ActiveJukebox = jukeboxInstance;
            Minigame2Element.ActiveMinigame = minigame2Element;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
        }
    }
}