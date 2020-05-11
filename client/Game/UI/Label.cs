using System.Drawing;
using System.Drawing.Text;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class Label : EasyDraw {
        public string LabelText;
        private readonly Rectangle bounds;
        private readonly LabelStyle labelStyle;

        public bool ShouldRepaint { private get; set; }

        public Label(float x, float y, string labelText)
            : this(x, y, Globals.WIDTH, Globals.HEIGHT, labelText, LabelStyle.Default) { }

        public Label(float x, float y, string labelText, LabelStyle labelStyle)
            : this(x, y, Globals.WIDTH, Globals.HEIGHT, labelText, labelStyle) { }

        public Label(float x, float y, float width, float height, string labelText)
            : this(x, y, width, height, labelText, LabelStyle.Default) { }

        public Label(float x, float y, float width, float height, string labelText, LabelStyle labelStyle)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            LabelText = labelText;
            this.labelStyle = labelStyle;
            Draw();
            SetXY(x, y);
        }

        private void Update() {
            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);
            NoStroke();
            Fill(labelStyle.TextColor);
            var textX = 0f;
            var textY = 0f;
            if (labelStyle.TextAlignment.Alignment == StringAlignment.Far)
                textX += bounds.width;
            else if (labelStyle.TextAlignment.Alignment == StringAlignment.Center) textX += bounds.width / 2;
            if (labelStyle.TextAlignment.LineAlignment == StringAlignment.Far)
                textY += bounds.height;
            else if (labelStyle.TextAlignment.LineAlignment == StringAlignment.Center) textY += bounds.height / 2;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.DrawString(LabelText, labelStyle.Font, brush, new RectangleF(0, 0, bounds.width, bounds.height), labelStyle.TextAlignment);
        }
    }
}