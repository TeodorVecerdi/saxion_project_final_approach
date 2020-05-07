using System;
using System.Drawing;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class TextField : EasyDraw {
        private readonly Action<string, string> onValueChanged;
        private readonly Action<int> onKeyTyped;
        private readonly Action onGainFocus;
        private readonly Action onLoseFocus;
        private readonly Action onMouseClick;
        private readonly Action onMouseEnter;
        private readonly Action onMouseLeave;
        private readonly Action onMousePress;
        private readonly Action onMouseRelease;

        private readonly Rectangle bounds;
        private readonly string placeholderText;
        private TextFieldStyle textFieldStyle;

        private string currentText;
        private string oldText;

        private const float caretTimerInitial = 0.5f;
        private float caretTimer;

        private bool focused;
        private bool pressed;
        private bool showCaret;
        private bool wasMouseOnTopPreviousFrame;

        private bool IsMouseOnTop => Input.mouseX >= bounds.x && Input.mouseX <= bounds.x + bounds.width && Input.mouseY >= bounds.y && Input.mouseY <= bounds.y + bounds.height;
        public string Text => currentText;

        public TextField(float x, float y, float width, float height, string placeholderText, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, placeholderText, TextFieldStyle.Default, onValueChanged, onKeyTyped, onGainFocus, onLoseFocus, onMouseClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public TextField(float x, float y, float width, float height, string placeholderText, TextFieldStyle textFieldStyle, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.placeholderText = placeholderText;
            this.textFieldStyle = textFieldStyle;

            this.onValueChanged += onValueChanged;
            this.onKeyTyped += onKeyTyped;
            this.onGainFocus += onGainFocus;
            this.onLoseFocus += onLoseFocus;
            this.onMouseClick += onMouseClick;
            this.onMouseEnter += onMouseEnter;
            this.onMouseLeave += onMouseLeave;
            this.onMousePress += onMousePress;
            this.onMouseRelease += onMouseRelease;

            SetXY(x, y);
            Draw();
        }

        private void Update() {
            var onTop = IsMouseOnTop;

            // Check for button states and apply style
            if (Input.GetMouseButtonUp(GXPEngine.Button.LEFT) && pressed) {
                onMouseRelease?.Invoke();
                pressed = false;
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && onTop) {
                onMouseClick?.Invoke();
                pressed = true;
                if (!focused) {
                    onGainFocus?.Invoke();
                    focused = true;
                    caretTimer = caretTimerInitial;
                    showCaret = true;
                }

                textFieldStyle.Focus();
                Draw();
            } else if (Input.GetMouseButton(GXPEngine.Button.LEFT) && pressed) {
                onMousePress?.Invoke();
                Draw();
            } else if (onTop && !wasMouseOnTopPreviousFrame && !pressed) {
                onMouseEnter?.Invoke();
                Draw();
            } else if (!onTop && wasMouseOnTopPreviousFrame && !pressed) {
                onMouseLeave?.Invoke();
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && !onTop && focused) {
                focused = false;
                onLoseFocus?.Invoke();
                textFieldStyle.Normal();
                Draw();
            }

            if (Input.AnyKeyDown() && focused) {
                var key = Input.LastKeyDown;
                onKeyTyped?.Invoke(key);
                oldText = currentText;
                if (key == Key.BACKSPACE) {
                    if (!string.IsNullOrEmpty(currentText))
                        currentText = currentText.Substring(0, currentText.Length - 1);
                } else {
                    currentText += Input.KeyToString(key);
                }

                onValueChanged?.Invoke(oldText, currentText);
                Draw();
            }

            if (focused) {
                caretTimer -= Time.deltaTime;
                if (caretTimer <= 0f) {
                    showCaret = !showCaret;
                    caretTimer = caretTimerInitial;
                    Draw();
                }
            }

            wasMouseOnTopPreviousFrame = onTop;
        }

        private void Draw() {
            Clear(Color.Transparent);

            Stroke(textFieldStyle.Border);
            StrokeWeight(textFieldStyle.BorderSize);
            Fill(textFieldStyle.Background);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Rect(0, 0, bounds.width, bounds.height);

            TextSize(textFieldStyle.TextSize);
            TextAlign(CenterMode.Min, CenterMode.Center);
            if (string.IsNullOrEmpty(currentText)) {
                Fill(textFieldStyle.PlaceholderText);
                Text(placeholderText, textFieldStyle.LeftMargin, bounds.height / 2f);
            } else {
                Fill(textFieldStyle.Text);
                Text(currentText, textFieldStyle.LeftMargin, bounds.height / 2f);
            }

            if (focused && showCaret) {
                ShapeAlign(CenterMode.Min, CenterMode.Center);
                NoStroke();
                Fill(textFieldStyle.Caret);
                var textWidth = TextWidth(currentText);
                var textHeight = TextHeight("A");
                Rect(textWidth + textFieldStyle.LeftMargin, bounds.height / 2f, 2, textHeight);
            }
        }
    }
}