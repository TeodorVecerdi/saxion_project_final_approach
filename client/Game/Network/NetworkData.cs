using System.Text;
using GXPEngine;

namespace game.network {
    public struct NetworkData {
        public string Name { get; set; }
        public string GUID { get; set; }
        public Vector2 Position { get; set; }

        public NetworkData(string name = "", string guid = "", Vector2 position = default) {
            Name = name;
            GUID = guid;
            Position = position;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            sb.AppendLine($"NetworkData[");
            sb.AppendLine($"  name: \"{Name}\",");
            sb.AppendLine($"  guid: \"{GUID}\",");
            sb.AppendLine($"  pos: {{");
            sb.AppendLine($"    x: {Position.x},");
            sb.AppendLine($"    y: {Position.y},");
            sb.AppendLine($"  }}");
            sb.AppendLine($"]");
            return sb.ToString();
        }
    }
}