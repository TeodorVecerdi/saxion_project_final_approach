using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using NUnit.Framework;
using Image = game.ui.Image;
using Utils = game.utils.Utils;

namespace game {
    public class FakeLoadingScene : Scene {
        private float timeLeft;
        private RectangleElement fakeOpacity;
        private Image loading;
        private Image logo;
        private const float moveTime = 2f;
        private const float stayTime = 1f;

        public FakeLoadingScene() {
            SceneName = "FakeLoading";
        }

        public override void Load() {
            timeLeft = moveTime + stayTime;
            Root.AddChild(loading = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/fakeLoading.png")));
            Root.AddChild(fakeOpacity = new RectangleElement(0, 0, Globals.WIDTH, Globals.HEIGHT, Color.Transparent, Color.Transparent, 0f));
            Root.AddChild(logo = new Image(542, 96, new Sprite("data/sprites/logo.png")));
            IsLoaded = true;
        }

        private void Update() {
            if (!IsLoaded) return;
            
            if (timeLeft > stayTime) {
                var opacity = Utils.Map(timeLeft - stayTime, moveTime, 0f, 0f, 1f);
                fakeOpacity.FillColor = Color.FromArgb((int) (opacity * 255), 255, 255, 255);
                fakeOpacity.ShouldRepaint = true;

                var logoPosition = Utils.Map(timeLeft - stayTime, moveTime, 0f, 96f, Globals.HEIGHT / 2f - 163f);
                logo.y = logoPosition;
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0) {
                SceneManager.Instance.LoadScene("Login");
            }
        }
    }
}