using System;
using System.Drawing;
using System.Drawing.Text;
using game.utils;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class PlayerAvatarElement : EasyDraw {
        private readonly Rectangle bounds;
        private Action onClick;
        private LabelStyle labelStyle;

        private string playerUsername;
        private Sprite playerAvatar;
        private bool IsMouseOnTop {
            get {
                var globalBounds = TransformPoint(0, 0);
                return Input.mouseX >= globalBounds.x && Input.mouseX <= globalBounds.x + bounds.width && Input.mouseY >= globalBounds.y && Input.mouseY <= globalBounds.y + bounds.width;
            }
        }

        public PlayerAvatarElement(float x, float y, string playerUsername, int avatarIndex, LabelStyle labelStyle, Action onClick = null, float spriteSize = 128f)
            : base(Mathf.Ceiling(spriteSize), Mathf.Ceiling(spriteSize) * 2) {
            bounds = new Rectangle(x, y, spriteSize, spriteSize * 2f);
            this.playerUsername = playerUsername;
            this.onClick += onClick;
            this.labelStyle = labelStyle;

            var playerAvatarPath = "data/sprites/avatars/";
            if (avatarIndex < 5) playerAvatarPath += $"female_{avatarIndex + 1}_";
            else playerAvatarPath += $"male_{avatarIndex - 4}_";
            if (Math.Abs(spriteSize - 128f) < 0.00001f) playerAvatarPath += "128.png";
            else playerAvatarPath += "64.png";
            playerAvatar = new Sprite(playerAvatarPath, true, false);
            SetXY(x, y);
            Draw();
        }

        private void Update() {
            if(IsMouseOnTop && Input.GetMouseButtonUp(GXPEngine.Button.LEFT)) onClick?.Invoke();
        }

        private void Draw() {
            Clear(Color.Transparent);
            DrawSprite(playerAvatar, playerAvatar.texture.width / 2f, playerAvatar.texture.height / 2f);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            Fill(labelStyle.TextColor);
            graphics.DrawString(playerUsername, labelStyle.Font, brush, new RectangleF(0, bounds.width+10, bounds.width, bounds.width-10), labelStyle.TextAlignment);
        }
    }
}