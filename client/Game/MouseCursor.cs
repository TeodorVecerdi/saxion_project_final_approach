using GXPEngine;
using GXPEngine.Core;

namespace game {
    public class MouseCursor : GameObject {
        private static MouseCursor instance;
        public static MouseCursor Instance => instance ?? (instance = new MouseCursor());
        
        private readonly Sprite normal, button, text;
        private Sprite active;

        public MouseCursor() {
            normal = new Sprite("data/sprites/cursors/cursor_normal.png") {scale = 0.05f};
            button = new Sprite("data/sprites/cursors/cursor_button.png") {scale = 0.05f};
            text = new Sprite("data/sprites/cursors/cursor_text.png") {scale = 0.05f};
            Normal();
        }

        public void Normal() => active = normal;
        public void Button() => active = button;
        public void Text() => active = text;

        private void Update() {
            SetXY(Input.mouseX, Input.mouseY);
        }

        protected override void RenderSelf(GLContext glContext) {
            active?.Render(glContext);
        }
    }
}