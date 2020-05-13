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
            SetXY(x, y);
        }

        public void Initialize() {
            Deinitialize();
            rootElement.AddChild(new Image(x, y, bounds.width, bounds.height, new Sprite(backgroundPath, true, false)));
            rootElement.AddChild(new Button(1383-391, 147-137, 138, 85, "Back", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.StopPlayingSound(currentlyPlaying);
                currentlyPlaying = "none";
            }));
            rootElement.AddChild(new Button(179, 278-137, 783, 91, "Song1", ButtonStyle.Transparent, () => {
                currentlyPlaying = "Song1";
                NetworkManager.Instance.PlaySound("Song1", true);
            }));
            rootElement.AddChild(new Button(179, 378-137, 783, 91, "Song2", ButtonStyle.Transparent, () => {
                currentlyPlaying = "Song2";
                NetworkManager.Instance.PlaySound("Song2", true);
            }));
            rootElement.AddChild(new Button(179, 478-137, 783, 91, "Song3", ButtonStyle.Transparent, () => {
                currentlyPlaying = "Song3";
                NetworkManager.Instance.PlaySound("Song3", true);
            }));
            rootElement.AddChild(new Button(179, 578-137, 783, 91, "Song4", ButtonStyle.Transparent, () => {
                currentlyPlaying = "Song4";
                NetworkManager.Instance.PlaySound("Song4", true);
            }));
            rootElement.AddChild(new Button(179, 678-137, 783, 91, "Song5", ButtonStyle.Transparent, () => {
                currentlyPlaying = "Song5";
                NetworkManager.Instance.PlaySound("Song5", true);
            }));
            rootElement.AddChild(new Button(906-391, 789-137, 111, 111, "Stop", ButtonStyle.Transparent, () => {
                Deinitialize();
            }));
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }
    }
}