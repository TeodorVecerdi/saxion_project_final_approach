using System.Drawing;
using GXPEngine;
using GXPEngine.Core;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class Image : EasyDraw {
        private readonly Rectangle bounds;
        private new readonly Texture2D texture;
        private readonly Sprite sprite;
        private readonly bool useSprite;
        
        public bool ShouldRepaint { private get; set; }

        public Image(float x, float y, Texture2D image) : this(x, y, image.width, image.height, image) {
        }
        
        public Image(float x, float y, Sprite image) : this(x, y, image.width, image.height, image) {
        }

        public Image(float x, float y, float width, float height, Texture2D image) : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            useSprite = false;
            texture = image;
            Draw();
            SetXY(x, y);
        }

        public Image(float x, float y, float width, float height, Sprite image) : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            useSprite = true;
            sprite = image;
            Draw();
            SetXY(x, y);
        }

        private void Update() {
            if (ShouldRepaint) {
                Draw();
                ShouldRepaint = false;
            }
        }

        private void Draw() {
            Clear(Color.Transparent);
            if(useSprite) DrawSprite(sprite, sprite.texture.width/2f, sprite.texture.height/2f);
            else DrawTexture(texture, texture.width/2f, texture.height/2f);
        }
    }
}