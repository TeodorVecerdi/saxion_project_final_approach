using GXPEngine;
using GXPEngine.Core;

namespace game {
    public class MouseCursor : GameObject {
        private static MouseCursor instance;

        private readonly Sprite normal, button, text;
        private Sprite active;
        public static MouseCursor Instance => instance ?? (instance = new MouseCursor());

        public MouseCursor() {
            name = "Mouse Cursor";
            normal = new Sprite("data/sprites/cursors/cursor_normal.png") {scale = 0.05f};
            normal.SetOrigin(512, 512);
            button = new Sprite("data/sprites/cursors/cursor_button.png") {scale = 0.05f};
            button.SetOrigin(512, 512);
            text = new Sprite("data/sprites/cursors/cursor_text.png") {scale = 0.05f};
            text.SetOrigin(512, 512);
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