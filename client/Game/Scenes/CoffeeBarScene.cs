using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;

namespace game {
    public class CoffeeBarScene : Scene {
        private ChatElement chatInstance;
        private JukeboxElement jukeboxInstance;
        private Minigame3Element minigame3Element;

        public CoffeeBarScene() {
            SceneName = "Coffee Fellows-Bar";
        }

        public override void Load() {
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new game.Sprite("data/sprites/locations/coffee_fellows/bar.jpg")));
            Root.AddChild(new Button(Globals.WIDTH / 3f + 20, 40, 200, 40, "Start minigame", () => { NetworkManager.Instance.StartMinigame3(); }));
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH / 3f, Globals.HEIGHT));
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/coffee_fellows/jukebox.jpg",
                new []{"Song1","Song2","Song3","Song4","Song5"}));
            Root.AddChild(new Button(916, 420, 255, 110, "Open jukebox", ButtonStyle.Transparent, () => {jukeboxInstance.Initialize();}));
            Root.AddChild(minigame3Element = new Minigame3Element(730f, 314f, 1100f, 450f, LabelStyle.Default.Alter(textSizeNormal: 24f, textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.White), LabelStyle.Default.Alter(textColorNormal: Color.White), LabelStyle.Default.Alter(textSizeNormal: 12f, textColorNormal: Color.White, textAlignmentNormal: FontLoader.CenterTopAlignment), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.LeftCenterAlignment, textColorNormal: Color.White), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: Color.White), ButtonStyle.Transparent,ButtonStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(255,73,73,81), backgroundColorHover:Color.FromArgb(255,73,73,81), backgroundColorPressed: Color.FromArgb(255,73,73,81), borderSizeHover: 4, borderSizeNormal: 4, borderSizePressed: 4, borderColorNormal:Color.White, borderColorHover:Color.White, borderColorPressed:Color.White, textColorHover:Color.Transparent, textColorNormal:Color.Transparent, textColorPressed:Color.Transparent)));
            Root.AddChild(new Button(10, 10, Globals.WIDTH/3f - 20f, 40, "Leave room", () => {
                SoundManager.Instance.StopPlaying(jukeboxInstance.CurrentlyPlaying);
                NetworkManager.Instance.LeaveRoom();
            }));
            ChatElement.ActiveChat = chatInstance;
            JukeboxElement.ActiveJukebox = jukeboxInstance;
            Minigame3Element.ActiveMinigame = minigame3Element;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
            Minigame3Element.ActiveMinigame = null;
            JukeboxElement.ActiveJukebox = null;
        }
    }
}