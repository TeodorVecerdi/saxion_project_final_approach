using System.Collections.Generic;
using GXPEngine;

namespace game {
    public class SceneManager : GameObject {
        private static SceneManager instance;
        public static SceneManager Instance => instance ?? (instance = new SceneManager());

        private Dictionary<string, Scene> scenes;
        private string activeSceneID;

        private SceneManager() {
            scenes = new Dictionary<string, Scene>();
            activeSceneID = "none";
            SetupScenes();
        }

        private void SetupScenes() {
            AddScene(new LoginScene());
            AddScene(new LoadingScene());
            AddScene(new MenuScene());
            AddScene(new MapScene());
            AddScene(new TestChatScene());
            AddScene(new Test());
        }

        private void AddScene(Scene scene) {
            scenes.Add(scene.SceneName, scene);
        }

        public void LoadScene(string sceneID) {
            if (scenes.ContainsKey(activeSceneID)) {
                Debug.LogInfo($"Unloaded scene `{activeSceneID}`.");
                scenes[activeSceneID].Unload();
                RemoveChild(scenes[activeSceneID]);
            } else {
                Debug.LogWarning($"Could not unload scene `{activeSceneID}`. Reason: Scene not found in Scene dictionary.");
            }

            activeSceneID = sceneID;
            if (scenes.ContainsKey(activeSceneID)) {
                Debug.LogInfo($"Loaded scene `{activeSceneID}`.");
                scenes[activeSceneID].Load();
                AddChild(scenes[activeSceneID]);
            } else {
                Debug.LogWarning($"Could not load scene `{activeSceneID}`. Reason: Scene not found in Scene dictionary.");
            }
        }
    }
}