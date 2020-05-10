using System;
using System.Drawing;
using System.Drawing.Text;
using game.utils;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;
using Utils = game.utils.Utils;

namespace game.ui {
    public class Checkbox : EasyDraw {
        private readonly Action<bool, bool> onValueChanged;
        private readonly Action onClick;
        private readonly Action onMouseEnter;
        private readonly Action onMouseLeave;
        private readonly Action onMousePress;
        private readonly Action onMouseRelease;

        private readonly Rectangle bounds;
        private readonly Sprite tickSprite;
        private LabelStyle labelStyle;
        private CheckboxStyle checkboxStyle;
        private bool isChecked;
        private bool pressed;
        private bool wasMouseOnTopPreviousFrame;

        public string LabelText;

        public bool IsChecked => isChecked;
        public bool ShouldRepaint { private get; set; }
        private bool IsMouseOnTop {
            get {
                var globalBounds = TransformPoint(0, 0);
                return Input.mouseX >= globalBounds.x && Input.mouseX <= globalBounds.x + bounds.width && Input.mouseY >= globalBounds.y && Input.mouseY <= globalBounds.y + bounds.height;
            }
        }

        public Checkbox(float x, float y, float width, float height, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, "", LabelStyle.DefaultCheckboxLabel, CheckboxStyle.Default, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, LabelStyle labelStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, "", labelStyle, CheckboxStyle.Default, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, CheckboxStyle checkboxStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, "", LabelStyle.DefaultCheckboxLabel, checkboxStyle, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, string labelText, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, labelText, LabelStyle.DefaultCheckboxLabel, CheckboxStyle.Default, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, string labelText, LabelStyle labelStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, labelText, labelStyle, CheckboxStyle.Default, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, string labelText, CheckboxStyle checkboxStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, labelText, LabelStyle.DefaultCheckboxLabel, checkboxStyle, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, LabelStyle labelStyle, CheckboxStyle checkboxStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, "", labelStyle, checkboxStyle, onValueChanged, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public Checkbox(float x, float y, float width, float height, string labelText, LabelStyle labelStyle, CheckboxStyle checkboxStyle, Action<bool, bool> onValueChanged = null, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            LabelText = labelText;
            this.labelStyle = labelStyle;
            this.checkboxStyle = checkboxStyle;
            this.onValueChanged += onValueChanged;
            this.onClick += onClick;
            this.onMouseEnter += onMouseEnter;
            this.onMouseLeave += onMouseLeave;
            this.onMousePress += onMousePress;
            this.onMouseRelease += onMouseRelease;
            isChecked = false;

            var closestSpriteSize = Utils.ClosestSorted(height, 32, 64, 128, 256, 512);
            var spriteScale = height / closestSpriteSize;
            tickSprite = new Sprite($"data/sprites/ui/checkbox_tick_{closestSpriteSize}.png");
            tickSprite.SetScaleXY(spriteScale, spriteScale);

            SetXY(x, y);
            Draw();
        }

        private void Update() {
            var onTop = IsMouseOnTop;

            // Check for button states and apply style
            if (Input.GetMouseButtonUp(GXPEngine.Button.LEFT) && pressed) {
                onMouseRelease?.Invoke();
                onValueChanged?.Invoke(isChecked, !isChecked);
                isChecked = !isChecked;
                if (onTop) checkboxStyle.Hover();
                else checkboxStyle.Normal();
                pressed = false;
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && onTop) {
                onClick?.Invoke();
                checkboxStyle.Press();
                pressed = true;
                Draw();
            } else if (Input.GetMouseButton(GXPEngine.Button.LEFT) && pressed) {
                onMousePress?.Invoke();
                checkboxStyle.Press();
                Draw();
            } else if (onTop && !wasMouseOnTopPreviousFrame && !pressed) {
                onMouseEnter?.Invoke();
                MouseCursor.Instance.Button();
                checkboxStyle.Hover();
                Draw();
            } else if (!onTop && wasMouseOnTopPreviousFrame && !pressed) {
                onMouseLeave?.Invoke();
                MouseCursor.Instance.Normal();
                checkboxStyle.Normal();
                Draw();
            }

            wasMouseOnTopPreviousFrame = onTop;

            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);

            Stroke(checkboxStyle.BorderColor);
            StrokeWeight(checkboxStyle.BorderSize);
            Fill(checkboxStyle.BackgroundColor);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Rect(0, 0, bounds.height, bounds.height);
            var newBitmap = tickSprite.texture.bitmap.Tint(checkboxStyle.TickColor, 1f);
            if (isChecked) {
                Fill(checkboxStyle.TickColor);
                graphics.DrawString("\u2713", FontLoader.SourceCodeBold[bounds.height * 0.7f], brush, bounds.height / 2f + checkboxStyle.BorderSize, bounds.height / 2f + checkboxStyle.BorderSize, FontLoader.CenterCenterAlignment);
            }

            NoStroke();
            Fill(labelStyle.TextColor);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            var textX = checkboxStyle.LabelOffset + bounds.height;
            var textY = 0f;
            if (labelStyle.TextAlignment.Alignment == StringAlignment.Far)
                textX += bounds.width;
            else if (labelStyle.TextAlignment.Alignment == StringAlignment.Center) textX += bounds.width / 2f;
            if (labelStyle.TextAlignment.LineAlignment == StringAlignment.Far)
                textY += bounds.height;
            else if (labelStyle.TextAlignment.LineAlignment == StringAlignment.Center) textY += bounds.height / 2;
            graphics.DrawString(LabelText, labelStyle.Font, brush, textX, textY, labelStyle.TextAlignment);
        }
    }
}