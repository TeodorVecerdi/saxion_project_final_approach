namespace GXPEngine.Core {
    public class Collider {
        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        public virtual bool HitTest(Collider other) {
            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														HitTest()
        //------------------------------------------------------------------------------------------------------------------------		
        public virtual bool HitTestPoint(float x, float y) {
            return false;
        }

        public virtual float TimeOfImpact(Collider other, float vx, float vy, out Vector2 normal) {
            normal = new Vector2();
            return float.MaxValue;
        }

        /// <summary>
        ///     If this collider and the collider other are overlapping, this method returns useful collision info such as
        ///     the collision normal, the point of impact, and the penetration depth,
        ///     contained in a Collision object (the time of impact field will always be zero).
        ///     If they are not overlapping, this method returns null.
        /// </summary>
        public virtual Collision GetCollisionInfo(Collider other) {
            return null;
        }
    }
}