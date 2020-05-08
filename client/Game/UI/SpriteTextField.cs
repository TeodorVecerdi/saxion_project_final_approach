using System;
using System.Drawing;
using System.Drawing.Text;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class SpriteTextField : EasyDraw {
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
        private new readonly Texture2D texture;
        private TextFieldStyle textFieldStyle;

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

        public bool ShouldRepaint { private get; set; }
        public new string Text = "";
        
        private bool IsMouseOnTop {
            get {
                var globalBounds = TransformPoint(0,0);
                return Input.mouseX >= globalBounds.x && Input.mouseX <= globalBounds.x + bounds.width && Input.mouseY >= globalBounds.y && Input.mouseY <= globalBounds.y + bounds.height;
            }
        }

        public SpriteTextField(float x, float y, float width, float height, string placeholderText, Texture2D texture, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, placeholderText, texture, TextFieldStyle.Default, onValueChanged, onKeyTyped, onKeyRepeat, onGainFocus, onLoseFocus, onMouseClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public SpriteTextField(float x, float y, float width, float height, string placeholderText, Texture2D texture, TextFieldStyle textFieldStyle, Action<string, string> onValueChanged = null, Action<int> onKeyTyped = null, Action<int> onKeyRepeat = null, Action onGainFocus = null, Action onLoseFocus = null, Action onMouseClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.placeholderText = placeholderText;
            this.texture = texture;
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
                    oldText = Text;
                    if (key == Key.BACKSPACE && !string.IsNullOrEmpty(Text) && caretIndex != -1) {
                        Text = Text.Remove(caretIndex, 1);
                        caretIndex--;
                        caretIndex = caretIndex.Constrain(-1, Text.Length - 1);
                    } else if (key == Key.DELETE && !string.IsNullOrEmpty(Text) && caretIndex != Text.Length - 1) {
                        Text = Text.Remove(caretIndex + 1, 1);
                        caretIndex = caretIndex.Constrain(-1, Text.Length - 1);
                    } else if (key == Key.LEFT) {
                        caretIndex--;
                        caretIndex = caretIndex.Constrain(-1, Text.Length - 1);
                    } else if (key == Key.RIGHT) {
                        caretIndex++;
                        caretIndex = caretIndex.Constrain(-1, Text.Length - 1);
                    } else {
                        // Treat as normal character
                        var keyValue = Input.KeyToString(key);
                        if (!string.IsNullOrEmpty(keyValue)) {
                            Text = Text.Insert(caretIndex + 1, keyValue);
                            caretIndex++;
                            caretIndex = caretIndex.Constrain(-1, Text.Length - 1);
                        }
                    }

                    onValueChanged?.Invoke(oldText, Text);
                    Draw();
                } else if (Input.AnyKey() && !repeating) {
                    repeating = true;
                    repeatTimer = repeatStart;
                } else if (!Input.AnyKey() && repeating) {
                    repeating = false;
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
            
            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);

            DrawTexture(texture, 0, 0);

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            
            if (string.IsNullOrEmpty(Text)) {
                Fill(textFieldStyle.PlaceholderTextColor);
                graphics.DrawString(placeholderText, textFieldStyle.PlaceholderFont, brush, textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.TextAlignment);
            } else {
                Fill(textFieldStyle.TextColor);
                graphics.DrawString(Text, textFieldStyle.Font, brush, textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.TextAlignment);
            }

            if (focused && showCaret) {
                ShapeAlign(CenterMode.Min, CenterMode.Center);
                NoStroke();
                Fill(textFieldStyle.CaretColor);
                var textLength = Text.Length.Constrain(1, int.MaxValue);
                var stringWidth = graphics.MeasureString(Text, textFieldStyle.Font, new SizeF(0f, 0f), textFieldStyle.TextAlignment).Width;
                var charWidth = stringWidth / (double) textLength;
                var caretPosition = (caretIndex + 1) * charWidth - Mathf.Sqrt(textFieldStyle.TextSize);
                var textHeight = TextHeight("A");
                Rect((float)caretPosition + textFieldStyle.LeftMargin, bounds.height / 2f, textFieldStyle.CaretWidth, textHeight);
            }
        }
    }
}