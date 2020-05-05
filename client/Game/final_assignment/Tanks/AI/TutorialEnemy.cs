using System;
using System.Drawing;
using GXPEngine;

namespace physics_programming.final_assignment {
    public class TutorialEnemy : TankAIBase {
        private float desiredRotation = 0f;

        public TutorialEnemy(float px, float py, float accuracy = 1f) : base(accuracy) {
            Tank = new Tank(px, py, this, TankMove, TankShoot, BarrelMove, 0xffaa2222);
            AddChild(Tank);
        }

        protected override void TankMove(Tank tank) { }
        protected override void TankShoot(Tank tank) { }

        protected override void BarrelMove(Tank tank) {
            // Rotates randomly
            if (tank.Barrel.rotation > 360) tank.Barrel.rotation -= 360; 
            if (tank.Barrel.rotation < 0) tank.Barrel.rotation += 360; 
            if (Math.Abs(tank.Barrel.rotation - desiredRotation) > 0.5f) {
                var delta = desiredRotation - tank.Barrel.rotation;
                var shortestAngle = Mathf.Clamp(delta - Mathf.Floor(delta / 360f) * 360f, 0.0f, 360f);
                if (shortestAngle > 180)
                    shortestAngle -= 360;
                if (Math.Abs(tank.Barrel.rotation - desiredRotation) > 0.5)
                    tank.Barrel.rotation += shortestAngle * 0.05f;
            } else {
                desiredRotation = Rand.Range(0f, 360f);
            }
        }
    }
}