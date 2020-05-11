using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct NetworkPlayer {
        public string Username;
        public string GUID;
        public string RoomID;
        public string Location;
        public int AvatarIndex;
        public bool Consent;

        public NetworkPlayer(string username, string guid, string roomID, string location, int avatarIndex, bool consent) {
            Username = username;
            GUID = guid;
            RoomID = roomID;
            Location = location;
            AvatarIndex = avatarIndex;
            Consent = consent;
        }

        public JObject JSONObject => new JObject {["username"] = Username, ["avatar"] = AvatarIndex, ["guid"] = GUID, ["room"] = RoomID, ["location"] = Location, ["consent"] = Consent};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}