using System;
using GXPEngine;
using GXPEngine.Core;

namespace physics_programming.final_assignment {
    public class DoubleDestructibleLineSegment : GameObject {
        public readonly CircleCollider EndCollider;
        public readonly CircleCollider StartCollider;
        public readonly DestructibleLineSegment SideA;
        public readonly DestructibleLineSegment SideB;
        public bool ShouldRemove;

        public DoubleDestructibleLineSegment(Vec2 start, Vec2 end, uint color = 0xffffffff, uint lineWidth = 1) {
            SideA = new DestructibleLineSegment(start, end, 0xffff0000, lineWidth);
            SideB = new DestructibleLineSegment(end, start, 0xff0000ff, lineWidth);
            AddChild(SideA);
            AddChild(SideB);
            StartCollider = new CircleCollider(0, start, true);
            EndCollider = new CircleCollider(0, end, true);
            AddChild(StartCollider);
            AddChild(EndCollider);
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														RenderSelf()
        //------------------------------------------------------------------------------------------------------------------------
        protected override void RenderSelf(GLContext glContext) { }

        public static ValueTuple<DoubleDestructibleLineSegment, DoubleDestructibleLineSegment> Split(DoubleDestructibleLineSegment a, Vec2 point, float size) {
            var split1 = DestructibleLineSegment.Split(a.SideA, point, size);
            var split2 = DestructibleLineSegment.Split(a.SideB, point, size);
            DoubleDestructibleLineSegment lineLeft;
            DoubleDestructibleLineSegment lineRight;
            if (split1.Item1 == null || split2.Item2 == null)
                lineLeft = null;
            else
                lineLeft = new DoubleDestructibleLineSegment(split1.Item1.Start, split1.Item1.End, split1.Item1.Color, split1.Item1.LineWidth);

            if (split1.Item2 == null || split2.Item1 == null)
                lineRight = null;
            else
                lineRight = new DoubleDestructibleLineSegment(split1.Item2.Start, split1.Item2.End, split1.Item2.Color, split1.Item2.LineWidth);
            return ValueTuple.Create(lineLeft, lineRight);
        }
    }
}