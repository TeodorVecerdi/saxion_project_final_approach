using GXPEngine;
using GXPEngine.Core;

namespace physics_programming.final_assignment.Components {
    public class Arrow : GameObject {
        public readonly float ScaleFactor;
        public readonly uint Color;
        public readonly uint LineWidth;
        public Vec2 StartPoint;
        public Vec2 Vector;

        public Arrow(Vec2 startPoint, Vec2 vector, float scale, uint color = 0xffffffff, uint lineWidth = 1) {
            StartPoint = startPoint;
            Vector = vector;
            ScaleFactor = scale;

            Color = color;
            LineWidth = lineWidth;
        }

        protected override void RenderSelf(GLContext glContext) {
            var endPoint = StartPoint + Vector * ScaleFactor;
            LineSegment.RenderLine(StartPoint, endPoint, Color, LineWidth, true);

            var smallVec = Vector.normalized * -10;
            var left = new Vec2(-smallVec.y, smallVec.x) + smallVec + endPoint;
            var right = new Vec2(smallVec.y, -smallVec.x) + smallVec + endPoint;

            LineSegment.RenderLine(endPoint, left, Color, LineWidth, true);
            LineSegment.RenderLine(endPoint, right, Color, LineWidth, true);
        }
    }
}