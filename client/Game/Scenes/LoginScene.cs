using System;
using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Image = game.ui.Image;

namespace game {
    public class LoginScene : Scene {
        private string username;
        private int avatarIndex;
        private bool consent;

        public LoginScene() {
            SceneName = "Login";
        }

        public override void Load() {
            username = "";
            avatarIndex = 0;
            consent = true;
            
            var avatarButtonStyle = new ButtonStyle(
                textSizeNormal: 14f, textSizeHover: 14f, textSizePressed: 14f,
                textColorNormal: Color.Black, textColorHover: Color.Black, textColorPressed: Color.Black,
                fontLoaderInstance: FontLoader.SourceCodeBold);

            Root.AddChild(new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/login.jpg")));
            Root.AddChild(new TextField(670, 417, 500, 60, "", new TextFieldStyle(placeholderTextSizeNormal: 12f, placeholderTextSizeFocused: 12f), onValueChanged: (oldValue, newValue) => username = newValue));
            var avatarContainer = new Pivot {x = -100000f, y = 582};
            var avatarButton = new SpriteButton(856, 582, 128, 128, "Click\r\nto switch", new Sprite("data/sprites/avatars/female_1_128.png", true), avatarButtonStyle);
            avatarButton.OnClick += () => {
                avatarContainer.x = 230f;
                avatarButton.x = -100000f;
                MouseCursor.Instance.PreventMouseEventPropagation = true;
            };
            Root.AddChild(avatarButton);
            Root.AddChild(avatarContainer);
            var female1 = UIFactory.CreateAvatarSelectionEntry(128 * 0, 0, "data/sprites/avatars/female_1_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female1.OnClick += () => avatarIndex = 0;
            var female2 = UIFactory.CreateAvatarSelectionEntry(128 * 1 + 20, 0, "data/sprites/avatars/female_2_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female2.OnClick += () => avatarIndex = 1;
            var female3 = UIFactory.CreateAvatarSelectionEntry(128 * 2 + 20, 0, "data/sprites/avatars/female_3_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female3.OnClick += () => avatarIndex = 2;
            var female4 = UIFactory.CreateAvatarSelectionEntry(128 * 3 + 20, 0, "data/sprites/avatars/female_4_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female4.OnClick += () => avatarIndex = 3;
            var female5 = UIFactory.CreateAvatarSelectionEntry(128 * 4 + 20, 0, "data/sprites/avatars/female_5_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            female5.OnClick += () => avatarIndex = 4;
            var male1 = UIFactory.CreateAvatarSelectionEntry(128 * 5 + 20, 0, "data/sprites/avatars/male_1_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male1.OnClick += () => avatarIndex = 5;
            var male2 = UIFactory.CreateAvatarSelectionEntry(128 * 6 + 20, 0, "data/sprites/avatars/male_2_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male2.OnClick += () => avatarIndex = 6;
            var male3 = UIFactory.CreateAvatarSelectionEntry(128 * 7 + 20, 0, "data/sprites/avatars/male_3_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male3.OnClick += () => avatarIndex = 7;
            var male4 = UIFactory.CreateAvatarSelectionEntry(128 * 8 + 20, 0, "data/sprites/avatars/male_4_128.png", avatarButton, avatarContainer, avatarButtonStyle);
            male4.OnClick += () => avatarIndex = 8;
            var male5 = UIFactory.CreateAvatarSelectionEntry(128 * 9 + 20, 0, "data/sprites/avatars/male_5_128.png", avatarButton, avatarContainer, avatarButtonStyle);
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
            
            Root.AddChild(new Checkbox(475, 775, 815, 60, "", (oldValue, newValue) => {
                consent = newValue;
            }) {IsChecked = true});
            Root.AddChild(new Button(800, 912, 243, 77, "Submit", ButtonStyle.Transparent, onClick: () => {
                if (string.IsNullOrEmpty(username)) return;
                NetworkManager.Instance.CreateAccount(username, avatarIndex, consent);
                SceneManager.Instance.LoadScene("Map");
            }));
            IsLoaded = true;
        }
    }
}