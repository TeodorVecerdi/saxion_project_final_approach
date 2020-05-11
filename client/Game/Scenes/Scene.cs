using GXPEngine;

namespace game {
    public abstract class Scene : GameObject {
        public string SceneName;
        public readonly Pivot Root = new Pivot();
        public bool IsLoaded;

        protected Scene() {
            name = "Scene";
            AddChild(Root);
        }

        public abstract void Load();

        public virtual void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}