using System;
using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;

namespace game {
    public class LoginScene : Scene {
        private string username;
        private int avatarIndex;

        public LoginScene() {
            username = "";
            avatarIndex = 0;
            SceneID = "Login";
        }

        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle2 = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment);
            var textFieldLabelStyle = new LabelStyle(Color.FromArgb(205, 205, 205), 14f, FontLoader.LeftCenterAlignment);
            var avatarButtonStyle = new ButtonStyle(
                textSizeNormal: 14f, textSizeHover: 14f, textSizePressed: 14f,
                textColorNormal: Color.FromArgb(93, 55, 170), textColorHover: Color.FromArgb(106, 52, 178), textColorPressed: Color.FromArgb(111, 56, 189),
                fontLoaderInstance: FontLoader.SourceCodeBold);

            var header = new Pivot();
            Root.AddChild(header);
            header.AddChild(new Label(0, 10, Globals.WIDTH, 40,
                "Welcome!", primaryTitleStyle));
            header.AddChild(new Label(0, 55, Globals.WIDTH, 28,
                "We can't bring you to the bar", secondaryTitleStyle));
            header.AddChild(new Label(0, 83, Globals.WIDTH, 28,
                "So we brought the bar to YOU!", secondaryTitleStyle));
            header.AddChild(new Label(20, 140, Globals.WIDTH, 28,
                "So start by creating an account:", secondaryTitleStyle2));
            var body = new Pivot();
            body.SetXY(30, 190);
            Root.AddChild(body);
            body.AddChild(new Label(0, 0, 500, 24, "Username", textFieldLabelStyle));
            body.AddChild(new TextField(0, 24, 500, 40, "", new TextFieldStyle(placeholderTextSizeNormal: 12f, placeholderTextSizeFocused: 12f), onValueChanged: (oldValue, newValue) => username = newValue));
            body.AddChild(new Label(0, 84, 500, 24, "Avatar", textFieldLabelStyle));
            var avatarContainer = new Pivot {scaleX = 0f, x = 160, y = 108};
            var avatarButton = new SpriteButton(0, 108, 128, 128, "Click\r\nto switch", new Sprite("data/sprites/avatar_blue_sm.png", true), avatarButtonStyle, () => { avatarContainer.scaleX = Math.Abs(avatarContainer.scaleX) < 0.0001f ? 1f : 0f; });
            body.AddChild(avatarButton);
            body.AddChild(avatarContainer);
            avatarContainer.AddChild(new SpriteButton(0, 0, 128, 128, "", new Sprite("data/sprites/avatar_blue_sm.png", true), avatarButtonStyle, () => {
                avatarButton.Sprite = new Sprite("data/sprites/avatar_blue_sm.png", true);
                avatarButton.ShouldRepaint = true;
                avatarContainer.scaleX = 0f;
                avatarIndex = 0;
            }));
            avatarContainer.AddChild(new SpriteButton(138, 0, 128, 128, "", new Sprite("data/sprites/avatar_green_sm.png", true), avatarButtonStyle, () => {
                avatarButton.Sprite = new Sprite("data/sprites/avatar_green_sm.png", true);
                avatarButton.ShouldRepaint = true;
                avatarContainer.scaleX = 0f;
                avatarIndex = 1;
            }));
            avatarContainer.AddChild(new SpriteButton(276, 0, 128, 128, "", new Sprite("data/sprites/avatar_red_sm.png", true), avatarButtonStyle, () => {
                avatarButton.Sprite = new Sprite("data/sprites/avatar_red_sm.png", true);
                avatarButton.ShouldRepaint = true;
                avatarContainer.scaleX = 0f;
                avatarIndex = 2;
            }));
            avatarContainer.AddChild(new SpriteButton(414, 0, 128, 128, "", new Sprite("data/sprites/avatar_yellow_sm.png", true), avatarButtonStyle, () => {
                avatarButton.Sprite = new Sprite("data/sprites/avatar_yellow_sm.png", true);
                avatarButton.ShouldRepaint = true;
                avatarContainer.scaleX = 0f;
                avatarIndex = 3;
            }));
            body.AddChild(new Button(20, Globals.HEIGHT - 40 - 190 - 30, Globals.WIDTH - 80, 40, "Submit", onClick: () => {
                if (string.IsNullOrEmpty(username)) return;
                NetworkManager.Instance.Initialize(username, avatarIndex);
                SceneManager.Instance.LoadScene("Loading");
            }));
            IsLoaded = true;
        }

        public override void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}