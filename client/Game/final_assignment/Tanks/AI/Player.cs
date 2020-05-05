using System;
using GXPEngine;
using physics_programming.final_assignment.Utils;

namespace physics_programming.final_assignment {
    public class Player : TankAIBase {
        public readonly float MaxVelocity;
        public new Tank Tank {
            get => base.Tank;
            set => base.Tank = value;
        }

        public Player(float px, float py, float maxVelocity) : base(1f, 0f) {
            MaxVelocity = maxVelocity;
            Tank = new Tank(px, py, this, TankMove, TankShoot, BarrelMove);
            AddChild(Tank);
        }

        protected override void BarrelMove(Tank tank) {
            Vec2 mousePos = Input.mousePosition;
            var desiredRotation = -tank.rotation + Vec2.Rad2Deg((float) Math.Atan2(mousePos.y - tank.Position.y, mousePos.x - tank.Position.x));
            var delta = desiredRotation - tank.Barrel.rotation;
            var shortestAngle = Mathf.Clamp(delta - Mathf.Floor(delta / 360f) * 360f, 0.0f, 360f);
            if (shortestAngle > 180)
                shortestAngle -= 360;
            if (Math.Abs(tank.Barrel.rotation - desiredRotation) > 0.5)
                tank.Barrel.rotation += shortestAngle * 0.15f;
        }

        protected override void TankShoot(Tank tank) {
            if (Input.GetMouseButtonDown(0)) {
                var g = (MyGame) game;
                var bullet = new Bullet(tank.Position, Vec2.GetUnitVectorDeg(tank.Barrel.rotation + tank.rotation), Tank) {rotation = tank.Barrel.rotation + tank.rotation};
                g.AddBullet(bullet);
            }
        }

        protected override void TankMove(Tank tank) {
            Controls(tank);
            if (tank.Acceleration != Vec2.Zero) {
                tank.Velocity += tank.Acceleration;
                if (tank.Velocity.sqrMagnitude >= MaxVelocity * MaxVelocity)
                    tank.Velocity = tank.Velocity.normalized * MaxVelocity;
            } else {
                var newVel = ExponentialDecay(tank, 5f);
                tank.Velocity = newVel;
            }

            tank.OldPosition = tank.Position;
            tank.Position += tank.Velocity * Time.deltaTime;
        }

        private void Controls(Tank tank) {
            var rotationAmount = 0f;
            if (Input.GetKey(Key.D))
                rotationAmount += 1f;

            if (Input.GetKey(Key.A))
                rotationAmount += -1f;

            rotationAmount *= MathUtils.Map(tank.Velocity.sqrMagnitude, 0, MaxVelocity * MaxVelocity, 0, 1);
            tank.rotation += rotationAmount;
            tank.Acceleration = Vec2.Right * 20f;
            tank.Acceleration.SetAngleDegrees(tank.rotation);

            var dir = 0f;
            if (Input.GetKey(Key.W)) dir += 1;
            if (Input.GetKey(Key.S)) dir -= 1;
            tank.Acceleration *= dir;
        }

        private Vec2 ExponentialDecay(Tank tank, float dampAmount) {
            // v' = v * e^-dt
            return tank.Velocity * Mathf.Exp(-dampAmount * Time.deltaTime);
        }
    }
}