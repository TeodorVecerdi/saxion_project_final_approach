using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GXPEngine;

namespace physics_programming.final_assignment {
    public class Tank : Sprite {
        public readonly Barrel Barrel;
        public readonly List<CircleCollider> Colliders;
        public readonly TankAIBase AIRef;

        public Vec2 Acceleration;
        public Vec2 OldPosition;
        public Vec2 Position;
        public Vec2 Velocity;
        private readonly Action<Tank> barrelMove;

        private readonly Action<Tank> tankMove;
        private readonly Action<Tank> tankShoot;

        public Tank(float px, float py, TankAIBase aiRef, Action<Tank> tankMove, Action<Tank> tankShoot, Action<Tank> barrelMove, uint color = 0xffffffff) : base("data/assets/bodies/t34.png") {
            Position.x = px;
            Position.y = py;
            AIRef = aiRef;
            this.tankMove = tankMove;
            this.tankShoot = tankShoot;
            this.barrelMove = barrelMove;
            this.color = color;

            SetOrigin(texture.width / 2f, texture.height / 2f);
            Barrel = new Barrel(color);
            Barrel.SetOrigin(34, 28);
            AddChild(Barrel);

            Colliders = new List<CircleCollider> {
                new CircleCollider(40, new Vec2(-texture.width / 2f + 45, 0)),
                new CircleCollider(40, new Vec2(texture.width / 2f - 45, 0))
            };
            Colliders.ForEach(collider => {
                collider.IsVisible = true;
                AddChild(collider);
            });
        }

        private void Collisions() {
            var collisionInfo = new CollisionInfo(Vec2.Zero, null, Mathf.Infinity);
            Colliders.Select(collider => collider.FindEarliestLineCollision(Position, Velocity, OldPosition, rotation))
                .Where(earliest => earliest != null && earliest.TimeOfImpact < collisionInfo.TimeOfImpact).ToList()
                .ForEach(earliest => collisionInfo = new CollisionInfo(earliest.Normal, null, earliest.TimeOfImpact));
            Colliders.Select(collider => collider.FindEarliestDestructibleLineCollision(Position, Velocity, OldPosition, rotation))
                .Where(earliest => earliest != null && earliest.TimeOfImpact < collisionInfo.TimeOfImpact).ToList()
                .ForEach(earliest => collisionInfo = new CollisionInfo(earliest.Normal, null, earliest.TimeOfImpact));
            Colliders.Select(collider => collider.FindEarliestChunkCollision(Position, Velocity, OldPosition, rotation))
                .Where(earliest => earliest != null && earliest.TimeOfImpact < collisionInfo.TimeOfImpact).ToList()
                .ForEach(earliest => collisionInfo = new CollisionInfo(earliest.Normal, null, earliest.TimeOfImpact));
            Colliders.Select(collider => collider.FindEarliestBlockCollision(Position, Velocity, OldPosition, rotation))
                .Where(earliest => earliest != null && earliest.TimeOfImpact < collisionInfo.TimeOfImpact).ToList()
                .ForEach(earliest => collisionInfo = new CollisionInfo(earliest.Normal, null, earliest.TimeOfImpact));
            if (!float.IsPositiveInfinity(collisionInfo.TimeOfImpact))
                Position = OldPosition + Time.deltaTime * (collisionInfo.TimeOfImpact - 0.00001f) * Velocity;
        }

        private void UpdateScreenPosition() {
            x = Position.x;
            y = Position.y;
        }

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void Update() {
            tankMove?.Invoke(this);
            Collisions();

            barrelMove?.Invoke(this);
            tankShoot?.Invoke(this);

            UpdateScreenPosition();
        }
    }
}