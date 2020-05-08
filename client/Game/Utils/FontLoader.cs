using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace game.utils {
    public class FontLoader {
        private static FontLoader instance;
        private static FontLoader instanceSourceCodeVariable;
        private static FontLoader instanceFiraCode;
        
        public static FontLoader Instance => SourceCode;
        public static FontLoader SourceCode => instanceSourceCodeVariable ?? (instanceSourceCodeVariable = new FontLoader("Source Code Variable", "data/fonts/SourceCodeVariable-Roman.ttf"));
        public static FontLoader FiraCode => instanceFiraCode ?? (instanceFiraCode = new FontLoader("Fira Code Retina", "data/fonts/FiraCode-Retina.ttf"));

        public static readonly StringFormat LeftTopAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat CenterTopAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat RightTopAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat LeftCenterAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat CenterCenterAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat RightCenterAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat LeftBottomAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat CenterBottomAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        public static readonly StringFormat RightBottomAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces};
        
        public static readonly StringFormat LeftVerticalAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            CenterVerticalAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            RightVerticalAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};

        private readonly Dictionary<float, Font> fonts;
        private readonly FontFamily fontFamily;

        private FontLoader(string fontFamily, string fontPath) {
            var collection = new PrivateFontCollection();
            collection.AddFontFile(fontPath);
            this.fontFamily = new FontFamily(fontFamily, collection);
            fonts = new Dictionary<float, Font>();
        }
        
        private FontLoader() : this("Source Code Variable", "data/fonts/SourceCodeVariable-Roman.ttf") {}
        
        public Font this[float fontSize] => fonts.ContainsKey(fontSize) ? fonts[fontSize] : fonts[fontSize] = new Font(fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Point);
    }
}