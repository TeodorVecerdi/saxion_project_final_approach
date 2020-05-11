using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace game.utils {
    public class FontLoader {
        public static readonly StringFormat CenterBottomAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat CenterCenterAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat CenterTopAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat LeftBottomAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat LeftCenterAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};

        public static readonly StringFormat LeftTopAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};

        public static readonly StringFormat LeftVerticalAlignment = new StringFormat {Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            CenterVerticalAlignment = new StringFormat {Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical},
            RightVerticalAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionVertical};
        public static readonly StringFormat RightBottomAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat RightCenterAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        public static readonly StringFormat RightTopAlignment = new StringFormat {Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near, FormatFlags = StringFormatFlags.MeasureTrailingSpaces, Trimming = StringTrimming.Word};
        private static FontLoader instanceFiraCode;
        private static FontLoader instanceFiraCodeBold;
        private static FontLoader instanceSourceCode;
        private static FontLoader instanceSourceCodeBold;

        private readonly Dictionary<float, Font> fonts;
        private readonly FontFamily fontFamily;
        private readonly FontStyle fontStyle = FontStyle.Regular;

        public static FontLoader Default => SourceCode;
        public static FontLoader SourceCode => instanceSourceCode ?? (instanceSourceCode = new FontLoader("Source Code Variable", "data/fonts/SourceCodeVariable-Roman.ttf"));
        public static FontLoader SourceCodeBold => instanceSourceCodeBold ?? (instanceSourceCodeBold = new FontLoader("Source Code Variable", "data/fonts/SourceCodeVariable-Roman.ttf", true));
        public static FontLoader FiraCode => instanceFiraCode ?? (instanceFiraCode = new FontLoader("Fira Code Retina", "data/fonts/FiraCode-Retina.ttf"));
        public static FontLoader FiraCodeBold => instanceFiraCodeBold ?? (instanceFiraCodeBold = new FontLoader("Fira Code Retina", "data/fonts/FiraCode-Retina.ttf", true));

        public Font this[float fontSize] => fonts.ContainsKey(fontSize) ? fonts[fontSize] : fonts[fontSize] = new Font(fontFamily, fontSize, fontStyle, GraphicsUnit.Point);

        private FontLoader(string fontFamily, string fontPath, bool bold = false, bool italic = false, bool strikethrough = false, bool underline = false) {
            var collection = new PrivateFontCollection();
            collection.AddFontFile(fontPath);
            this.fontFamily = new FontFamily(fontFamily, collection);
            if (bold) fontStyle |= FontStyle.Bold;
            if (italic) fontStyle |= FontStyle.Italic;
            if (strikethrough) fontStyle |= FontStyle.Strikeout;
            if (underline) fontStyle |= FontStyle.Underline;
            fonts = new Dictionary<float, Font>();
        }

        private FontLoader() : this("Source Code Variable", "data/fonts/SourceCodeVariable-Roman.ttf") { }
    }
}