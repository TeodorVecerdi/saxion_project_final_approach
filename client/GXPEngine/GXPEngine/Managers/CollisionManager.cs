using System;
using System.Collections.Generic;
using System.Reflection;

namespace GXPEngine {
    //------------------------------------------------------------------------------------------------------------------------
    //														CollisionManager
    //------------------------------------------------------------------------------------------------------------------------
    public class CollisionManager {
        public static bool SafeCollisionLoop = true;
        private readonly Dictionary<GameObject, ColliderInfo> _collisionReferences = new Dictionary<GameObject, ColliderInfo>();
        private readonly List<ColliderInfo> activeColliderList = new List<ColliderInfo>();

        private readonly List<GameObject> colliderList = new List<GameObject>();

        private bool collisionLoopActive;

        //------------------------------------------------------------------------------------------------------------------------
        //														CollisionManager()
        //------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------------------------
        //														Step()
        //------------------------------------------------------------------------------------------------------------------------
        public void Step() {
            collisionLoopActive = SafeCollisionLoop;
            for (var i = activeColliderList.Count - 1; i >= 0; i--) {
                var info = activeColliderList[i];
                for (var j = colliderList.Count - 1; j >= 0; j--) {
                    if (j >= colliderList.Count) continue; //fix for removal in loop
                    var other = colliderList[j];
                    if (info.gameObject != other)
                        if (info.gameObject.HitTest(other))
                            if (info.onCollision != null)
                                info.onCollision(other);
                }
            }

            collisionLoopActive = false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        //												 GetCurrentCollisions()
        //------------------------------------------------------------------------------------------------------------------------
        public GameObject[] GetCurrentCollisions(GameObject gameObject) {
            var list = new List<GameObject>();
            for (var j = colliderList.Count - 1; j >= 0; j--) {
                if (j >= colliderList.Count) continue; //fix for removal in loop
                var other = colliderList[j];
                if (gameObject != other)
                    if (gameObject.HitTest(other))
                        list.Add(other);
            }

            return list.ToArray();
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Add()
        //------------------------------------------------------------------------------------------------------------------------
        public void Add(GameObject gameObject) {
            if (collisionLoopActive)
                throw new Exception("Cannot call AddChild for gameobjects during OnCollision - use LateAddChild instead.");
            if (gameObject.collider != null && !colliderList.Contains(gameObject)) colliderList.Add(gameObject);

            var info = gameObject.GetType().GetMethod("OnCollision", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (info != null) {
                var onCollision = (CollisionDelegate) Delegate.CreateDelegate(typeof(CollisionDelegate), gameObject, info, false);
                if (onCollision != null && !_collisionReferences.ContainsKey(gameObject)) {
                    var colliderInfo = new ColliderInfo(gameObject, onCollision);
                    _collisionReferences[gameObject] = colliderInfo;
                    activeColliderList.Add(colliderInfo);
                }
            } else {
                validateCase(gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														validateCase()
        //------------------------------------------------------------------------------------------------------------------------
        private void validateCase(GameObject gameObject) {
            var info = gameObject.GetType().GetMethod("OnCollision",
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (info != null) throw new Exception("'OnCollision' function was not binded. Please check its case (capital O?)");
        }

        //------------------------------------------------------------------------------------------------------------------------
        //														Remove()
        //------------------------------------------------------------------------------------------------------------------------
        public void Remove(GameObject gameObject) {
            if (collisionLoopActive)
                throw new Exception("Cannot destroy or remove gameobjects during OnCollision - use LateDestroy or LateRemove instead.");
            colliderList.Remove(gameObject);
            if (_collisionReferences.ContainsKey(gameObject)) {
                var colliderInfo = _collisionReferences[gameObject];
                activeColliderList.Remove(colliderInfo);
                _collisionReferences.Remove(gameObject);
            }
        }

        public string GetDiagnostics() {
            var output = "";
            output += "Number of colliders: " + colliderList.Count + '\n';
            output += "Number of active colliders: " + activeColliderList.Count + '\n';
            return output;
        }

        #region Nested type: ColliderInfo
        //------------------------------------------------------------------------------------------------------------------------
        //														ColliderInfo
        //------------------------------------------------------------------------------------------------------------------------
        private struct ColliderInfo {
            public readonly GameObject gameObject;
            public readonly CollisionDelegate onCollision;

            //------------------------------------------------------------------------------------------------------------------------
            //														ColliderInfo()
            //------------------------------------------------------------------------------------------------------------------------
            public ColliderInfo(GameObject gameObject, CollisionDelegate onCollision) {
                this.gameObject = gameObject;
                this.onCollision = onCollision;
            }
        }
        #endregion

        #region Nested type: CollisionDelegate
        private delegate void CollisionDelegate(GameObject gameObject);
        #endregion
    }
}