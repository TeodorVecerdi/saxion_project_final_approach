using GXPEngine;

namespace game {
    public abstract class Scene : GameObject {
        public string SceneID;
        public readonly Pivot Root = new Pivot();
        public bool IsLoaded = false;

        protected Scene() {
            name = "Scene";
            AddChild(Root);
        }
        public abstract void Load();
        public abstract void Unload();
    }
}