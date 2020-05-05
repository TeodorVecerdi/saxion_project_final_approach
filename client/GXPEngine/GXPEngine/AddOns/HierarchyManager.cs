using System;
using System.Collections.Generic;
using GXPEngine;

internal struct GameObjectPair {
    public GameObject parent;
    public GameObject child;
    public int index;

    public GameObjectPair(GameObject pParent, GameObject pChild, int pIndex = -1) {
        parent = pParent;
        child = pChild;
        index = pIndex;
    }
}

namespace GXPEngine {
    /// <summary>
    ///     If you are getting strange bugs because you are calling Destroy during the Update loop,
    ///     you can use this class to do this more cleanly: when using
    ///     HierarchyManager.Instance.LateDestroy,
    ///     all these hierarchy changes will be made after the update loop is finished.
    ///     Similarly, you can use HierarchyManager.Instance.LateCall to postpone a certain method call until
    ///     after the update loop.
    /// </summary>
    internal class HierarchyManager {
        private static HierarchyManager instance;

        private readonly List<GameObjectPair> toAdd;
        private readonly List<Action> toCall;
        private readonly List<GameObject> toDestroy;
        private readonly List<GameObject> toRemove;

        // Don't construct these yourself - get the one HierarchyManager using HierarchyManager.Instance
        private HierarchyManager() {
            Game.main.OnAfterStep += UpdateHierarchy;
            toAdd = new List<GameObjectPair>();
            toDestroy = new List<GameObject>();
            toRemove = new List<GameObject>();
            toCall = new List<Action>();
        }

        public static HierarchyManager Instance {
            get {
                if (instance == null) instance = new HierarchyManager();
                return instance;
            }
        }

        public void LateAdd(GameObject parent, GameObject child, int index = -1) {
            toAdd.Add(new GameObjectPair(parent, child, index));
        }

        public void LateDestroy(GameObject obj) {
            toDestroy.Add(obj);
        }

        public void LateRemove(GameObject obj) {
            toRemove.Add(obj);
        }

        public bool IsOnDestroyList(GameObject obj) {
            return toDestroy.Contains(obj);
        }

        public void LateCall(Action meth) {
            toCall.Add(meth);
        }

        public void UpdateHierarchy() {
            foreach (var pair in toAdd)
                if (pair.index >= 0)
                    pair.parent.AddChildAt(pair.child, pair.index);
                else
                    pair.parent.AddChild(pair.child);
            toAdd.Clear();

            foreach (var obj in toDestroy) obj.Destroy();
            toDestroy.Clear();

            foreach (var obj in toRemove) obj.Remove();
            toRemove.Clear();

            // This type of loop supports calling LateCall from within the loop:
            for (var i = 0; i < toCall.Count; i++) toCall[i]();
            toCall.Clear();
        }
    }
}