using System.Drawing;
using System.Drawing.Printing;

namespace game.ui {
    public struct TextFieldStyle {
        private Color textNormal;
        private Color textFocused;
        private Color placeholderTextNormal;
        private Color placeholderTextFocused;
        private Color backgroundNormal;
        private Color backgroundFocused;
        private Color borderNormal;
        private Color borderFocused;
        private Color caretNormal;
        private Color caretFocused;
        private float borderSizeNormal;
        private float borderSizeFocused;
        private float leftMarginNormal;
        private float leftMarginFocused;
        private float textSizeNormal;
        private float textSizeFocused;

        public Color Text;
        public Color PlaceholderText;
        public Color Background;
        public Color Border;
        public Color Caret;
        public float BorderSize;
        public float LeftMargin;
        public float TextSize;

        public void Normal() {
            Text = textNormal;
            PlaceholderText = placeholderTextNormal;
            Background = backgroundNormal;
            Border = borderNormal;
            Caret = caretNormal;
            BorderSize = borderSizeNormal;
            LeftMargin = leftMarginNormal;
            TextSize = textSizeNormal;
        }
        public void Focus() {
            Text = textFocused;
            PlaceholderText = placeholderTextFocused;
            Background = backgroundFocused;
            Border = borderFocused;
            Caret = caretFocused;
            BorderSize = borderSizeFocused;
            LeftMargin = leftMarginFocused;
            TextSize = textSizeFocused;
        }

        public TextFieldStyle(Color textNormal = default, Color textFocused = default, Color placeholderTextNormal = default, Color placeholderTextFocused = default, Color backgroundNormal = default, Color backgroundFocused = default, Color borderNormal = default, Color borderFocused = default, Color caretNormal = default, Color caretFocused = default, float borderSizeNormal = default, float borderSizeFocused = default, float leftMarginNormal = default, float leftMarginFocused = default, float textSizeNormal = default, float textSizeFocused = default) : this() {
            this.textNormal = textNormal == default ? Default.textNormal : textNormal;
            this.textFocused = textFocused == default ? Default.textFocused : textFocused;
            this.placeholderTextNormal = placeholderTextNormal == default ? Default.placeholderTextNormal : placeholderTextNormal;
            this.placeholderTextFocused = placeholderTextFocused == default ? Default.placeholderTextFocused : placeholderTextFocused;
            this.backgroundNormal = backgroundNormal == default ? Default.backgroundNormal : backgroundNormal;
            this.backgroundFocused = backgroundFocused == default ? Default.backgroundFocused : backgroundFocused;
            this.borderNormal = borderNormal == default ? Default.borderNormal : borderNormal;
            this.borderFocused = borderFocused == default ? Default.borderFocused : borderFocused;
            this.caretNormal = caretNormal == default ? Default.caretNormal : caretNormal;
            this.caretFocused = caretFocused == default ? Default.caretFocused : caretFocused;
            this.borderSizeNormal = borderSizeNormal == default ? Default.borderSizeNormal : borderSizeNormal;
            this.borderSizeFocused = borderSizeFocused == default ? Default.borderSizeFocused : borderSizeFocused;
            this.leftMarginNormal = leftMarginNormal == default ? Default.leftMarginNormal : leftMarginNormal;
            this.leftMarginFocused = leftMarginFocused == default ? Default.leftMarginFocused : leftMarginFocused;
            this.textSizeNormal = textSizeNormal == default ? Default.textSizeNormal : textSizeNormal;
            this.textSizeFocused = textSizeFocused == default ? Default.textSizeFocused : textSizeFocused;
            Normal();
        }

        public static TextFieldStyle Default = new TextFieldStyle {
            textNormal = Color.FromArgb(255, 255, 255),
            textFocused = Color.FromArgb(255, 255, 255),
            placeholderTextNormal = Color.FromArgb(192, 192, 192),
            placeholderTextFocused = Color.FromArgb(128, 128, 128),
            backgroundNormal = Color.FromArgb(41, 41, 55),
            backgroundFocused = Color.FromArgb(41, 41, 55),
            borderNormal = Color.FromArgb(30, 30, 45),
            borderFocused = Color.FromArgb(47, 47, 65),
            caretNormal = Color.FromArgb(0, 255,215,0),
            caretFocused = Color.FromArgb(255,215,0),
            borderSizeNormal = 2f,
            borderSizeFocused = 2f,
            leftMarginNormal = 8f,
            leftMarginFocused = 8f,
            textSizeNormal = 16f,
            textSizeFocused = 16f,
            
            Text = Color.FromArgb(255, 255, 255),
            PlaceholderText = Color.FromArgb(192, 192, 192),
            Background = Color.FromArgb(41, 41, 55),
            Border = Color.FromArgb(30, 30, 45),
            Caret = Color.FromArgb(0, 255,215,0),
            BorderSize = 2f,
            LeftMargin = 8f,
            TextSize = 16f
        };
    }
}