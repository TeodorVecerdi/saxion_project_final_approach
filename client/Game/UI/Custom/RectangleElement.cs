using System;
using System.Drawing;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui {
    public class RectangleElement : EasyDraw {
        private readonly Rectangle bounds;
        private Color fillColor;
        private Color strokeColor;
        private float strokeWeight;
        public bool ShouldRepaint { private get; set; }

        public RectangleElement(float x, float y, float width, float height, Color fillColor, Color strokeColor, float strokeWeight)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.fillColor = fillColor;
            this.strokeColor = strokeColor;
            this.strokeWeight = strokeWeight;
            SetXY(x, y);
            Draw();
        }

        private void Update() {
            if (ShouldRepaint) {
                ShouldRepaint = false;
                Draw();
            }
        }

        private void Draw() {
            Clear(Color.Transparent);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Fill(fillColor);
            Stroke(strokeColor);
            if(Math.Abs(strokeWeight) < 0.00001f) NoStroke();
            else StrokeWeight(strokeWeight);
            Rect(0, 0, bounds.width, bounds.height);
        }
        
    }
}