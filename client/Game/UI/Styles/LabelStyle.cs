using System.Drawing;
using game.utils;

namespace game.ui {
    public struct LabelStyle {
        private Color textColorNormal;
        private float textSizeNormal;
        private StringFormat textAlignmentNormal;
        private FontLoader fontLoaderInstance;
        private Font fontNormal => fontLoaderInstance[textSizeNormal];

        public Color TextColor;
        public float TextSize;
        public StringFormat TextAlignment;
        public Font Font;

        public void Normal() {
            TextColor = textColorNormal;
            TextSize = textSizeNormal;
            TextAlignment = textAlignmentNormal;
            Font = fontNormal;
        }
        
        public LabelStyle Alter(Color textColorNormal = default, float textSizeNormal = default, StringFormat textAlignmentNormal = null, FontLoader fontLoaderInstance = null) {
            var copy = this;
            copy.textColorNormal = textColorNormal == default ? this.textColorNormal : textColorNormal;
            copy.textSizeNormal = textSizeNormal == default ? this.textSizeNormal : textSizeNormal;
            copy.textAlignmentNormal = textAlignmentNormal ?? this.textAlignmentNormal;
            copy.fontLoaderInstance = fontLoaderInstance ?? this.fontLoaderInstance;
            copy.Normal();
            return copy;
        }

        public LabelStyle(Color textColorNormal = default, float textSizeNormal = default, StringFormat textAlignmentNormal = null, FontLoader fontLoaderInstance = null) : this() {
            this.textColorNormal = textColorNormal == default ? Default.textColorNormal : textColorNormal;
            this.textSizeNormal = textSizeNormal == default ? Default.textSizeNormal : textSizeNormal;
            this.textAlignmentNormal = textAlignmentNormal ?? Default.textAlignmentNormal;
            this.fontLoaderInstance = fontLoaderInstance ?? Default.fontLoaderInstance;
            Normal();
        }
        
        public static LabelStyle Default = new LabelStyle {
            textColorNormal = Color.FromArgb(255,0,0,0),
            textSizeNormal = 16f,
            textAlignmentNormal = FontLoader.LeftTopAlignment,
            fontLoaderInstance = FontLoader.SourceCode,
            
            TextColor = Color.FromArgb(255,0,0,0),
            TextSize = 16f,
            TextAlignment = FontLoader.LeftTopAlignment,
            Font = FontLoader.SourceCode[16f]
        };
        
        public static LabelStyle DefaultCheckboxLabel = new LabelStyle {
            textColorNormal = Color.FromArgb(255,255,255,255),
            textSizeNormal = 16f,
            textAlignmentNormal = FontLoader.LeftCenterAlignment,
            fontLoaderInstance = FontLoader.SourceCode,
            
            TextColor = Color.FromArgb(255,255,255,255),
            TextSize = 16f,
            TextAlignment = FontLoader.LeftCenterAlignment,
            Font = FontLoader.SourceCode[16f]
        };
        
        public static LabelStyle DefaultChat = DefaultCheckboxLabel.Alter(textSizeNormal: 14f);
    }
}