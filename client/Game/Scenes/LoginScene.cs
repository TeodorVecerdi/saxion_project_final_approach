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
        private bool consent;

        public LoginScene() {
            SceneID = "Login";
        }

        public override void Load() {
            username = "";
            avatarIndex = 0;
            consent = true;
            
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
            var avatarContainer = new Pivot {x = -100000f, y = 108};
            var avatarButton = new SpriteButton(0, 108, 128, 128, "Click\r\nto switch", new Sprite("data/sprites/avatars/female_1_128.png", true), avatarButtonStyle, () => {
                avatarContainer.x = Math.Abs(avatarContainer.x - 160f) < 0.0001f ? -100000f : 160f;
            });
            body.AddChild(avatarButton);
            body.AddChild(avatarContainer);
            var female1 = UIFactory.CreateAvatarSelectionEntry(128 * 0, 0, "data/sprites/avatars/female_1_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female1.OnClick += () => avatarIndex = 0;
            var female2 = UIFactory.CreateAvatarSelectionEntry(128 * 1 + 10, 0, "data/sprites/avatars/female_2_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female2.OnClick += () => avatarIndex = 1;
            var female3 = UIFactory.CreateAvatarSelectionEntry(128 * 2 + 10, 0, "data/sprites/avatars/female_3_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female3.OnClick += () => avatarIndex = 2;
            var female4 = UIFactory.CreateAvatarSelectionEntry(128 * 3 + 10, 0, "data/sprites/avatars/female_4_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female4.OnClick += () => avatarIndex = 3;
            var female5 = UIFactory.CreateAvatarSelectionEntry(128 * 4 + 10, 0, "data/sprites/avatars/female_5_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female5.OnClick += () => avatarIndex = 4;
            var male1 = UIFactory.CreateAvatarSelectionEntry(128 * 0, 138, "data/sprites/avatars/male_1_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male1.OnClick += () => avatarIndex = 5;
            var male2 = UIFactory.CreateAvatarSelectionEntry(128 * 1 + 10, 138, "data/sprites/avatars/male_2_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male2.OnClick += () => avatarIndex = 6;
            var male3 = UIFactory.CreateAvatarSelectionEntry(128 * 2 + 10, 138, "data/sprites/avatars/male_3_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male3.OnClick += () => avatarIndex = 7;
            var male4 = UIFactory.CreateAvatarSelectionEntry(128 * 3 + 10, 138, "data/sprites/avatars/male_4_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male4.OnClick += () => avatarIndex = 8;
            var male5 = UIFactory.CreateAvatarSelectionEntry(128 * 4 + 10, 138, "data/sprites/avatars/male_5_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male5.OnClick += () => avatarIndex = 9;
            avatarContainer.AddChild(female1);
            avatarContainer.AddChild(female2);
            avatarContainer.AddChild(female3);
            avatarContainer.AddChild(female4);
            avatarContainer.AddChild(female5);
            avatarContainer.AddChild(male1);
            avatarContainer.AddChild(male2);
            avatarContainer.AddChild(male3);
            avatarContainer.AddChild(male4);
            avatarContainer.AddChild(male5);
            
            body.AddChild(new Checkbox(0, 270+138, Globals.WIDTH - 40, 70, "Would you like to share anonymous\nusage statistics to help us\nimprove the user experience?", onValueChanged: (oldValue, newValue) => {
                consent = newValue;
            }) {IsChecked = true});
            body.AddChild(new Button(20, Globals.HEIGHT - 40 - 190 - 30, Globals.WIDTH - 80, 40, "Submit", onClick: () => {
                if (string.IsNullOrEmpty(username)) return;
                NetworkManager.Instance.Initialize(username, avatarIndex, consent);
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