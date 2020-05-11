using System.Drawing;

namespace game.ui {
    public struct CheckboxStyle {
        private Color backgroundColorNormal;
        private Color backgroundColorHover;
        private Color backgroundColorPressed;
        private Color borderColorNormal;
        private Color borderColorHover;
        private Color borderColorPressed;
        private Color tickColorNormal;
        private Color tickColorHover;
        private Color tickColorPressed;
        private float borderSizeNormal;
        private float borderSizeHover;
        private float borderSizePressed;
        private float labelOffsetNormal;
        private float labelOffsetHover;
        private float labelOffsetPressed;

        public Color BackgroundColor;
        public Color BorderColor;
        public Color TickColor;
        public float BorderSize;
        public float LabelOffset;

        public void Normal() {
            BackgroundColor = backgroundColorNormal;
            BorderColor = borderColorNormal;
            BorderSize = borderSizeNormal;
            LabelOffset = labelOffsetNormal;
            TickColor = tickColorNormal;
        }

        public void Hover() {
            BackgroundColor = backgroundColorHover;
            BorderColor = borderColorHover;
            BorderSize = borderSizeHover;
            LabelOffset = labelOffsetHover;
            TickColor = tickColorHover;
        }

        public void Press() {
            BackgroundColor = backgroundColorPressed;
            BorderColor = borderColorPressed;
            BorderSize = borderSizePressed;
            LabelOffset = labelOffsetPressed;
            TickColor = tickColorPressed;
        }

        public CheckboxStyle Alter(Color backgroundColorNormal = default, Color borderColorNormal = default, float borderSizeNormal = default, Color backgroundColorHover = default, Color backgroundColorPressed = default, Color borderColorHover = default, Color borderColorPressed = default, float borderSizeHover = default, float borderSizePressed = default, float labelOffsetNormal = default, float labelOffsetHover = default, float labelOffsetPressed = default, Color tickColorNormal = default, Color tickColorHover = default, Color tickColorPressed = default) {
            var copy = this;
            copy.backgroundColorNormal = backgroundColorNormal == default ? this.backgroundColorNormal : backgroundColorNormal;
            copy.borderColorNormal = borderColorNormal == default ? this.borderColorNormal : borderColorNormal;
            copy.borderSizeNormal = borderSizeNormal == default ? this.borderSizeNormal : borderSizeNormal;
            copy.backgroundColorHover = backgroundColorHover == default ? this.backgroundColorHover : backgroundColorHover;
            copy.borderColorHover = borderColorHover == default ? this.borderColorHover : borderColorHover;
            copy.borderSizeHover = borderSizeHover == default ? this.borderSizeHover : borderSizeHover;
            copy.backgroundColorPressed = backgroundColorPressed == default ? this.backgroundColorPressed : backgroundColorPressed;
            copy.borderColorPressed = borderColorPressed == default ? this.borderColorPressed : borderColorPressed;
            copy.borderSizePressed = borderSizePressed == default ? this.borderSizePressed : borderSizePressed;
            copy.labelOffsetNormal = labelOffsetNormal == default ? this.labelOffsetNormal : labelOffsetNormal;
            copy.labelOffsetHover = labelOffsetHover == default ? this.labelOffsetHover : labelOffsetHover;
            copy.labelOffsetPressed = labelOffsetPressed == default ? this.labelOffsetPressed : labelOffsetPressed;
            copy.tickColorNormal = tickColorNormal == default ? this.tickColorNormal : tickColorNormal;
            copy.tickColorHover = tickColorHover == default ? this.tickColorHover : tickColorHover;
            copy.tickColorPressed = tickColorPressed == default ? this.tickColorPressed : tickColorPressed;
            copy.Normal();
            return copy;
        }

        public CheckboxStyle(Color backgroundColorNormal = default, Color borderColorNormal = default, float borderSizeNormal = default, Color backgroundColorHover = default, Color backgroundColorPressed = default, Color borderColorHover = default, Color borderColorPressed = default, float borderSizeHover = default, float borderSizePressed = default, float labelOffsetNormal = default, float labelOffsetHover = default, float labelOffsetPressed = default, Color tickColorNormal = default, Color tickColorHover = default, Color tickColorPressed = default) : this() {
            this.backgroundColorNormal = backgroundColorNormal == default ? Default.backgroundColorNormal : backgroundColorNormal;
            this.borderColorNormal = borderColorNormal == default ? Default.borderColorNormal : borderColorNormal;
            this.borderSizeNormal = borderSizeNormal == default ? Default.borderSizeNormal : borderSizeNormal;
            this.backgroundColorHover = backgroundColorHover == default ? Default.backgroundColorHover : backgroundColorHover;
            this.borderColorHover = borderColorHover == default ? Default.borderColorHover : borderColorHover;
            this.borderSizeHover = borderSizeHover == default ? Default.borderSizeHover : borderSizeHover;
            this.backgroundColorPressed = backgroundColorPressed == default ? Default.backgroundColorPressed : backgroundColorPressed;
            this.borderColorPressed = borderColorPressed == default ? Default.borderColorPressed : borderColorPressed;
            this.borderSizePressed = borderSizePressed == default ? Default.borderSizePressed : borderSizePressed;
            this.labelOffsetNormal = labelOffsetNormal == default ? Default.labelOffsetNormal : labelOffsetNormal;
            this.labelOffsetHover = labelOffsetHover == default ? Default.labelOffsetHover : labelOffsetHover;
            this.labelOffsetPressed = labelOffsetPressed == default ? Default.labelOffsetPressed : labelOffsetPressed;
            this.tickColorNormal = tickColorNormal == default ? Default.tickColorNormal : tickColorNormal;
            this.tickColorHover = tickColorHover == default ? Default.tickColorHover : tickColorHover;
            this.tickColorPressed = tickColorPressed == default ? Default.tickColorPressed : tickColorPressed;
            Normal();
        }

        public static CheckboxStyle Default = new CheckboxStyle {
            backgroundColorNormal = Color.FromArgb(41, 41, 55),
            borderColorNormal = Color.FromArgb(30, 30, 45),
            borderSizeNormal = 2f,
            backgroundColorHover = Color.FromArgb(51, 51, 65),
            borderColorHover = Color.FromArgb(41, 41, 55),
            borderSizeHover = 2f,
            backgroundColorPressed = Color.FromArgb(61, 61, 75),
            borderColorPressed = Color.FromArgb(51, 51, 65),
            borderSizePressed = 2f,
            labelOffsetNormal = 8f,
            labelOffsetHover = 8f,
            labelOffsetPressed = 8f,
            tickColorNormal = Color.FromArgb(255, 255, 255, 255),
            tickColorHover = Color.FromArgb(255, 255, 255, 255),
            tickColorPressed = Color.FromArgb(255, 255, 255, 255),

            BackgroundColor = Color.FromArgb(41, 41, 55),
            BorderColor = Color.FromArgb(30, 30, 45),
            BorderSize = 2f,
            LabelOffset = 8f,
            TickColor = Color.FromArgb(255, 255, 255, 255)
        };
    }
}