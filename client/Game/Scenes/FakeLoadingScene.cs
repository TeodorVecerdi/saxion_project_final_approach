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
        private Image logo;
        private Label extra;
        private Label extra2;
        private bool showedExtraText;
        private const float moveTime = 2f;
        private const float stayTime = 1f;

        public FakeLoadingScene() {
            SceneName = "FakeLoading";
        }

        public override void Load() {
            timeLeft = moveTime + stayTime;
            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/fakeLoading.png")));
            Root.AddChild(fakeOpacity = new RectangleElement(0, 0, Globals.WIDTH, Globals.HEIGHT, Color.Transparent, Color.Transparent, 0f));
            Root.AddChild(logo = new Image(542, 96, new Sprite("data/sprites/logo.png")));
            Root.AddChild(extra = new Label(0, Globals.HEIGHT/2f + 200, Globals.WIDTH, 100, "Loading extra assets. Please wait.", LabelStyle.Default.Alter(textAlignmentNormal:FontLoader.CenterCenterAlignment, textSizeNormal:48f, fontLoaderInstance:FontLoader.SourceCodeBold)) {x = -100000f});
            Root.AddChild(extra2 = new Label(0, Globals.HEIGHT/2f + 250, Globals.WIDTH, 100, "It should take less than a minute.", LabelStyle.Default.Alter(textAlignmentNormal:FontLoader.CenterCenterAlignment, textSizeNormal:48f, fontLoaderInstance:FontLoader.SourceCodeBold)) {x = -100000f});
            IsLoaded = true;
        }

        private void Update() {
            if (!IsLoaded) return;
            
            if (timeLeft >= stayTime) {
                var opacity = Utils.Map(timeLeft - stayTime, moveTime, 0f, 0f, 1f);
                fakeOpacity.FillColor = Color.FromArgb((int) (opacity * 255), 255, 255, 255);
                fakeOpacity.ShouldRepaint = true;

                var logoPosition = Utils.Map(timeLeft - stayTime, moveTime, 0f, 96f, Globals.HEIGHT / 2f - 163f);
                logo.y = logoPosition;
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0 && (!SoundManager.Instance.IsLoadingDone || !SceneManager.Instance.IsLoadingDone) && !showedExtraText) {
                extra.x = 0f;
                extra2.x = 0f;
                fakeOpacity.FillColor = Color.FromArgb(255, 255, 255, 255);
                fakeOpacity.ShouldRepaint = true;
                showedExtraText = true;
            }
            if (timeLeft <= 0 && SoundManager.Instance.IsLoadingDone && SceneManager.Instance.IsLoadingDone) {
                SceneManager.Instance.LoadScene("Login");
            }
        }
    }
}