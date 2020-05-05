using System;
using GXPEngine;
using GXPEngine.Core;
using GXPEngine.OpenGL;

namespace physics_programming.final_assignment.Components {
    public class LineSegment : GameObject {
        public readonly uint Color;
        public readonly uint LineWidth;
        public Vec2 End;
        public Vec2 Start;

        public LineSegment(Vec2 start, Vec2 end, uint color = 0xffffffff, uint lineWidth = 1) {
            Start = start;
            End = end;
            Color = color;
            LineWidth = lineWidth;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        protected override void RenderSelf(GLContext glContext) {
            if (game != null)
                RenderLine(Start, End, Color, LineWidth);
        }

        public static void RenderLine(Vec2 start, Vec2 end, uint color = 0xffffffff, uint lineWidth = 1, bool useGlobalCoords = false) {
            RenderLine(start.x, start.y, end.x, end.y, color, lineWidth, useGlobalCoords);
        }

        public static void RenderLine(float startX, float startY, float endX, float endY, uint color = 0xffffffff, uint lineWidth = 1, bool useGlobalCoords = false) {
            if (useGlobalCoords) GL.LoadIdentity();
            GL.Disable(GL.TEXTURE_2D);
            GL.LineWidth(lineWidth);
            GL.Color4ub((byte) ((color >> 16) & 0xff), (byte) ((color >> 8) & 0xff), (byte) (color & 0xff), (byte) ((color >> 24) & 0xff));
            float[] vertices = {startX, startY, endX, endY};
            GL.EnableClientState(GL.VERTEX_ARRAY);
            GL.VertexPointer(2, GL.FLOAT, 0, vertices);
            GL.DrawArrays(GL.LINES, 0, 2);
            GL.DisableClientState(GL.VERTEX_ARRAY);
            GL.Enable(GL.TEXTURE_2D);
        }

        public static ValueTuple<LineSegment, LineSegment> Split(LineSegment a, Vec2 point, float size) {
            var line1Start = a.Start;
            var line1Vector = point - line1Start;
            var line1Length = line1Vector.magnitude - size;
            line1Vector = line1Vector.normalized * line1Length;
            var line1End = line1Start + line1Vector;
            var line1 = new LineSegment(line1Start, line1End, a.Color, a.LineWidth);

            var line2End = a.End;
            var line2Vector = point - line2End;
            var line2Length = line2Vector.magnitude - size;
            line2Vector = line2Vector.normalized * line2Length;
            var line2Start = line2End + line2Vector;
            var line2 = new LineSegment(line2Start, line2End, a.Color, a.LineWidth);

            if ((line1End - line1Start).sqrMagnitude <= Globals.World.DestructibleLineMinLength * Globals.World.DestructibleLineMinLength) line1 = null;
            if ((line2End - line2Start).sqrMagnitude <= Globals.World.DestructibleLineMinLength * Globals.World.DestructibleLineMinLength) line2 = null;
            return ValueTuple.Create(line1, line2);
        }
    }
}