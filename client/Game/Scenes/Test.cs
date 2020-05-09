using game.ui;
using GXPEngine;

namespace game.Scenes {
    public class Test : Scene {
        public Test() {
            SceneID = "0";
        }
        
        public override void Load() {
            Root.AddChild(new TextField(20, Globals.HEIGHT/2f, 400, 50, ""));
            TextField messageTextfield = null;
            Root.AddChild(messageTextfield = new TextField(20, Globals.HEIGHT/2f + 60, 400, 50, "", onKeyTyped: key => {
                if(key != Key.ENTER) return;
                var message = messageTextfield.Text;
                Debug.Log($"Message sent: {message}");
                messageTextfield.Text = "";
            }));
        }

        public override void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}