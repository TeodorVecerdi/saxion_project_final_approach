using System;
using System.Drawing;
using System.Threading.Tasks;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Button = game.ui.Button;
using Debug = GXPEngine.Debug;

namespace game {
    public class App : Game {
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            SetupInput();

            var username = "";
            var avatarIndex = 0;

            var mainUI = new Pivot();
            AddChild(mainUI);

            #region MAIN_UI
            var header = new Pivot();
            mainUI.AddChild(header);

            #region HEADER
            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle2 = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment);
            header.AddChild(new Label(0, 10, Globals.WIDTH, 40,
                "Welcome!", primaryTitleStyle));
            header.AddChild(new Label(0, 55, Globals.WIDTH, 28,
                "We can't bring you to the bar", secondaryTitleStyle));
            header.AddChild(new Label(0, 83, Globals.WIDTH, 28,
                "So we brought the bar to YOU!", secondaryTitleStyle));
            header.AddChild(new Label(20, 140, Globals.WIDTH, 28,
                "So start by creating an account:", secondaryTitleStyle2));
            #endregion

            var body = new Pivot();
            body.SetXY(30, 190);
            mainUI.AddChild(body);

            #region BODY
            var textFieldLabelStyle = new LabelStyle(Color.FromArgb(205, 205, 205), 14f, FontLoader.LeftCenterAlignment);
            body.AddChild(new Label(0, 0, 500, 24, "Username", textFieldLabelStyle));
            body.AddChild(new TextField(0, 24, 500, 40, "", new TextFieldStyle(placeholderTextSizeNormal: 12f, placeholderTextSizeFocused: 12f), onValueChanged: (oldValue, newValue) => username = newValue));
            body.AddChild(new Label(0, 84, 500, 24, "Avatar", textFieldLabelStyle));

            var avatarButtonStyle = new ButtonStyle(
                textSizeNormal: 14f, textSizeHover: 14f, textSizePressed: 14f,
                textNormal: Color.FromArgb(93, 55, 170), textHover: Color.FromArgb(106, 52, 178), textPressed: Color.FromArgb(111, 56, 189),
                fontLoaderInstance: FontLoader.SourceCodeBold);
            var avatarContainer = new Pivot {scaleX = 0f, x = 160, y = 108};
            var avatarButton = new SpriteButton(0, 108, 128, 128, "Click\r\nto switch", new Sprite("data/sprites/avatar_blue_sm.png", true), avatarButtonStyle, () => { avatarContainer.scaleX = Math.Abs(avatarContainer.scaleX) < 0.0001f ? 1f : 0f; });
            body.AddChild(avatarButton);

            body.AddChild(avatarContainer);
            {
                #region AVATAR_CONTAINER
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
                #endregion
            }
            body.AddChild(new Button(20, Globals.HEIGHT - 40 - 190 - 30, Globals.WIDTH - 80, 40, "Submit", onClick: () => {
                if (string.IsNullOrEmpty(username)) return;
                NetworkManager.Instance.Initialize(username, avatarIndex);
                mainUI.LateDestroy();
                CreateTempLoggedInUI();
                // ShowLoadingScreen();
            }));
            #endregion
            #endregion
        }

        private void CreateTempLoggedInUI() {
            var temp = new Pivot();
            LateAddChild(temp);

            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment);
            var secondaryTitleStyleBold = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment, FontLoader.SourceCodeBold);
            temp.LateAddChild(new Label(10, 10, Globals.WIDTH, 60, "Successfully logged in!", primaryTitleStyle) {ShouldRepaint = true});
            temp.LateAddChild(new Label(40, 100, Globals.WIDTH, 40, $"Username:", secondaryTitleStyle) {ShouldRepaint = true});
            temp.LateAddChild(new Label(370, 100, Globals.WIDTH, 40, $"{NetworkManager.Instance.Username}", secondaryTitleStyleBold) {ShouldRepaint = true});
            temp.LateAddChild(new Label(40, 150, Globals.WIDTH, 40, $"Unique Identifier:", secondaryTitleStyle) {ShouldRepaint = true});
            temp.LateAddChild(new Label(370, 150, Globals.WIDTH, 40, $"{NetworkManager.Instance.GUID}", secondaryTitleStyleBold) {ShouldRepaint = true});
            temp.LateAddChild(new Label(40, 200, Globals.WIDTH, 40, $"Room Id:", secondaryTitleStyle) {ShouldRepaint = true});
            temp.LateAddChild(new Label(370, 200, Globals.WIDTH, 40, $"{NetworkManager.Instance.RoomID}", secondaryTitleStyleBold) {ShouldRepaint = true});
            temp.LateAddChild(new Label(40, 250, Globals.WIDTH, 40, $"Avatar:", secondaryTitleStyle) {ShouldRepaint = true});
            Sprite texture;
            if (NetworkManager.Instance.AvatarIndex == 0) texture = new Sprite("data/sprites/avatar_blue.png");
            else if (NetworkManager.Instance.AvatarIndex == 1) texture = new Sprite("data/sprites/avatar_green.png");
            else if (NetworkManager.Instance.AvatarIndex == 2) texture = new Sprite("data/sprites/avatar_red.png");
            else texture = new Sprite("data/sprites/avatar_yellow.png");
            texture.SetScaleXY(0.5f, 0.5f);
            temp.LateAddChild(new SpriteButton(370, 250, 512, 512, "", texture));
        }

        private void SetupInput() {
            // Input.AddAxis("Horizontal", new List<int>{Key.LEFT, Key.A}, new List<int>{Key.RIGHT, Key.D});
            // Input.AddAxis("Vertical", new List<int>{Key.DOWN, Key.S}, new List<int>{Key.UP, Key.W}, true);
        }

        public static void Main(string[] args) {
            new App().Start();
        }
    }
}