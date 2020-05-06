using System.Drawing;
using GXPEngine;

namespace game.network {
    public class NetworkPlayer : EasyDraw {
        public string Name;
        public string GUID;
        public Vector2 Position;

        public NetworkPlayer(string name, string guid) : this(name, guid, Vector2.zero) { }

        public NetworkPlayer(string name, string guid, Vector2 position) : base(Globals.WIDTH, Globals.HEIGHT)  {
            Name = name;
            GUID = guid;
            Position = position;
            Draw();
        }

        public void UpdateClient(NetworkData data) {
            Position = data.Position;
            // Draw();
        }

        public void Draw() {
            Clear(Color.Transparent);
            Fill(Color.Teal);
            NoStroke();
            Rect(Position.x, Position.y, 64f, 64f);
            Fill(Color.Chartreuse);
            TextSize(18f);
            TextAlign(CenterMode.Center, CenterMode.Max);
            Text(Name, Position.x, Position.y - 36f);
        }
    }
}