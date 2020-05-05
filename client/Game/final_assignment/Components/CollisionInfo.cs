using GXPEngine;

namespace physics_programming.final_assignment {
    public class CollisionInfo {
        public readonly float TimeOfImpact;
        public readonly GameObject Other;
        public readonly Vec2 Normal;

        public CollisionInfo(Vec2 normal, GameObject other, float timeOfImpact) {
            Normal = normal;
            Other = other;
            TimeOfImpact = timeOfImpact;
        }
    }
}