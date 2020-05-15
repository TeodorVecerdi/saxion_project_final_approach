using System.Drawing;
using game.utils;

namespace game {
    public class Sprite : GXPEngine.Sprite {
        public Sprite(Bitmap bitmap, bool addCollider = true) : base(new Bitmap(bitmap), addCollider) {
        }
        public Sprite(string filename, bool keepInCache = false, bool addCollider = true) : base(Utils.ReadBitmapFromFile(filename), addCollider) {
        }
    }
}