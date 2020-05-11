using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct ChatMessage {
        public string SenderName;
        public string SenderGUID;
        public string Message;
        
        public ChatMessage(string senderName, string senderGUID, string message) {
            SenderName = senderName;
            SenderGUID = senderGUID;
            Message = message;
        }
        
        public JObject JSONObject => new JObject {["fromName"] = SenderName, ["fromGUID"] = SenderGUID, ["message"] = Message};
        public string JSONString => JSONObject.ToString(Formatting.None);

        public static ChatMessage FromJSON(JObject json) {
            return new ChatMessage(json.Value<string>("fromName"), json.Value<string>("fromGUID"), json.Value<string>("message"));
        }
    }
}