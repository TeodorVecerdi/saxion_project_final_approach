using System;
using System.Drawing;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class MouseHotspot : EasyDraw {
        private readonly Rectangle bounds;
        private readonly Action onClick;
        private bool shouldDraw;
        public bool ShouldDraw {
            get => shouldDraw;
            set {
                if (value) Draw();
                else Clear(Color.Transparent);
            }
        }

        public bool IsMouseOnTop => Input.mouseX >= bounds.x && Input.mouseX <= bounds.x + bounds.width && Input.mouseY >= bounds.y && Input.mouseY <= bounds.y + bounds.height;

        public MouseHotspot(Vector2 from, Vector2 to, Action onClick = null, bool shouldDraw = false) : this(from.x, from.y, to.x - from.x, to.y - from.y, onClick, shouldDraw) { }

        public MouseHotspot(float x, float y, float width, float height, Action onClick = null, bool shouldDraw = false) : base((int) width, (int) height, false) {
            this.onClick = onClick;
            bounds = new Rectangle(x, y, width, height);
            position.Set(x, y);
            ShouldDraw = shouldDraw;
        }

        public void Update() {
            if (Input.GetMouseButtonDown(0) && IsMouseOnTop)
                onClick.Invoke();
        }

        private void Draw() {
            Clear(Color.Transparent);
            Fill(Color.FromArgb(127, 127, 127, 127));
            Rect(x, y, width, height);
        }
    }
}