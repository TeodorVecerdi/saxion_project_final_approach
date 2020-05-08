using System;
using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Button = game.ui.Button;

namespace game {
    public class App : Game {
        public App() : base(Globals.WIDTH, Globals.HEIGHT, Globals.FULLSCREEN, Globals.VSYNC, pPixelArt: Globals.PIXEL_ART, windowTitle: Globals.WINDOW_TITLE) {
            targetFps = 60;
            SetupInput();

            var username = "";
            var avatar = 0;
            
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
            body.AddChild(new TextField(0, 24, 500, 40, "", new TextFieldStyle(placeholderTextSizeNormal: 12f, placeholderTextSizeFocused: 12f), onValueChanged: (oldValue, newValue) => username=newValue));
            body.AddChild(new Label(0, 84, 500, 24, "Avatar", textFieldLabelStyle));

            var avatarButtonStyle = new ButtonStyle(
                textSizeNormal: 14f, textSizeHover: 14f, textSizePressed: 14f,
                textNormal: Color.FromArgb(93, 55, 170), textHover: Color.FromArgb(106, 52, 178), textPressed: Color.FromArgb(111, 56, 189),
                fontLoaderInstance: FontLoader.SourceCodeBold);
            var avatarContainer = new Pivot {scaleX = 0f, x = 160, y = 108};
            var avatarButton = new SpriteButton(0, 108, 128, 128, "Click\r\nto switch", Texture2D.GetInstance("data/sprites/avatar_blue_sm.png", true), avatarButtonStyle, () => { avatarContainer.scaleX = Math.Abs(avatarContainer.scaleX) < 0.0001f ? 1f : 0f; });
            body.AddChild(avatarButton);

            body.AddChild(avatarContainer);
            {
                #region AVATAR_CONTAINER
                avatarContainer.AddChild(new SpriteButton(0, 0, 128, 128, "", Texture2D.GetInstance("data/sprites/avatar_blue_sm.png", true), avatarButtonStyle, () => {
                    avatarButton.Texture = Texture2D.GetInstance("data/sprites/avatar_blue_sm.png", true);
                    avatarButton.ShouldRepaint = true;
                    avatarContainer.scaleX = 0f;
                    avatar = 0;
                }));
                avatarContainer.AddChild(new SpriteButton(138, 0, 128, 128, "", Texture2D.GetInstance("data/sprites/avatar_green_sm.png", true), avatarButtonStyle, () => {
                    avatarButton.Texture = Texture2D.GetInstance("data/sprites/avatar_green_sm.png", true);
                    avatarButton.ShouldRepaint = true;
                    avatarContainer.scaleX = 0f;
                    avatar = 1;
                }));
                avatarContainer.AddChild(new SpriteButton(276, 0, 128, 128, "", Texture2D.GetInstance("data/sprites/avatar_red_sm.png", true), avatarButtonStyle, () => {
                    avatarButton.Texture = Texture2D.GetInstance("data/sprites/avatar_red_sm.png", true);
                    avatarButton.ShouldRepaint = true;
                    avatarContainer.scaleX = 0f;
                    avatar = 2;
                }));
                avatarContainer.AddChild(new SpriteButton(414, 0, 128, 128, "", Texture2D.GetInstance("data/sprites/avatar_yellow_sm.png", true), avatarButtonStyle, () => {
                    avatarButton.Texture = Texture2D.GetInstance("data/sprites/avatar_yellow_sm.png", true);
                    avatarButton.ShouldRepaint = true;
                    avatarContainer.scaleX = 0f;
                    avatar = 3;
                }));
                #endregion
            }
            body.AddChild(new Button(20, Globals.HEIGHT - 40 - 190 - 30, Globals.WIDTH - 80, 40, "Submit", onClick: () => {
                
            }));
            #endregion
            #endregion
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