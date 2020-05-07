using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class SpriteButton : EasyDraw {
        private readonly Action onMouseClick;
        private readonly Action onMouseEnter;
        private readonly Action onMouseLeave;
        private readonly Action onMousePress;
        private readonly Action onMouseRelease;

        private readonly Rectangle bounds;
        private readonly string buttonText;
        private new readonly Texture2D texture;
        private ButtonStyle buttonStyle;

        private bool wasMouseOnTopPreviousFrame;
        private bool pressed;
        private bool IsMouseOnTop => Input.mouseX >= bounds.x && Input.mouseX <= bounds.x + bounds.width && Input.mouseY >= bounds.y && Input.mouseY <= bounds.y + bounds.height;

        public SpriteButton(float x, float y, float width, float height, string buttonText, Texture2D texture, Action onMouseClick, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, buttonText, texture, ButtonStyle.Default, onMouseClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public SpriteButton(float x, float y, float width, float height, string buttonText, Texture2D texture, ButtonStyle buttonStyle, Action onMouseClick, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.buttonText = buttonText;
            this.texture = texture;
            this.buttonStyle = buttonStyle;

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

            // Check for button states and setup style
            if (Input.GetMouseButtonUp(GXPEngine.Button.LEFT) && pressed) {
                onMouseRelease?.Invoke();
                if (onTop) buttonStyle.Hover();
                else buttonStyle.Normal();
                pressed = false;
                Draw();
            } else if (Input.GetMouseButtonDown(GXPEngine.Button.LEFT) && onTop) {
                onMouseClick?.Invoke();
                buttonStyle.Press();
                pressed = true;
                Draw();
            } else if (Input.GetMouseButton(GXPEngine.Button.LEFT) && pressed) {
                onMousePress?.Invoke();
                buttonStyle.Press();
                Draw();
            } else if (onTop && !wasMouseOnTopPreviousFrame && !pressed) {
                onMouseEnter?.Invoke();
                buttonStyle.Hover();
                Draw();
            } else if (!onTop && wasMouseOnTopPreviousFrame && !pressed) {
                onMouseLeave?.Invoke();
                buttonStyle.Normal();
                Draw();
            }

            wasMouseOnTopPreviousFrame = onTop;
        }

        private void Draw() {
            Clear(Color.Transparent);
            
            DrawTexture(texture, 0, 0);
            
            Fill(buttonStyle.TextColor);
            TextSize(buttonStyle.TextSize);
            TextAlign(CenterMode.Center, CenterMode.Center);
            Text(buttonText, bounds.width / 2f, bounds.height / 2f);
        }
    }
}