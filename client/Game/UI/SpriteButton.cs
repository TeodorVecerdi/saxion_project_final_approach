using System;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class SpriteButton : EasyDraw {
        private readonly Action onClick;
        private readonly Action onMouseEnter;
        private readonly Action onMouseLeave;
        private readonly Action onMousePress;
        private readonly Action onMouseRelease;

        private readonly Rectangle bounds;
        private ButtonStyle buttonStyle;

        private bool wasMouseOnTopPreviousFrame;
        private bool pressed;

        public bool ShouldRepaint { private get; set; }
        public string ButtonText;
        public Sprite Sprite;
        
        private bool IsMouseOnTop {
            get {
                var globalBounds = TransformPoint(0,0);
                return Input.mouseX >= globalBounds.x && Input.mouseX <= globalBounds.x + bounds.width && Input.mouseY >= globalBounds.y && Input.mouseY <= globalBounds.y + bounds.height;
            }
        }
        
        public SpriteButton(float x, float y, float width, float height, string buttonText, Sprite sprite, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : this(x, y, width, height, buttonText, sprite, ButtonStyle.Default, onClick, onMouseEnter, onMouseLeave, onMousePress, onMouseRelease) { }

        public SpriteButton(float x, float y, float width, float height, string buttonText, Sprite sprite, ButtonStyle buttonStyle, Action onClick = null, Action onMouseEnter = null, Action onMouseLeave = null, Action onMousePress = null, Action onMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            ButtonText = buttonText;
            Sprite = sprite;
            this.buttonStyle = buttonStyle;

            this.onClick += onClick;
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
                onClick?.Invoke();
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

            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);
            
            DrawSprite(Sprite, Sprite.texture.width/2f, Sprite.texture.height/2f);
            
            NoStroke();
            Fill(buttonStyle.TextColor);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            var textX = 0f;
            var textY = 0f;
            if (buttonStyle.TextAlignment.Alignment == StringAlignment.Far)
                textX += bounds.width;
            else if (buttonStyle.TextAlignment.Alignment == StringAlignment.Center) textX += bounds.width / 2;
            if (buttonStyle.TextAlignment.LineAlignment == StringAlignment.Far)
                textY += bounds.height;
            else if (buttonStyle.TextAlignment.LineAlignment == StringAlignment.Center) textY += bounds.height / 2;
            graphics.DrawString(ButtonText, buttonStyle.Font, brush, textX, textY, buttonStyle.TextAlignment);
        }
    }
}