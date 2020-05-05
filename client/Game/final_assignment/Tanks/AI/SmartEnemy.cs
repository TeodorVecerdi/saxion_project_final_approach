using System;
using System.Drawing;
using GXPEngine;

namespace physics_programming.final_assignment {
    public class SmartEnemy : TankAIBase {
        private readonly AttackIndicator attackIndicator;

        public SmartEnemy(float px, float py, float accuracy = 1f, float timeToShoot = 2f) : base(accuracy, timeToShoot) {
            Tank = new Tank(px, py, this, TankMove, TankShoot, BarrelMove, 0xffffeabc);
            AddChild(Tank);
            attackIndicator = new AttackIndicator(TimeToShoot, radius: 50, color: Color.FromArgb(127, 55, 55, 255));
            attackIndicator.SetXY(25, -50);
            Tank.Barrel.AddChild(attackIndicator);
        }

        protected override void TankMove(Tank tank) {
            // Enemy tanks don't move
        }

        protected override void TankShoot(Tank tank) {
            if (TimeLeftToShoot > 0f)
                return;

            var g = (MyGame) game;
            var accuracyOffset = Rand.Range(-AccuracyDegreeVariation + Accuracy * AccuracyDegreeVariation, AccuracyDegreeVariation - Accuracy * AccuracyDegreeVariation);
            var bulletRotation = accuracyOffset + tank.Barrel.rotation + tank.rotation;
            var bullet = new Bullet(tank.Position, Vec2.GetUnitVectorDeg(bulletRotation), Tank) {rotation = bulletRotation};
            g.AddBullet(bullet);
            TimeLeftToShoot = TimeToShoot;
        }

        protected override void BarrelMove(Tank tank) {
            // Advanced aiming
            // vars
            var player = ((MyGame) game).Player;
            var targetTank = player.Tank;
            var targetVelocityWhenShot = targetTank.Velocity + TimeLeftToShoot * targetTank.Acceleration;
            if (targetVelocityWhenShot.sqrMagnitude >= player.MaxVelocity * player.MaxVelocity)
                targetVelocityWhenShot = targetVelocityWhenShot.normalized * player.MaxVelocity;
            
            var positionDelta = targetTank.Position - tank.Position;

            // quadratic equation
            var a = targetVelocityWhenShot.Dot(targetVelocityWhenShot) - Bullet.Speed * Bullet.Speed;
            var b = 2f * targetVelocityWhenShot.Dot(positionDelta);
            var c = positionDelta.Dot(positionDelta);
            var delta = b * b - 4f * a * c;
            if (delta <= 0f) return;
            var t = 2f * c / (Mathf.Sqrt(delta) - b);
            if (t < 0f) return;

            // rotation
            var aimTarget = targetTank.Position + t * targetVelocityWhenShot;
            var desiredRotation = -tank.rotation + Vec2.Rad2Deg((float) Math.Atan2(aimTarget.y - tank.Position.y, aimTarget.x - tank.Position.x));
            var deltaRotation = desiredRotation - tank.Barrel.rotation;
            var shortestAngle = Mathf.Clamp(deltaRotation - Mathf.Floor(deltaRotation / 360f) * 360f, 0.0f, 360f);
            if (shortestAngle > 180)
                shortestAngle -= 360;
            if (Math.Abs(tank.Barrel.rotation - desiredRotation) > 0.5)
                tank.Barrel.rotation += shortestAngle * 0.10f;

            attackIndicator.UpdateIndicator(TimeLeftToShoot);
        }
    }
}