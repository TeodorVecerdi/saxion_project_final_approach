using System;
using System.Drawing;
using GXPEngine;
using physics_programming.final_assignment.Utils;

namespace physics_programming.final_assignment {
    public class CircleCollider : EasyDraw {
        public readonly bool IsKinematic;
        public readonly int Radius;
        public Vec2 OldPosition;
        public Vec2 Position;

        private bool isVisible;

        public bool IsVisible {
            get => isVisible;
            set {
                isVisible = value;
                if (isVisible)
                    Draw(0x00, 0xff, 0x00);
                else
                    Clear(Color.Transparent);
            }
        }

        public CircleCollider(int radius, Vec2 offset, bool isKinematic = false, bool isVisible = true) : base(radius * 2 + 1, radius * 2 + 1) {
            Radius = radius;
            Position = offset;
            IsKinematic = isKinematic;

            UpdateScreenPosition();
            SetOrigin(Radius, Radius);

            IsVisible = isVisible;
        }

        private void Draw(byte red, byte green, byte blue) {
            NoFill();
            Stroke(red, green, blue);
            Ellipse(Radius, Radius, 2 * Radius, 2 * Radius);
        }

        private void UpdateScreenPosition() {
            x = Position.x;
            y = Position.y;
        }

        public CollisionInfo FindEarliestDestructibleLineCollision(Vec2 parentPosition, Vec2 parentVelocity, Vec2 parentOldPosition, float parentRotation) {
            var myGame = (MyGame) game;
            var collisionInfo = new CollisionInfo(Vec2.Zero, null, Mathf.Infinity);

            var (worldPosition, worldOldPosition) = LocalToWorldCoords(Position, OldPosition, parentPosition, parentOldPosition);
            var (rotatedPosition, rotatedOldPosition) = ApplyRotation(worldPosition, parentPosition, worldOldPosition, parentOldPosition, parentRotation);

            foreach (var line in myGame.DestructibleLines) {
                var collInfo = CollisionUtils.CircleDestructibleLineCollision(rotatedPosition, rotatedOldPosition, parentVelocity * Time.deltaTime, Radius, line);
                if (collInfo != null && collInfo.TimeOfImpact < collisionInfo.TimeOfImpact)
                    collisionInfo = new CollisionInfo(collInfo.Normal, null, collInfo.TimeOfImpact);
            }

            return float.IsPositiveInfinity(collisionInfo.TimeOfImpact) ? null : collisionInfo;
        }

        public CollisionInfo FindEarliestLineCollision(Vec2 parentPosition, Vec2 parentVelocity, Vec2 parentOldPosition, float parentRotation) {
            var myGame = (MyGame) game;
            var collisionInfo = new CollisionInfo(Vec2.Zero, null, Mathf.Infinity);

            var (worldPosition, worldOldPosition) = LocalToWorldCoords(Position, OldPosition, parentPosition, parentOldPosition);
            var (rotatedPosition, rotatedOldPosition) = ApplyRotation(worldPosition, parentPosition, worldOldPosition, parentOldPosition, parentRotation);

            foreach (var line in myGame.Lines) {
                var collInfo = CollisionUtils.CircleLineCollision(rotatedPosition, rotatedOldPosition, parentVelocity * Time.deltaTime, Radius, line);
                if (collInfo != null && collInfo.TimeOfImpact < collisionInfo.TimeOfImpact)
                    collisionInfo = new CollisionInfo(collInfo.Normal, null, collInfo.TimeOfImpact);
            }

            return float.IsPositiveInfinity(collisionInfo.TimeOfImpact) ? null : collisionInfo;
        }

        public CollisionInfo FindEarliestChunkCollision(Vec2 parentPosition, Vec2 parentVelocity, Vec2 parentOldPosition, float parentRotation) {
            var myGame = (MyGame) game;
            var collisionInfo = new CollisionInfo(Vec2.Zero, null, Mathf.Infinity);

            var (worldPosition, worldOldPosition) = LocalToWorldCoords(Position, OldPosition, parentPosition, parentOldPosition);
            var (rotatedPosition, rotatedOldPosition) = ApplyRotation(worldPosition, parentPosition, worldOldPosition, parentOldPosition, parentRotation);

            foreach (var chunk in myGame.DestructibleChunks) {
                var collInfo = CollisionUtils.CircleChunkCollision(rotatedPosition, rotatedOldPosition, parentVelocity, Radius, chunk);
                if (collInfo != null && collInfo.TimeOfImpact < collisionInfo.TimeOfImpact)
                    collisionInfo = new CollisionInfo(collInfo.Normal, null, collInfo.TimeOfImpact);
            }

            return float.IsPositiveInfinity(collisionInfo.TimeOfImpact) ? null : collisionInfo;
        }

        public CollisionInfo FindEarliestBlockCollision(Vec2 parentPosition, Vec2 parentVelocity, Vec2 parentOldPosition, float parentRotation) {
            var myGame = (MyGame) game;
            var collisionInfo = new CollisionInfo(Vec2.Zero, null, Mathf.Infinity);

            var (worldPosition, worldOldPosition) = LocalToWorldCoords(Position, OldPosition, parentPosition, parentOldPosition);
            var (rotatedPosition, rotatedOldPosition) = ApplyRotation(worldPosition, parentPosition, worldOldPosition, parentOldPosition, parentRotation);

            foreach (var block in myGame.DestructibleBlocks) {
                var collInfo = CollisionUtils.CircleBlockCollision(rotatedPosition, rotatedOldPosition, parentVelocity, Radius, block);
                if (collInfo != null && collInfo.TimeOfImpact < collisionInfo.TimeOfImpact)
                    collisionInfo = new CollisionInfo(collInfo.Normal, null, collInfo.TimeOfImpact);
            }

            return float.IsPositiveInfinity(collisionInfo.TimeOfImpact) ? null : collisionInfo;
        }

        public static ValueTuple<Vec2, Vec2> LocalToWorldCoords(Vec2 position, Vec2 oldPosition, Vec2 parentPosition, Vec2 parentOldPosition) {
            return ValueTuple.Create(position + parentPosition, oldPosition + parentOldPosition);
        }

        public static ValueTuple<Vec2, Vec2> ApplyRotation(Vec2 position, Vec2 parentPosition, Vec2 oldPosition, Vec2 parentOldPosition, float parentRotation) {
            parentRotation *= Mathf.Deg2Rad;
            var sin = Mathf.Sin(parentRotation);
            var cos = Mathf.Cos(parentRotation);
            var newCircleX = (position.x - parentPosition.x) * cos - (position.y - parentPosition.y) * sin + parentPosition.x;
            var newCircleY = (position.x - parentPosition.x) * sin - (position.y - parentPosition.y) * cos + parentPosition.y;
            var newPos = new Vec2(newCircleX, newCircleY);

            var newCircleOldX = (oldPosition.x - parentOldPosition.x) * cos - (oldPosition.y - parentOldPosition.y) * sin + parentOldPosition.x;
            var newCircleOldY = (oldPosition.x - parentOldPosition.x) * sin - (oldPosition.y - parentOldPosition.y) * cos + parentOldPosition.y;
            var newOldPos = new Vec2(newCircleOldX, newCircleOldY);
            return ValueTuple.Create(newPos, newOldPos);
        }
    }
}