using System.Drawing;
using game.ui;
using game.utils;
using Debug = GXPEngine.Debug;

namespace game {
    public class MapScene : Scene {
        public MapScene() {
            SceneID = "Map";
        }
        
        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            Root.AddChild(new Label(40, 20, Globals.WIDTH - 80, 40, "CHOOSE A LOCATION", primaryTitleStyle));
            Root.AddChild(new Button(40, 80, Globals.WIDTH - 80f, 40, "Fellini", onClick: () => {
                NetworkManager.Instance.JoinLocation("Fellini");
            }));
            Root.AddChild(new Button(40, 140, Globals.WIDTH - 80f, 40, "Rock's", onClick: () => {
                NetworkManager.Instance.JoinLocation("Rock's");
            }));
            Root.AddChild(new Button(40, 200, Globals.WIDTH - 80f, 40, "Coffee Fellows", onClick: () => {
                NetworkManager.Instance.JoinLocation("Coffee Fellows");
            }));
            IsLoaded = true;
        }

        public override void Unload() {
            IsLoaded = false;
            Root.GetChildren().ForEach(child => child.LateDestroy());
        }
    }
}