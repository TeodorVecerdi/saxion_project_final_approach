using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;

namespace game {
    public class FelliniBarScene : Scene {
        private ChatElement chatInstance;
        private Minigame1Element minigame1Element;
        private JukeboxElement jukeboxInstance;

        public FelliniBarScene() {
            SceneName = "Fellini-Bar";
        }

        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/fellini/bar.png")));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH / 3f, Globals.HEIGHT));
            Root.AddChild(new Button(Globals.WIDTH / 3f + 20, 40, 200, 40, "Start minigame", () => { NetworkManager.Instance.StartMinigame1(); }));
            Root.AddChild(minigame1Element = new Minigame1Element(730f, 314f, 1100f, 450f, LabelStyle.Default.Alter(textSizeNormal: 32f, textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textSizeNormal: 12f, textColorNormal: Color.Yellow, textAlignmentNormal: FontLoader.CenterTopAlignment), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.LeftCenterAlignment, textColorNormal: Color.Yellow), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.Yellow), ButtonStyle.Transparent));
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/fellini/jukebox.jpg"));
            Root.AddChild(new Button(1642, 490, 246, 310, "Open jukebox", ButtonStyle.Transparent, () => {jukeboxInstance.Initialize();}));
            ChatElement.ActiveChat = chatInstance;
            Minigame1Element.ActiveMinigame = minigame1Element;
            JukeboxElement.ActiveJukebox = jukeboxInstance;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
        }
    }
}