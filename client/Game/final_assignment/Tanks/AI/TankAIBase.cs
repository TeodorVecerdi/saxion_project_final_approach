using System;
using System.Diagnostics.CodeAnalysis;
using GXPEngine;

namespace physics_programming.final_assignment {
    public abstract class TankAIBase : GameObject {
        public Tank Tank;
        public bool Dead = false;
        protected const float AccuracyDegreeVariation = 30f;

        private readonly bool waitToShoot;
        protected readonly float Accuracy;
        protected readonly float TimeToShoot;
        protected float TimeLeftToShoot;

        protected TankAIBase(float accuracy, float timeToShoot = 2f) {
            waitToShoot = !(Math.Abs(timeToShoot) < 0.000001f);
            TimeToShoot = timeToShoot;
            TimeLeftToShoot = TimeToShoot;
            Accuracy = accuracy;
        }

        protected abstract void BarrelMove(Tank tank);
        protected abstract void TankMove(Tank tank);
        protected abstract void TankShoot(Tank tank);

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void Update() {
            if (waitToShoot) TimeLeftToShoot -= Time.deltaTime;
        }
    }
}