using System;
using System.Drawing;
using GXPEngine;

namespace physics_programming.final_assignment {
    public class DumbEnemy : TankAIBase {
        private readonly AttackIndicator attackIndicator;

        public DumbEnemy(float px, float py, float accuracy = 1f, float timeToShoot = 2f) : base(accuracy, timeToShoot) {
            Tank = new Tank(px, py, this, TankMove, TankShoot, BarrelMove, 0xff669a1c);
            AddChild(Tank);

            attackIndicator = new AttackIndicator(TimeToShoot, radius: 50, color: Color.FromArgb(127, 55, 255, 55));
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
            // Basic aiming
            var target = ((MyGame) game).Player.Tank;
            var aimTarget = target.Position;
            var desiredRotation = -tank.rotation + Vec2.Rad2Deg((float) Math.Atan2(aimTarget.y - tank.Position.y, aimTarget.x - tank.Position.x));
            var delta = desiredRotation - tank.Barrel.rotation;
            var shortestAngle = Mathf.Clamp(delta - Mathf.Floor(delta / 360f) * 360f, 0.0f, 360f);
            if (shortestAngle > 180)
                shortestAngle -= 360;
            if (Math.Abs(tank.Barrel.rotation - desiredRotation) > 0.5)
                tank.Barrel.rotation += shortestAngle * 0.10f;

            attackIndicator.UpdateIndicator(TimeLeftToShoot);
        }
    }
}