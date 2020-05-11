using System.Drawing;
using game.utils;

namespace game.ui {
    public struct ButtonStyle {
        private Color textColorNormal;
        private Color textColorHover;
        private Color textColorPressed;
        private Color backgroundColorNormal;
        private Color backgroundColorHover;
        private Color backgroundColorPressed;
        private Color borderColorNormal;
        private Color borderColorHover;
        private Color borderColorPressed;
        private float borderSizeNormal;
        private float borderSizeHover;
        private float borderSizePressed;
        private float textSizeNormal;
        private float textSizeHover;
        private float textSizePressed;
        private StringFormat textAlignmentNormal;
        private StringFormat textAlignmentHover;
        private StringFormat textAlignmentPressed;
        private FontLoader fontLoaderInstance;
        private Font fontNormal => fontLoaderInstance[textSizeNormal];
        private Font fontHover => fontLoaderInstance[textSizeHover];
        private Font fontPressed => fontLoaderInstance[textSizePressed];

        public Color TextColor;
        public Color BackgroundColor;
        public Color BorderColor;
        public float BorderSize;
        public float TextSize;
        public StringFormat TextAlignment;
        public Font Font;

        public void Normal() {
            TextColor = textColorNormal;
            BackgroundColor = backgroundColorNormal;
            BorderColor = borderColorNormal;
            BorderSize = borderSizeNormal;
            TextSize = textSizeNormal;
            TextAlignment = textAlignmentNormal;
            Font = fontNormal;
        }

        public void Hover() {
            TextColor = textColorHover;
            BackgroundColor = backgroundColorHover;
            BorderColor = borderColorHover;
            BorderSize = borderSizeHover;
            TextSize = textSizeHover;
            TextAlignment = textAlignmentHover;
            Font = fontHover;
        }

        public void Press() {
            TextColor = textColorPressed;
            BackgroundColor = backgroundColorPressed;
            BorderColor = borderColorPressed;
            BorderSize = borderSizePressed;
            TextSize = textSizePressed;
            TextAlignment = textAlignmentPressed;
            Font = fontPressed;
        }

        public ButtonStyle Alter(Color textColorNormal = default, Color textColorHover = default, Color textColorPressed = default, Color backgroundColorNormal = default, Color backgroundColorHover = default, Color backgroundColorPressed = default, Color borderColorNormal = default, Color borderColorHover = default, Color borderColorPressed = default, float borderSizeNormal = -1f, float borderSizeHover = -1f, float borderSizePressed = -1f, float textSizeNormal = -1f, float textSizeHover = -1f, float textSizePressed = -1f, StringFormat textAlignmentNormal = null, StringFormat textAlignmentHover = null, StringFormat textAlignmentPressed = null, FontLoader fontLoaderInstance = null) {
            var copy = this;
            copy.textColorNormal = textColorNormal == default ? this.textColorNormal : textColorNormal;
            copy.textColorHover = textColorHover == default ? this.textColorHover : textColorHover;
            copy.textColorPressed = textColorPressed == default ? this.textColorPressed : textColorPressed;
            copy.backgroundColorNormal = backgroundColorNormal == default ? this.backgroundColorNormal : backgroundColorNormal;
            copy.backgroundColorHover = backgroundColorHover == default ? this.backgroundColorHover : backgroundColorHover;
            copy.backgroundColorPressed = backgroundColorPressed == default ? this.backgroundColorPressed : backgroundColorPressed;
            copy.borderColorNormal = borderColorNormal == default ? this.borderColorNormal : borderColorNormal;
            copy.borderColorHover = borderColorHover == default ? this.borderColorHover : borderColorHover;
            copy.borderColorPressed = borderColorPressed == default ? this.borderColorPressed : borderColorPressed;
            copy.borderSizeNormal = borderSizeNormal == -1f ? this.borderSizeNormal : borderSizeNormal;
            copy.borderSizeHover = borderSizeHover == -1f ? this.borderSizeHover : borderSizeHover;
            copy.borderSizePressed = borderSizePressed == -1f ? this.borderSizePressed : borderSizePressed;
            copy.textSizeNormal = textSizeNormal == -1f ? this.textSizeNormal : textSizeNormal;
            copy.textSizeHover = textSizeHover == -1f ? this.textSizeHover : textSizeHover;
            copy.textSizePressed = textSizePressed == -1f ? this.textSizePressed : textSizePressed;
            copy.textAlignmentNormal = textAlignmentNormal ?? this.textAlignmentNormal;
            copy.textAlignmentHover = textAlignmentHover ?? this.textAlignmentHover;
            copy.textAlignmentPressed = textAlignmentPressed ?? this.textAlignmentPressed;
            copy.fontLoaderInstance = fontLoaderInstance ?? this.fontLoaderInstance;
            copy.Normal();
            return copy;
        }

        public ButtonStyle(Color textColorNormal = default, Color textColorHover = default, Color textColorPressed = default, Color backgroundColorNormal = default, Color backgroundColorHover = default, Color backgroundColorPressed = default, Color borderColorNormal = default, Color borderColorHover = default, Color borderColorPressed = default, float borderSizeNormal = -1f, float borderSizeHover = -1f, float borderSizePressed = -1f, float textSizeNormal = -1f, float textSizeHover = -1f, float textSizePressed = -1f, StringFormat textAlignmentNormal = null, StringFormat textAlignmentHover = null, StringFormat textAlignmentPressed = null, FontLoader fontLoaderInstance = null) : this() {
            this.textColorNormal = textColorNormal == default ? Default.textColorNormal : textColorNormal;
            this.textColorHover = textColorHover == default ? Default.textColorHover : textColorHover;
            this.textColorPressed = textColorPressed == default ? Default.textColorPressed : textColorPressed;
            this.backgroundColorNormal = backgroundColorNormal == default ? Default.backgroundColorNormal : backgroundColorNormal;
            this.backgroundColorHover = backgroundColorHover == default ? Default.backgroundColorHover : backgroundColorHover;
            this.backgroundColorPressed = backgroundColorPressed == default ? Default.backgroundColorPressed : backgroundColorPressed;
            this.borderColorNormal = borderColorNormal == default ? Default.borderColorNormal : borderColorNormal;
            this.borderColorHover = borderColorHover == default ? Default.borderColorHover : borderColorHover;
            this.borderColorPressed = borderColorPressed == default ? Default.borderColorPressed : borderColorPressed;
            this.borderSizeNormal = borderSizeNormal == -1f ? Default.borderSizeNormal : borderSizeNormal;
            this.borderSizeHover = borderSizeHover == -1f ? Default.borderSizeHover : borderSizeHover;
            this.borderSizePressed = borderSizePressed == -1f ? Default.borderSizePressed : borderSizePressed;
            this.textSizeNormal = textSizeNormal == -1f ? Default.textSizeNormal : textSizeNormal;
            this.textSizeHover = textSizeHover == -1f ? Default.textSizeHover : textSizeHover;
            this.textSizePressed = textSizePressed == -1f ? Default.textSizePressed : textSizePressed;
            this.textAlignmentNormal = textAlignmentNormal ?? Default.textAlignmentNormal;
            this.textAlignmentHover = textAlignmentHover ?? Default.textAlignmentHover;
            this.textAlignmentPressed = textAlignmentPressed ?? Default.textAlignmentPressed;
            this.fontLoaderInstance = fontLoaderInstance ?? Default.fontLoaderInstance;
            Normal();
        }

        public static ButtonStyle Default = new ButtonStyle {
            textColorNormal = Color.FromArgb(255, 255, 255),
            textColorHover = Color.FromArgb(245, 245, 245),
            textColorPressed = Color.FromArgb(235, 235, 235),
            backgroundColorNormal = Color.FromArgb(41, 41, 55),
            backgroundColorHover = Color.FromArgb(51, 51, 65),
            backgroundColorPressed = Color.FromArgb(61, 61, 75),
            borderColorNormal = Color.FromArgb(30, 30, 45),
            borderColorHover = Color.FromArgb(41, 41, 55),
            borderColorPressed = Color.FromArgb(51, 51, 65),
            borderSizeNormal = 2f,
            borderSizeHover = 2f,
            borderSizePressed = 2f,
            textSizeNormal = 16f,
            textSizeHover = 16f,
            textSizePressed = 16f,
            textAlignmentNormal = FontLoader.CenterCenterAlignment,
            textAlignmentHover = FontLoader.CenterCenterAlignment,
            textAlignmentPressed = FontLoader.CenterCenterAlignment,
            fontLoaderInstance = FontLoader.SourceCode,

            TextColor = Color.FromArgb(255, 255, 255),
            BackgroundColor = Color.FromArgb(41, 41, 55),
            BorderColor = Color.FromArgb(30, 30, 45),
            BorderSize = 2f,
            TextSize = 16f,
            TextAlignment = FontLoader.CenterCenterAlignment,
            Font = FontLoader.SourceCode[16f]
        };
    }
}