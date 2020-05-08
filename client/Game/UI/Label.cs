using System.Drawing;
using game.utils;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class Label : EasyDraw {
        private readonly Rectangle bounds;
        private string labelText;
        private LabelStyle labelStyle;

        public string LabelText {
            get => labelText;
            set {
                labelText = value;
                TextSize(labelStyle.TextSize);
                base.width = Mathf.Ceiling(TextWidth(labelText) * 1.5f);
                base.height = Mathf.Ceiling(TextHeight(labelText) * 1.5f);
                Draw();
            }
        }

        public Label(float x, float y, string labelText) 
            : this(x, y, Globals.WIDTH, Globals.HEIGHT, labelText, LabelStyle.Default) { }

        public Label(float x, float y, string labelText, LabelStyle labelStyle) 
            : this(x, y, Globals.WIDTH, Globals.HEIGHT, labelText, labelStyle) { }

        public Label(float x, float y, float width, float height, string labelText)
            : this(x, y, width, height, labelText, LabelStyle.Default) { }

        public Label(float x, float y, float width, float height, string labelText, LabelStyle labelStyle)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.labelText = labelText;
            this.labelStyle = labelStyle;
            Draw();
            SetXY(x, y);
        }

        private void Draw() {
            Clear(Color.Transparent);
            NoStroke();
            Fill(labelStyle.TextColor);
            graphics.DrawString(labelText, labelStyle.Font, brush, 0, 0, labelStyle.TextAlignment);
        }
    }
}