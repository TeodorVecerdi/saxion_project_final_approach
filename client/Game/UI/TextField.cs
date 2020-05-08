using System;
using System.Drawing;
using game.utils;
using GXPEngine;
using Debug = GXPEngine.Debug;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class TextField : EasyDraw {
        private readonly Action<string, string> onValueChanged;
        private readonly Action<int> onKeyTyped;
        private readonly Action<int> onKeyRepeat;
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

        private string currentText = "";
        private string oldText = "";

        private const float caretTimerInitial = 0.5f;
        private const float repeatFrequency = 0.03f;
        private const float repeatStart = 0.75f;
        private float caretTimer;
        private float repeatTimer;
        private int caretIndex = -1;

        private bool focused;
        private bool pressed;
        private bool showCaret;
        private bool wasMouseOnTopPreviousFrame;
        private bool repeating;

        private bool IsMouseOnTop => Input.mouseX >= bounds.x && Input.mouseX <= bounds.x + bounds.width && Input.mouseY >= bounds.y && Input.mouseY <= bounds.y + bounds.height;
        public string Text => currentText;

        public TextField(float x, float y, float width, float height, string placeholderText, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, placeholderText, TextFieldStyle.Default, onValueChanged, onKeyTyped, onKeyRepeat, onGainFocus, onLoseFocus, onMouseClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public TextField(float x, float y, float width, float height, string placeholderText, TextFieldStyle textFieldStyle, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.placeholderText = placeholderText;
            this.textFieldStyle = textFieldStyle;

            this.onValueChanged += onValueChanged;
            this.onKeyTyped += onKeyTyped;
            this.onKeyRepeat += onKeyRepeat;
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

            if (focused) {
                if (Input.AnyKeyDown() || (repeating && repeatTimer <= 0f)) {
                    var key = Input.LastKeyDown;
                    if(repeating) onKeyRepeat?.Invoke(key);
                    else onKeyTyped?.Invoke(key);
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

                    onValueChanged?.Invoke(oldText, currentText);
                    Draw();
                } else if (Input.AnyKey() && !repeating) {
                    repeating = true;
                    repeatTimer = repeatStart;
                    Debug.Log("Started repeating");
                } else if (!Input.AnyKey() && repeating) {
                    repeating = false;
                    Debug.Log("Stopped repeating");
                }

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

            wasMouseOnTopPreviousFrame = onTop;
        }

        private void Draw() {
            Clear(Color.Transparent);

            Stroke(textFieldStyle.BorderColor);
            StrokeWeight(textFieldStyle.BorderSize);
            Fill(textFieldStyle.BackgroundColor);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Rect(0, 0, bounds.width, bounds.height);

            if (string.IsNullOrEmpty(currentText)) {
                Fill(textFieldStyle.PlaceholderTextColor);
                graphics.DrawString(placeholderText, textFieldStyle.Font, brush, textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.TextAlignment);
            } else {
                Fill(textFieldStyle.TextColor);
                graphics.DrawString(currentText, textFieldStyle.Font, brush, textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.TextAlignment);
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