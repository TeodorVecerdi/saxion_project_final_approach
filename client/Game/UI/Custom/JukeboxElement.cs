using System.Collections.Generic;
using GXPEngine;
using GXPEngine.Core;

namespace game.ui {
    public class JukeboxElement : GameObject {
        public static JukeboxElement ActiveJukebox;
        private readonly Rectangle bounds;
        private readonly string backgroundPath;
        private readonly Pivot rootElement;
        public string CurrentlyPlaying = "";

        private string[] songs;
        private List<Rectangle> buttonPositions;

        public JukeboxElement(float x, float y, float width, float height, string backgroundPath, string[] songs, List<Rectangle> buttonPositions = null) {
            bounds = new Rectangle(x, y, width, height);
            this.backgroundPath = backgroundPath;
            rootElement = new Pivot();
            AddChild(rootElement);
            this.songs = songs;
            this.buttonPositions = buttonPositions ?? new List<Rectangle> { 
                new Rectangle(1383, 147, 138, 85), 
                new Rectangle(179 + 391, 278, 783, 91),
                new Rectangle(179 + 391, 378, 783, 91),
                new Rectangle(179 + 391, 478, 783, 91),
                new Rectangle(179 + 391, 578, 783, 91),
                new Rectangle(179 + 391, 678, 783, 91),
                new Rectangle(906, 789, 111, 111)
            };
            SetXY(x, y);
        }

        public void Initialize() {
            Deinitialize();
            rootElement.AddChild(new Image(x, y, bounds.width, bounds.height, new Sprite(backgroundPath, true, false)));
            rootElement.AddChild(new Button(buttonPositions[0].x, buttonPositions[0].y, buttonPositions[0].width, buttonPositions[0].height, "Back", ButtonStyle.Transparent, () => { Deinitialize(); }));
            rootElement.AddChild(new Button(buttonPositions[1].x, buttonPositions[1].y, buttonPositions[1].width, buttonPositions[1].height, "Song1", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = songs[0];
                NetworkManager.Instance.PlaySound(CurrentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(buttonPositions[2].x, buttonPositions[2].y, buttonPositions[2].width, buttonPositions[2].height, "Song2", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = songs[1];
                NetworkManager.Instance.PlaySound(CurrentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(buttonPositions[3].x, buttonPositions[3].y, buttonPositions[3].width, buttonPositions[3].height, "Song3", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = songs[2];
                NetworkManager.Instance.PlaySound(CurrentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(buttonPositions[4].x, buttonPositions[4].y, buttonPositions[4].width, buttonPositions[4].height, "Song4", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = songs[3];
                NetworkManager.Instance.PlaySound(CurrentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(buttonPositions[5].x, buttonPositions[5].y, buttonPositions[5].width, buttonPositions[5].height, "Song5", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = songs[4];
                NetworkManager.Instance.PlaySound(CurrentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(buttonPositions[6].x, buttonPositions[6].y, buttonPositions[6].width, buttonPositions[6].height, "Stop", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(CurrentlyPlaying);
                CurrentlyPlaying = "none";
            }));
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }
    }
}