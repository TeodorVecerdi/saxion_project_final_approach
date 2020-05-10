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

        public ButtonStyle(Color textColorNormal = default, Color textColorHover = default, Color textColorPressed = default, Color backgroundColorNormal = default, Color backgroundColorHover = default, Color backgroundColorPressed = default, Color borderColorNormal = default, Color borderColorHover = default, Color borderColorPressed = default, float borderSizeNormal = default, float borderSizeHover = default, float borderSizePressed = default, float textSizeNormal = default, float textSizeHover = default, float textSizePressed = default, StringFormat textAlignmentNormal = null, StringFormat textAlignmentHover = null, StringFormat textAlignmentPressed = null, FontLoader fontLoaderInstance = null) : this() {
            this.textColorNormal = textColorNormal == default ? Default.textColorNormal : textColorNormal;
            this.textColorHover = textColorHover == default ? Default.textColorHover : textColorHover;
            this.textColorPressed = textColorPressed == default ? Default.textColorPressed : textColorPressed;
            this.backgroundColorNormal = backgroundColorNormal == default ? Default.backgroundColorNormal : backgroundColorNormal;
            this.backgroundColorHover = backgroundColorHover == default ? Default.backgroundColorHover : backgroundColorHover;
            this.backgroundColorPressed = backgroundColorPressed == default ? Default.backgroundColorPressed : backgroundColorPressed;
            this.borderColorNormal = borderColorNormal == default ? Default.borderColorNormal : borderColorNormal;
            this.borderColorHover = borderColorHover == default ? Default.borderColorHover : borderColorHover;
            this.borderColorPressed = borderColorPressed == default ? Default.borderColorPressed : borderColorPressed;
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
            Font =  FontLoader.SourceCode[16f]
        };
    }
}