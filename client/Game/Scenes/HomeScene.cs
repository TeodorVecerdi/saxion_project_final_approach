using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;

namespace game {
    public class HomeScene : Scene {
        public HomeScene() {
            SceneID = "Home";
        }

        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 32f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment);
            var secondaryTitleStyleBold = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment, FontLoader.SourceCodeBold);
            var userData = NetworkManager.Instance.UserData;
            Root.AddChild(new Label(10, 10, Globals.WIDTH, 60, "Successfully logged in!", primaryTitleStyle) {ShouldRepaint = true});
            Root.AddChild(new Label(40, 100, Globals.WIDTH, 40, $"Username:", secondaryTitleStyle) {ShouldRepaint = true});
            Root.AddChild(new Label(370, 100, Globals.WIDTH, 40, $"{userData["username"]}", secondaryTitleStyleBold) {ShouldRepaint = true});
            Root.AddChild(new Label(40, 150, Globals.WIDTH, 40, $"Unique Identifier:", secondaryTitleStyle) {ShouldRepaint = true});
            Root.AddChild(new Label(370, 150, Globals.WIDTH, 40, $"{userData["guid"]}", secondaryTitleStyleBold) {ShouldRepaint = true});
            Root.AddChild(new Label(40, 200, Globals.WIDTH, 40, $"Room Id:", secondaryTitleStyle) {ShouldRepaint = true});
            Root.AddChild(new Label(370, 200, Globals.WIDTH, 40, $"{userData["room"]}", secondaryTitleStyleBold) {ShouldRepaint = true});
            Root.AddChild(new Label(40, 250, Globals.WIDTH, 40, $"Avatar:", secondaryTitleStyle) {ShouldRepaint = true});
            Sprite texture;
            var avatarIndex = userData.Value<int>("avatar");
            if (avatarIndex == 0) texture = new Sprite("data/sprites/avatar_blue.png");
            else if (avatarIndex == 1) texture = new Sprite("data/sprites/avatar_green.png");
            else if (avatarIndex == 2) texture = new Sprite("data/sprites/avatar_red.png");
            else texture = new Sprite("data/sprites/avatar_yellow.png");
            
            texture.SetScaleXY(0.5f, 0.5f);
            Root.AddChild(new SpriteButton(370, 250, 512, 512, "", texture));
            IsLoaded = true;
        }

        public override void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}