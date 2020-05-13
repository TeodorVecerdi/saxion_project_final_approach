using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct NetworkRoom {
        public string Name;
        public string Description;
        public string GUID;
        public string Code;
        public string Type;
        public bool IsPublic;
        public bool IsNSFW;
        public Dictionary<string, NetworkPlayer> Players;

        public NetworkRoom(string name, string description, string guid, string code, string type, bool isPublic, bool isNSFW) {
            Name = name;
            Description = description;
            GUID = guid;
            Code = code;
            Type = type;
            IsPublic = isPublic;
            IsNSFW = isNSFW;
            Players = new Dictionary<string, NetworkPlayer>();
        }

        public JObject JSONObject => new JObject {["name"] = Name, ["desc"] = Description, ["type"] = Type, ["code"] = Code, ["nsfw"] = IsNSFW, ["pub"] = IsPublic, ["guid"] = GUID};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}