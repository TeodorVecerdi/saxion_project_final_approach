using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace GXPEngine {
    public enum CenterMode {
        Min,
        Center,
        Max
    }

    /// <summary>
    ///     Creates an easy-to-use layer on top of .NET's System.Drawing methods.
    ///     The API is inspired by Processing: internal states are maintained for font, fill/stroke color, etc.,
    ///     and everything works with simple methods that have many overloads.
    /// </summary>
    public class EasyDraw : Canvas {
        private static readonly Font defaultFont = new Font("Noto Sans", 15);
        protected bool _fill = true;
        protected bool _stroke = true;
        public CenterMode HorizontalShapeAlign = CenterMode.Center;

        public CenterMode HorizontalTextAlign = CenterMode.Min;
        public CenterMode VerticalShapeAlign = CenterMode.Center;
        public CenterMode VerticalTextAlign = CenterMode.Max;

        public EasyDraw(int width, int height, bool addCollider = true) : base(new Bitmap(width, height), addCollider) {
            Initialize();
        }

        public EasyDraw(Bitmap bitmap, bool addCollider = true) : base(bitmap, addCollider) {
            Initialize();
        }

        public EasyDraw(string filename, bool addCollider = true) : base(filename, addCollider) {
            Initialize();
        }

        public Font font { get; protected set; }
        public Pen pen { get; protected set; }
        public SolidBrush brush { get; protected set; }

        private void Initialize() {
            pen = new Pen(Color.White, 1);
            brush = new SolidBrush(Color.White);
            font = defaultFont;
            if (!game.PixelArt) {
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; //AntiAlias;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
            } else {
                graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            }
        }

        //////////// Setting Font

        public void TextFont(Font newFont) {
            font = newFont;
        }

        public void TextFont(string fontName, float pointSize, FontStyle style = FontStyle.Regular) {
            font = new Font(fontName, pointSize, style);
        }

        public void TextSize(float pointSize) {
            font = new Font(font.OriginalFontName, pointSize, font.Style);
        }

        //////////// Setting Alignment for text, ellipses and rects

        public void TextAlign(CenterMode horizontal, CenterMode vertical) {
            HorizontalTextAlign = horizontal;
            VerticalTextAlign = vertical;
        }

        public void ShapeAlign(CenterMode horizontal, CenterMode vertical) {
            HorizontalShapeAlign = horizontal;
            VerticalShapeAlign = vertical;
        }

        //////////// Setting Stroke

        public void NoStroke() {
            _stroke = false;
        }

        public void Stroke(Color newColor, int alpha = 255) {
            pen.Color = Color.FromArgb(alpha, newColor);
            _stroke = true;
        }

        public void Stroke(int grayScale, int alpha = 255) {
            pen.Color = Color.FromArgb(alpha, grayScale, grayScale, grayScale);
            _stroke = true;
        }

        public void Stroke(int red, int green, int blue, int alpha = 255) {
            pen.Color = Color.FromArgb(alpha, red, green, blue);
            _stroke = true;
        }

        public void StrokeWeight(float width) {
            pen.Width = width;
            _stroke = true;
        }

        //////////// Setting Fill

        public void NoFill() {
            _fill = false;
        }

        public void Fill(Color newColor, int alpha = 255) {
            brush.Color = Color.FromArgb(alpha, newColor);
            _fill = true;
        }

        public void Fill(int grayScale, int alpha = 255) {
            brush.Color = Color.FromArgb(alpha, grayScale, grayScale, grayScale);
            _fill = true;
        }

        public void Fill(int red, int green, int blue, int alpha = 255) {
            brush.Color = Color.FromArgb(alpha, red, green, blue);
            _fill = true;
        }

        //////////// Clear

        public void Clear(Color newColor) {
            graphics.Clear(newColor);
        }

        public void Clear(int grayScale) {
            graphics.Clear(Color.FromArgb(255, grayScale, grayScale, grayScale));
        }

        public void Clear(int red, int green, int blue) {
            graphics.Clear(Color.FromArgb(255, red, green, blue));
        }

        //////////// Draw & measure Text

        public void Text(string text, float x, float y) {
            float twidth, theight;
            TextDimensions(text, out twidth, out theight);
            if (HorizontalTextAlign == CenterMode.Max)
                x -= twidth;
            else if (HorizontalTextAlign == CenterMode.Center) x -= twidth / 2;
            if (VerticalTextAlign == CenterMode.Max)
                y -= theight;
            else if (VerticalTextAlign == CenterMode.Center) y -= theight / 2;
            graphics.DrawString(text, font, brush, x, y); //left+BoundaryPadding/2,top+BoundaryPadding/2);
        }

        public float TextWidth(string text) {
            var size = graphics.MeasureString(text, font);
            return size.Width;
        }

        public float TextHeight(string text) {
            var size = graphics.MeasureString(text, font);
            return size.Height;
        }

        public void TextDimensions(string text, out float width, out float height) {
            var size = graphics.MeasureString(text, font);
            width = size.Width;
            height = size.Height;
        }

        //////////// Draw Shapes

        public void Rect(float x, float y, float width, float height) {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill) graphics.FillRectangle(brush, x, y, width, height);
            if (_stroke) graphics.DrawRectangle(pen, x, y, width, height);
        }

        public void Ellipse(float x, float y, float width, float height) {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill) graphics.FillEllipse(brush, x, y, width, height);
            if (_stroke) graphics.DrawEllipse(pen, x, y, width, height);
        }

        public void Arc(float x, float y, float width, float height, float startAngleDegrees, float sweepAngleDegrees) {
            ShapeAlign(ref x, ref y, width, height);
            if (_fill) graphics.FillPie(brush, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
            if (_stroke) graphics.DrawArc(pen, x, y, width, height, startAngleDegrees, sweepAngleDegrees);
        }

        public void Line(float x1, float y1, float x2, float y2) {
            if (_stroke) graphics.DrawLine(pen, x1, y1, x2, y2);
        }

        public void Triangle(float x1, float y1, float x2, float y2, float x3, float y3) {
            Polygon(x1, y1, x2, y2, x3, y3);
        }

        public void Quad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) {
            Polygon(x1, y1, x2, y2, x3, y3, x4, y4);
        }

        public void Polygon(params float[] pt) {
            var pts = new PointF[pt.Length / 2];
            for (var i = 0; i < pts.Length; i++) pts[i] = new PointF(pt[2 * i], pt[2 * i + 1]);
            Polygon(pts);
        }

        public void Polygon(PointF[] pts) {
            if (_fill) graphics.FillPolygon(brush, pts);
            if (_stroke) graphics.DrawPolygon(pen, pts);
        }

        protected void ShapeAlign(ref float x, ref float y, float width, float height) {
            if (HorizontalShapeAlign == CenterMode.Max)
                x -= width;
            else if (HorizontalShapeAlign == CenterMode.Center) x -= width / 2;
            if (VerticalShapeAlign == CenterMode.Max)
                y -= height;
            else if (VerticalShapeAlign == CenterMode.Center) y -= height / 2;
        }
    }
}