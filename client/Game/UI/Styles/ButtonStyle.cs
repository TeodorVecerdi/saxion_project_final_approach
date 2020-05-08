using System.Drawing;
using game.utils;

namespace game.ui {
    public struct ButtonStyle {
        private Color textNormal;
        private Color textHover;
        private Color textPressed;
        private Color backgroundNormal;
        private Color backgroundHover;
        private Color backgroundPressed;
        private Color borderNormal;
        private Color borderHover;
        private Color borderPressed;
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
            TextColor = textNormal;
            BackgroundColor = backgroundNormal;
            BorderColor = borderNormal;
            BorderSize = borderSizeNormal;
            TextSize = textSizeNormal;
            TextAlignment = textAlignmentNormal;
            Font = fontNormal;
        }

        public void Hover() {
            TextColor = textHover;
            BackgroundColor = backgroundHover;
            BorderColor = borderHover;
            BorderSize = borderSizeHover;
            TextSize = textSizeHover;
            TextAlignment = textAlignmentHover;
            Font = fontHover;
        }

        public void Press() {
            TextColor = textPressed;
            BackgroundColor = backgroundPressed;
            BorderColor = borderPressed;
            BorderSize = borderSizePressed;
            TextSize = textSizePressed;
            TextAlignment = textAlignmentPressed;
            Font = fontPressed;
        }

        public ButtonStyle(Color textNormal = default, Color textHover = default, Color textPressed = default, Color backgroundNormal = default, Color backgroundHover = default, Color backgroundPressed = default, Color borderNormal = default, Color borderHover = default, Color borderPressed = default, float borderSizeNormal = default, float borderSizeHover = default, float borderSizePressed = default, float textSizeNormal = default, float textSizeHover = default, float textSizePressed = default, StringFormat textAlignmentNormal = null, StringFormat textAlignmentHover = null, StringFormat textAlignmentPressed = null, FontLoader fontLoaderInstance = null) : this() {
            this.textNormal = textNormal == default ? Default.textNormal : textNormal;
            this.textHover = textHover == default ? Default.textHover : textHover;
            this.textPressed = textPressed == default ? Default.textPressed : textPressed;
            this.backgroundNormal = backgroundNormal == default ? Default.backgroundNormal : backgroundNormal;
            this.backgroundHover = backgroundHover == default ? Default.backgroundHover : backgroundHover;
            this.backgroundPressed = backgroundPressed == default ? Default.backgroundPressed : backgroundPressed;
            this.borderNormal = borderNormal == default ? Default.borderNormal : borderNormal;
            this.borderHover = borderHover == default ? Default.borderHover : borderHover;
            this.borderPressed = borderPressed == default ? Default.borderPressed : borderPressed;
            this.borderSizeNormal = borderSizeNormal == default ? Default.borderSizeNormal : borderSizeNormal;
            this.borderSizeHover = borderSizeHover == default ? Default.borderSizeHover : borderSizeHover;
            this.borderSizePressed = borderSizePressed == default ? Default.borderSizePressed : borderSizePressed;
            this.textSizeNormal = textSizeNormal == default ? Default.textSizeNormal : textSizeNormal;
            this.textSizeHover = textSizeHover == default ? Default.textSizeHover : textSizeHover;
            this.textSizePressed = textSizePressed == default ? Default.textSizePressed : textSizePressed;
            this.textAlignmentNormal = textAlignmentNormal ?? Default.textAlignmentNormal;
            this.textAlignmentHover = textAlignmentHover ?? Default.textAlignmentHover;
            this.textAlignmentPressed = textAlignmentPressed ?? Default.textAlignmentPressed;
            this.fontLoaderInstance = fontLoaderInstance ?? Default.fontLoaderInstance;
            Normal();
        }

        public static ButtonStyle Default = new ButtonStyle {
            textNormal = Color.FromArgb(255, 255, 255),
            textHover = Color.FromArgb(245, 245, 245),
            textPressed = Color.FromArgb(235, 235, 235),
            backgroundNormal = Color.FromArgb(41, 41, 55),
            backgroundHover = Color.FromArgb(51, 51, 65),
            backgroundPressed = Color.FromArgb(61, 61, 75),
            borderNormal = Color.FromArgb(30, 30, 45),
            borderHover = Color.FromArgb(41, 41, 55),
            borderPressed = Color.FromArgb(51, 51, 65),
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
            Font =  FontLoader.SourceCode[16f]
        };
    }
}