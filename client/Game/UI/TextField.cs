using System;
using System.Drawing;
using System.Drawing.Text;
using game.utils;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class TextField : EasyDraw {
        public Action OnGainFocus;
        public Action OnLoseFocus;
        public Action OnMouseClick;
        public Action OnMouseEnter;
        public Action OnMouseLeave;
        public Action OnMousePress;
        public Action OnMouseRelease;
        public Action<int> OnKeyRepeat;
        public Action<int> OnKeyTyped;
        public Action<string, string> OnValueChanged;

        private const float caretTimerInitial = 0.5f;
        private const float repeatFrequency = 0.03f;
        private const float repeatStart = 0.75f;

        private readonly Rectangle bounds;
        private readonly string placeholderText;
        private bool focused;
        private bool pressed;
        private bool repeating;
        private bool showCaret;
        private bool wasMouseOnTopPreviousFrame;
        private float caretTimer;
        private float repeatTimer;
        private int caretIndex = -1;
        private int repeatKey;

        private string currentText = "";
        private string oldText = "";
        private TextFieldStyle textFieldStyle;

        public bool ShouldRepaint { private get; set; }
        public string Text {
            get => currentText;
            set {
                OnValueChanged?.Invoke(currentText, value);
                currentText = value;
                caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                Draw();
            }
        }

        private bool IsMouseOnTop {
            get {
                var globalBounds = TransformPoint(0, 0);
                return Input.mouseX >= globalBounds.x && Input.mouseX <= globalBounds.x + bounds.width && Input.mouseY >= globalBounds.y && Input.mouseY <= globalBounds.y + bounds.height;
            }
        }

        public TextField(float x, float y, float width, float height, string placeholderText, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, placeholderText, TextFieldStyle.Default, onValueChanged, onKeyTyped, onKeyRepeat, onGainFocus, onLoseFocus, onMouseClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public TextField(float x, float y, float width, float height, string placeholderText, TextFieldStyle textFieldStyle, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.placeholderText = placeholderText;
            this.textFieldStyle = textFieldStyle;

            OnValueChanged += onValueChanged;
            OnKeyTyped += onKeyTyped;
            OnKeyRepeat += onKeyRepeat;
            OnGainFocus += onGainFocus;
            OnLoseFocus += onLoseFocus;
            OnMouseClick += onMouseClick;
            OnMouseEnter += onMouseEnter;
            OnMouseLeave += onMouseLeave;
            OnMousePress += onMousePress;
            OnMouseRelease += onMouseRelease;

            SetXY(x, y);
            Draw();
        }

        private void Update() {
            var onTop = IsMouseOnTop;

            // Check for button states and apply style
            if (Input.GetMouseButtonUp(GXPEngine.Button.LEFT) && pressed) {
                OnMouseRelease?.Invoke();
                pressed = false;
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && onTop) {
                OnMouseClick?.Invoke();
                pressed = true;
                if (!focused) {
                    OnGainFocus?.Invoke();
                    focused = true;
                    caretTimer = caretTimerInitial;
                    showCaret = true;
                }

                textFieldStyle.Focus();
                Draw();
            } else if (Input.GetMouseButton(GXPEngine.Button.LEFT) && pressed) {
                OnMousePress?.Invoke();
                Draw();
            } else if (onTop && !wasMouseOnTopPreviousFrame && !pressed) {
                OnMouseEnter?.Invoke();
                MouseCursor.Instance.Text();
                Draw();
            } else if (!onTop && wasMouseOnTopPreviousFrame && !pressed) {
                OnMouseLeave?.Invoke();
                MouseCursor.Instance.Normal();
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && !onTop && focused) {
                focused = false;
                OnLoseFocus?.Invoke();
                textFieldStyle.Normal();
                Draw();
            }

            if (focused) {
                if (Input.AnyKeyDown() || repeating && repeatTimer <= 0f) {
                    var key = Input.LastKeyDown;
                    if (repeating) OnKeyRepeat?.Invoke(key);
                    else OnKeyTyped?.Invoke(key);
                    oldText = currentText;
                    if (key == Key.BACKSPACE && !string.IsNullOrEmpty(currentText) && caretIndex != -1) {
                        currentText = currentText.Remove(caretIndex, 1);
                        caretIndex--;
                        caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                    } else if (key == Key.DELETE && !string.IsNullOrEmpty(currentText) && caretIndex != currentText.Length - 1) {
                        currentText = currentText.Remove(caretIndex + 1, 1);
                        caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                    } else if (key == Key.LEFT) {
                        caretIndex--;
                        caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                    } else if (key == Key.RIGHT) {
                        caretIndex++;
                        caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                    } else {
                        // Treat as normal character
                        var keyValue = Input.KeyToString(key);
                        if (!string.IsNullOrEmpty(keyValue)) {
                            currentText = currentText.Insert(caretIndex + 1, keyValue);
                            caretIndex++;
                            caretIndex = caretIndex.Constrain(-1, currentText.Length - 1);
                        }
                    }

                    OnValueChanged?.Invoke(oldText, currentText);
                    Draw();
                } else if (Input.AnyKey() && !repeating) {
                    repeating = true;
                    repeatTimer = repeatStart;
                    repeatKey = Input.LastKeyDown;
                } else if (!Input.AnyKey() && repeating)
                    repeating = false;

                if (repeating) {
                    if (repeatTimer <= 0f) repeatTimer = repeatFrequency;
                    repeatTimer -= Time.deltaTime;
                }

                caretTimer -= Time.deltaTime;
                if (caretTimer <= 0f) {
                    showCaret = !showCaret;
                    caretTimer = caretTimerInitial;
                    Draw();
                }
            }

            if (repeating && repeatKey != Input.LastKey)
                repeating = false;

            wasMouseOnTopPreviousFrame = onTop;

            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);

            Stroke(textFieldStyle.BorderColor);
            StrokeWeight(textFieldStyle.BorderSize);
            Fill(textFieldStyle.BackgroundColor);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Rect(0, 0, bounds.width, bounds.height);

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            var textX = textFieldStyle.LeftMargin;
            var textY = 0f;
            if (textFieldStyle.TextAlignment.Alignment == StringAlignment.Far)
                textX += bounds.width;
            else if (textFieldStyle.TextAlignment.Alignment == StringAlignment.Center) textX += bounds.width / 2;
            if (textFieldStyle.TextAlignment.LineAlignment == StringAlignment.Far)
                textY += bounds.height;
            else if (textFieldStyle.TextAlignment.LineAlignment == StringAlignment.Center) textY += bounds.height / 2;
            if (string.IsNullOrEmpty(currentText)) {
                Fill(textFieldStyle.PlaceholderTextColor);
                graphics.DrawString(placeholderText, textFieldStyle.PlaceholderFont, brush, textX, textY, textFieldStyle.TextAlignment);
            } else {
                Fill(textFieldStyle.TextColor);
                graphics.DrawString(currentText, textFieldStyle.Font, brush, textX, textY, textFieldStyle.TextAlignment);
            }

            if (focused && showCaret) {
                ShapeAlign(CenterMode.Min, CenterMode.Center);
                NoStroke();
                Fill(textFieldStyle.CaretColor);
                var textLength = currentText.Length.Constrain(1, int.MaxValue);
                var stringWidth = graphics.MeasureString(currentText, textFieldStyle.Font, new SizeF(0f, 0f), textFieldStyle.TextAlignment).Width;
                var charWidth = stringWidth / (double) textLength;
                var caretPosition = (caretIndex + 1) * charWidth - Mathf.Sqrt(textFieldStyle.TextSize);
                var textHeight = TextHeight("A");
                Rect((float) caretPosition + textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.CaretWidth, textHeight);
            }
        }
    }
}