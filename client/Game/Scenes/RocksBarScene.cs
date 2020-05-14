using System.Collections.Generic;
using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;
using Rectangle = GXPEngine.Core.Rectangle;

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
            Root.AddChild(jukeboxInstance = new JukeboxElement(0, 0, Globals.WIDTH, Globals.HEIGHT, "data/sprites/locations/rocks/jukebox.png",
                new []{"Song11","Song12","Song13","Song14","Song15"},
                new List<Rectangle> {
                    new Rectangle(1392, 178, 112, 106),
                    new Rectangle(616, 315, 637, 111),
                    new Rectangle(616, 437, 637, 111),
                    new Rectangle(616, 560, 637, 111),
                    new Rectangle(616, 683, 637, 111),
                    new Rectangle(616, 806, 637, 111),
                    new Rectangle(1292, 796, 128, 121)
                }));
            var brownColor = Color.FromArgb(255, 64, 51, 31);
            var lightColor = Color.FromArgb(255, 239, 237, 225);
            var lightColor2 = Color.FromArgb(255, 215, 213, 201);
            Root.AddChild(minigame2Element = new Minigame2Element(730f, 500f, 1100f, 450f, LabelStyle.Default.Alter(textSizeNormal: 32f, textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: brownColor), LabelStyle.Default.Alter(textColorNormal: lightColor, textSizeNormal: 24f), LabelStyle.Default.Alter(textColorNormal: lightColor, textAlignmentNormal: FontLoader.CenterCenterAlignment), LabelStyle.Default.Alter(textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: lightColor), LabelStyle.Default.Alter(textSizeNormal: 24f, textAlignmentNormal: FontLoader.CenterCenterAlignment, textColorNormal: brownColor), ButtonStyle.Transparent, ButtonStyle.Transparent.Alter(backgroundColorNormal: lightColor, backgroundColorHover: lightColor2, backgroundColorPressed: lightColor2)));
            Root.AddChild(new Button(788, 207, 295, 300, "Open jukebox", ButtonStyle.Transparent, () => { jukeboxInstance.Initialize(); }));
            Root.AddChild(new Button(10, 10, Globals.WIDTH / 3f - 20f, 40, "Leave room", () => {
                SoundManager.Instance.StopPlaying(jukeboxInstance.CurrentlyPlaying);
                NetworkManager.Instance.LeaveRoom();
            }));
            ChatElement.ActiveChat = chatInstance;
            JukeboxElement.ActiveJukebox = jukeboxInstance;
            Minigame2Element.ActiveMinigame = minigame2Element;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
            Minigame2Element.ActiveMinigame = null;
            JukeboxElement.ActiveJukebox = null;
        }
    }
}