using GXPEngine;
using GXPEngine.Core;

namespace game {
    public class AnimatedSprite : GameObject {
        public uint Color = 0xffffffff;
        private readonly float frameTime;
        private readonly int cols;
        private readonly int rows;
        private readonly Texture2D texture;
        private readonly Vector2 uvSize;
        private float frameTimer;

        private int currentCol;
        private int currentRow;
        private Vector2 currentUV;

        public AnimatedSprite(Texture2D texture, int cols, int rows, float frameTime) {
            name = $"AnimatedSprite [{texture.filename}";
            this.texture = texture;
            this.cols = cols;
            this.rows = rows;
            this.frameTime = frameTime;

            uvSize = new Vector2(1f / cols, 1f / rows);
            currentCol = 0;
            currentRow = 0;
            frameTimer = frameTime;
            SetUV();
        }

        private void SetUV() {
            currentUV.x = currentCol * uvSize.x;
            currentUV.y = currentRow * uvSize.y;
        }

        private void Update() {
            if (frameTimer <= 0f) {
                currentCol++;
                if (currentCol == cols) {
                    currentCol = 0;
                    currentRow++;
                }

                if (currentRow == rows)
                    currentRow = 0;

                frameTimer = frameTime;
                SetUV();
            }

            frameTimer -= Time.deltaTime;
        }

        protected override void RenderSelf(GLContext glContext) {
            var uvs = new[] {
                currentUV.x, currentUV.y,
                currentUV.x + uvSize.x, currentUV.y,
                currentUV.x + uvSize.x, currentUV.y + uvSize.y,
                currentUV.x, currentUV.y + uvSize.y
            };
            var verts = new[] {
                0, 0,
                texture.width * uvSize.x * scaleX, 0,
                texture.width * uvSize.x * scaleX, texture.height * uvSize.y * scaleY,
                0, texture.height * uvSize.y * scaleY
            };
            texture.Bind();
            glContext.SetColor((byte) ((Color >> 16) & 0xFF),
                (byte) ((Color >> 8) & 0xFF),
                (byte) (Color & 0xFF),
                (byte) ((Color >> 24) & 0xFF));
            glContext.DrawQuad(verts, uvs);
            glContext.SetColor(1, 1, 1, 1);
            texture.Unbind();
        }
    }
}