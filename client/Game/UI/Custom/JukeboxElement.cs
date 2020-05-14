using GXPEngine;
using GXPEngine.Core;

namespace game.ui {
    public class JukeboxElement : GameObject {
        public static JukeboxElement ActiveJukebox;
        private readonly Rectangle bounds;
        private readonly string backgroundPath;
        private readonly Pivot rootElement;
        private string currentlyPlaying;

        public JukeboxElement(float x, float y, float width, float height, string backgroundPath) {
            bounds = new Rectangle(x, y, width, height);
            this.backgroundPath = backgroundPath;
            rootElement = new Pivot();
            AddChild(rootElement);
            SetXY(x, y);
        }

        public void Initialize() {
            Deinitialize();
            rootElement.AddChild(new Image(x, y, bounds.width, bounds.height, new Sprite(backgroundPath, true, false)));
            rootElement.AddChild(new Button(1383, 147, 138, 85, "Back", ButtonStyle.Transparent, () => { Deinitialize(); }));
            rootElement.AddChild(new Button(179 + 391, 278, 783, 91, "Song1", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "Song1";
                NetworkManager.Instance.PlaySound(currentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(179 + 391, 378, 783, 91, "Song2", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "Song2";
                NetworkManager.Instance.PlaySound(currentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(179 + 391, 478, 783, 91, "Song3", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "Song3";
                NetworkManager.Instance.PlaySound(currentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(179 + 391, 578, 783, 91, "Song4", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "Song4";
                NetworkManager.Instance.PlaySound(currentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(179 + 391, 678, 783, 91, "Song5", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "Song5";
                NetworkManager.Instance.PlaySound(currentlyPlaying, true);
            }));
            rootElement.AddChild(new Button(906, 789, 111, 111, "Stop", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "none";
            }));
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }
    }
}