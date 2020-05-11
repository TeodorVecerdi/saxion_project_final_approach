using game.ui.Custom;
using GXPEngine;

namespace game {
    public class TestChatScene : Scene {
        private ChatElement chatInstance;

        public TestChatScene() {
            SceneName = "Chat";
        }
        
        public override void Load() {
            Root.AddChild(chatInstance = new ChatElement(0, 0, Globals.WIDTH/3f, Globals.HEIGHT, messageSent => {
                Debug.LogInfo($"Sent message {messageSent.Message}");
            }, messageReceived => {
                Debug.LogInfo($"Received message {messageReceived.Message}");
            }));
            ChatElement.ActiveChat = chatInstance;
            IsLoaded = true;
        }

        public override void Unload() {
            base.Unload();
            ChatElement.ActiveChat = null;
        }
    }
}