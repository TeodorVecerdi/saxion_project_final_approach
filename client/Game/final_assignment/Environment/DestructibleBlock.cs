using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DelaunayVoronoi;
using GXPEngine;
using physics_programming.final_assignment.Components;
using physics_programming.final_assignment.Utils;

namespace physics_programming.final_assignment {
    public class DestructibleBlock : EasyDraw {
        public readonly LineSegment Length1;
        public readonly LineSegment Length2;

        public readonly LineSegment Side1;
        public readonly LineSegment Side2;
        public readonly List<Vec2> BlockVertices;

        public bool ShouldRemove;

        private readonly float blockWidth;
        private readonly Vec2 end;
        private readonly Vec2 start;

        public DestructibleBlock(float blockWidth, Vec2 start, Vec2 end) : base(Globals.WIDTH, Globals.HEIGHT) {
            this.blockWidth = blockWidth;
            this.start = start;
            this.end = end;

            var length1Start = start;
            var length1End = end;
            Length1 = new LineSegment(length1Start, length1End, 0x0);
            var length1Vector = end - start;
            var length1Normal = length1Vector.Normal();

            var side1Start = start - length1Normal * blockWidth;
            var side1End = start;
            Side1 = new LineSegment(side1Start, side1End, 0x0);

            var side2Start = end;
            var side2End = end - length1Normal * blockWidth;
            Side2 = new LineSegment(side2Start, side2End, 0x0);

            var length2Start = Side2.End;
            var length2End = Side1.Start;
            Length2 = new LineSegment(length2Start, length2End, 0x0);

            AddChild(Length1);
            AddChild(Length2);
            AddChild(Side1);
            AddChild(Side2);

            //Setup vertices and draw
            BlockVertices = new List<Vec2>();
            BlockVertices.AddRange(new[] {Length1.Start, Length2.End, Length2.Start, Length1.End});
            Draw();
        }

        public DestructibleBlock(List<Vec2> vertices) : base(Globals.WIDTH, Globals.HEIGHT) {
            blockWidth = (vertices[1] - vertices[0]).magnitude;
            start = vertices[0];
            end = vertices[3];
            BlockVertices = vertices;
            Length1 = new LineSegment(vertices[0], vertices[3], 0x0);
            Length2 = new LineSegment(vertices[2], vertices[1], 0x0);
            Side1 = new LineSegment(vertices[1], vertices[0], 0x0);
            Side2 = new LineSegment(vertices[3], vertices[2], 0x0);

            AddChild(Length1);
            AddChild(Length2);
            AddChild(Side1);
            AddChild(Side2);
            Draw();
        }

        private void Draw() {
            Clear(Color.Transparent);
            Stroke(Color.FromArgb(73, 21, 6));
            Fill(Color.FromArgb(73, 21, 6));

            // Fill(255, 255, 255, 100);
            Quad((int) BlockVertices[0].x, (int) BlockVertices[0].y,
                (int) BlockVertices[1].x, (int) BlockVertices[1].y,
                (int) BlockVertices[2].x, (int) BlockVertices[2].y,
                (int) BlockVertices[3].x, (int) BlockVertices[3].y
            );
        }

        public static IEnumerable<DestructibleChunk> Destruct(DestructibleBlock block) {
            var width = (block.end - block.start).magnitude;
            var height = block.blockWidth;
            var originalQuad = new Quad(new Vec2(0, 0), new Vec2(width, 0), new Vec2(width, height), new Vec2(0, height));
            var targetQuad = new Quad(block.BlockVertices[1], block.BlockVertices[2], block.BlockVertices[3], block.BlockVertices[0]);

            var delaunay = new DelaunayTriangulator();
            var points = delaunay.GeneratePoints(Globals.World.BlockDestructionPoints, width, height);
            var triangles = delaunay.BowyerWatson(points);

            return
                from triangle in triangles
                let v0 = MathUtils.MapPointOnQuad(new Vec2((float) triangle.Vertices[0].X, (float) triangle.Vertices[0].Y), originalQuad, targetQuad)
                let v1 = MathUtils.MapPointOnQuad(new Vec2((float) triangle.Vertices[1].X, (float) triangle.Vertices[1].Y), originalQuad, targetQuad)
                let v2 = MathUtils.MapPointOnQuad(new Vec2((float) triangle.Vertices[2].X, (float) triangle.Vertices[2].Y), originalQuad, targetQuad)
                select new DestructibleChunk(v0, v1, v2);
        }
    }
}