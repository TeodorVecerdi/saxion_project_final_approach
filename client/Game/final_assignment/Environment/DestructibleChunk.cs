using System.Drawing;
using GXPEngine;
using physics_programming.final_assignment.Components;

namespace physics_programming.final_assignment {
    public class DestructibleChunk : EasyDraw {
        public readonly LineSegment LineSegment0;
        public readonly LineSegment LineSegment1;
        public readonly LineSegment LineSegment2;
        public readonly LineSegment RLineSegment0;
        public readonly LineSegment RLineSegment1;
        public readonly LineSegment RLineSegment2;

        public bool ShouldRemove;
        public Vec2 P0;
        public Vec2 P1;
        public Vec2 P2;

        public DestructibleChunk(Vec2 p0, Vec2 p1, Vec2 p2) : base(Globals.WIDTH, Globals.HEIGHT) {
            P0 = p0;
            P1 = p1;
            P2 = p2;

            LineSegment0 = new LineSegment(P0, P1);
            LineSegment1 = new LineSegment(P1, P2);
            LineSegment2 = new LineSegment(P2, P0);
            RLineSegment0 = new LineSegment(P1, P0);
            RLineSegment1 = new LineSegment(P2, P1);
            RLineSegment2 = new LineSegment(P0, P2);
            Draw();
        }

        public void Draw() {
            Clear(Color.Transparent);
            Stroke(Color.FromArgb(255, 110, 98));
            Fill(Color.FromArgb(73, 21, 6));
            Triangle(P0.x, P0.y, P1.x, P1.y, P2.x, P2.y);
        }
    }
}