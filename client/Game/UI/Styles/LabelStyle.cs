using System.Drawing;
using GXPEngine;

namespace game.ui {
    public struct LabelStyle {
        private Color textColorNormal;
        private float textSizeNormal;
        private CenterMode textAlignmentHorizontalNormal;
        private CenterMode textAlignmentVerticalNormal;

        public Color TextColor;
        public float TextSize;
        public CenterMode TextAlignmentHorizontal;
        public CenterMode TextAlignmentVertical;

        public void Normal() {
            TextColor = textColorNormal;
            TextSize = textSizeNormal;
            TextAlignmentHorizontal = textAlignmentHorizontalNormal;
            TextAlignmentVertical = textAlignmentVerticalNormal;
        }

        public LabelStyle(Color textColorNormal = default, float textSizeNormal = default, CenterMode textAlignmentHorizontalNormal = CenterMode.Min, CenterMode textAlignmentVerticalNormal = CenterMode.Min) : this() {
            this.textColorNormal = textColorNormal == default ? Default.textColorNormal : textColorNormal;
            this.textSizeNormal = textSizeNormal == default ? Default.textSizeNormal : textSizeNormal;
            this.textAlignmentHorizontalNormal = textAlignmentHorizontalNormal == CenterMode.Min ? Default.textAlignmentHorizontalNormal : textAlignmentHorizontalNormal;
            this.textAlignmentVerticalNormal = textAlignmentVerticalNormal == CenterMode.Min ? Default.textAlignmentVerticalNormal : textAlignmentVerticalNormal;
            Normal();
        }
        
        public static LabelStyle Default = new LabelStyle {
            textColorNormal = Color.FromArgb(255,0,0,0),
            textSizeNormal = 16f,
            textAlignmentHorizontalNormal = CenterMode.Min,
            textAlignmentVerticalNormal = CenterMode.Min,
            
            TextColor = Color.FromArgb(255,0,0,0),
            TextSize = 16f,
            TextAlignmentHorizontal = CenterMode.Min,
            TextAlignmentVertical = CenterMode.Min
        };
    }
}