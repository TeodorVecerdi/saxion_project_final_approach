using System.Drawing;
using System.Drawing.Printing;
using game.utils;

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
        private float caretWidthNormal;
        private float caretWidthFocused;
        private StringFormat textAlignmentNormal;
        private StringFormat textAlignmentFocused;
        private Font fontNormal => FontLoader.Instance[textSizeNormal];
        private Font fontFocused => FontLoader.Instance[textSizeFocused];

        public Color TextColor;
        public Color PlaceholderTextColor;
        public Color BackgroundColor;
        public Color BorderColor;
        public Color CaretColor;
        public float BorderSize;
        public float LeftMargin;
        public float TextSize;
        public float CaretWidth;
        public Font Font;
        public StringFormat TextAlignment;

        public void Normal() {
            TextColor = textNormal;
            PlaceholderTextColor = placeholderTextNormal;
            BackgroundColor = backgroundNormal;
            BorderColor = borderNormal;
            CaretColor = caretNormal;
            BorderSize = borderSizeNormal;
            LeftMargin = leftMarginNormal;
            TextSize = textSizeNormal;
            CaretWidth = caretWidthNormal;
            Font = fontNormal;
            TextAlignment = textAlignmentNormal;
        }

        public void Focus() {
            TextColor = textFocused;
            PlaceholderTextColor = placeholderTextFocused;
            BackgroundColor = backgroundFocused;
            BorderColor = borderFocused;
            CaretColor = caretFocused;
            BorderSize = borderSizeFocused;
            LeftMargin = leftMarginFocused;
            TextSize = textSizeFocused;
            CaretWidth = caretWidthFocused;
            Font = fontFocused;
            TextAlignment = textAlignmentFocused;
        }

        public TextFieldStyle(Color textNormal = default, Color textFocused = default, Color placeholderTextNormal = default, Color placeholderTextFocused = default, Color backgroundNormal = default, Color backgroundFocused = default, Color borderNormal = default, Color borderFocused = default, Color caretNormal = default, Color caretFocused = default, float borderSizeNormal = default, float borderSizeFocused = default, float leftMarginNormal = default, float leftMarginFocused = default, float textSizeNormal = default, float textSizeFocused = default, float caretWidthNormal = default, float caretWidthFocused = default, StringFormat textAlignmentNormal = null, StringFormat textAlignmentFocused = null) : this() {
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
            this.caretWidthNormal = caretWidthNormal == default ? Default.caretWidthNormal : caretWidthNormal;
            this.caretWidthFocused = caretWidthFocused == default ? Default.caretWidthFocused : caretWidthFocused;
            this.textAlignmentNormal = textAlignmentNormal ?? Default.textAlignmentNormal;
            this.textAlignmentFocused = textAlignmentFocused ?? Default.textAlignmentFocused;
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
            caretNormal = Color.FromArgb(0, 255, 215, 0),
            caretFocused = Color.FromArgb(255, 215, 0),
            borderSizeNormal = 2f,
            borderSizeFocused = 2f,
            leftMarginNormal = 8f,
            leftMarginFocused = 8f,
            textSizeNormal = 16f,
            textSizeFocused = 16f,
            caretWidthNormal = 2f,
            caretWidthFocused = 2f,
            textAlignmentNormal = FontLoader.LeftCenterAlignment,
            textAlignmentFocused = FontLoader.LeftCenterAlignment,

            TextColor = Color.FromArgb(255, 255, 255),
            PlaceholderTextColor = Color.FromArgb(192, 192, 192),
            BackgroundColor = Color.FromArgb(41, 41, 55),
            BorderColor = Color.FromArgb(30, 30, 45),
            CaretColor = Color.FromArgb(0, 255, 215, 0),
            BorderSize = 2f,
            LeftMargin = 8f,
            TextSize = 16f,
            CaretWidth = 2f,
            Font = FontLoader.Instance[16f],
            TextAlignment = FontLoader.LeftCenterAlignment
        };
    }
}