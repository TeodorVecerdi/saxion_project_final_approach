using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;
using Debug = GXPEngine.Debug;

namespace game {
    public class HomeScene : Scene {
        private TextField publicRoomNameTextField;
        private TextField publicRoomDescriptionTextField;
        private Checkbox publicRoomNSFWCheckbox;
        
        private TextField privateRoomNameTextField;
        private TextField privateRoomDescriptionTextField;
        private TextField privateRoomCodeTextField;
        private Checkbox privateRoomNSFWCheckbox;

        private TextField joinRoomCodeTextField;
        private GameObject tab3;
        
        public HomeScene() {
            SceneID = "Home";
        }

        /*
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
        */
        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 16f, FontLoader.CenterCenterAlignment);
            var textFieldLabelStyle = new LabelStyle(Color.FromArgb(205, 205, 205), 14f, FontLoader.LeftCenterAlignment);

            var tab1 = new Pivot();
            var tab2 = new Pivot();
            tab2.SetXY(Globals.WIDTH/3f, 0f);
            tab3 = new Pivot();
            tab3.SetXY(2f * Globals.WIDTH/3f, 0f);
            Root.AddChild(tab1);
            Root.AddChild(tab2);
            Root.AddChild(tab3);
            var tab1Main = new Pivot();
            var tab1CreatePublic = new Pivot() { x=-100000f};
            var tab1CreatePrivate = new Pivot() { x=-100000f};
            tab1.AddChild(tab1Main);
            tab1.AddChild(tab1CreatePublic);
            tab1.AddChild(tab1CreatePrivate);
            tab1Main.AddChild(new Label(0, 50, Globals.WIDTH/3f, 50, "CREATE ROOM", primaryTitleStyle));
            tab1Main.AddChild(new Button(40, 100, Globals.WIDTH/3f - 80, 40, "Public room", onClick: () => {
                tab1CreatePublic.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1Main.AddChild(new Button(40, 150, Globals.WIDTH/3f - 80, 40, "Private room", onClick: () => {
                tab1CreatePrivate.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40, Globals.WIDTH/3f - 80f, 40, "Create public room", primaryTitleStyle));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH/3f-80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePublic.AddChild(publicRoomNameTextField = new TextField(40, 24 + 40 + 60, Globals.WIDTH/3f-80, 40, ""));
            tab1CreatePublic.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH/3f-80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePublic.AddChild(publicRoomDescriptionTextField = new TextField(40, 108 + 40 + 60, Globals.WIDTH/3f-80, 40, ""));
            tab1CreatePublic.AddChild(publicRoomNSFWCheckbox = new Checkbox(40, 168 + 40 + 60, Globals.WIDTH/3f-80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePublic.AddChild(new Button(40, 168+40+60+40+20, Globals.WIDTH/3f - 80, 40, "Create", onClick: () => {
                if(string.IsNullOrEmpty(publicRoomNameTextField.Text) || string.IsNullOrEmpty(publicRoomDescriptionTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(publicRoomNameTextField.Text, publicRoomDescriptionTextField.Text, "0", "", publicRoomNSFWCheckbox.IsChecked, true);
            }));
            tab1CreatePublic.AddChild(new Button(40, Globals.HEIGHT-80, Globals.WIDTH/3f-80, 40, "Back", onClick: () => {
                tab1CreatePublic.x = -100000f;
                tab1Main.x = 0f;
            }));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40, Globals.WIDTH/3f - 80f, 40, "Create private room", primaryTitleStyle));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomNameTextField = new TextField(40, 24 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomDescriptionTextField = new TextField(40, 108 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 168 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Code", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomCodeTextField = new TextField(40, 192 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(privateRoomNSFWCheckbox = new Checkbox(40, 252 + 40 + 60, Globals.WIDTH/3f - 80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePrivate.AddChild(new Button(40, 252 + 40 + 60+40+20, Globals.WIDTH/3f - 80, 40, "Create", onClick: () => {
                if(string.IsNullOrEmpty(privateRoomNameTextField.Text) || string.IsNullOrEmpty(privateRoomDescriptionTextField.Text) || string.IsNullOrEmpty(privateRoomCodeTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(privateRoomNameTextField.Text, privateRoomDescriptionTextField.Text, "0", privateRoomCodeTextField.Text, privateRoomNSFWCheckbox.IsChecked, false);
            }));
            tab1CreatePrivate.AddChild(new Button(40, Globals.HEIGHT-80, Globals.WIDTH/3f - 80, 40, "Back", onClick: () => {
                tab1CreatePrivate.x = -100000f;
                tab1Main.x = 0f;
            }));
            
            tab2.AddChild(new Label(40, 50, Globals.WIDTH/3f - 80, 50, "JOIN PRIVATE ROOM", primaryTitleStyle));
            tab2.AddChild(new Label(40, 120, Globals.WIDTH/3f - 80, 50, "Enter room code:", secondaryTitleStyle));
            tab2.AddChild(joinRoomCodeTextField = new TextField(40, 170, Globals.WIDTH/3f - 80, 50, "code", TextFieldStyle.Default));
            tab2.AddChild(new Button(40, 170+50+20, Globals.WIDTH/3f - 80f, 40, "JOIN"));
            tab3.AddChild(new Label(0, 50, Globals.WIDTH/3f, 50, "JOIN PUBLIC ROOM", primaryTitleStyle));
            tab3.AddChild(new Button(40, Globals.HEIGHT-80f, Globals.WIDTH/3f-80, 40, "Refresh", onClick: () => {
                NetworkManager.Instance.RequestRooms();
            }));
            NetworkManager.Instance.RequestRooms();
            IsLoaded = true;
        }

        
        private void Update() {
            if (NetworkManager.Instance.RoomsReady) {
                NetworkManager.Instance.RoomsReady = false;
                tab3.GetChildren().ForEach(child => {
                    if(child.name.StartsWith("ROOM:")) child.LateDestroy();
                });
                var i = 0;
                foreach (var room in NetworkManager.Instance.Rooms) {
                    if(room.Value<bool>("pub") == false) continue;
                    var roomUIElement = UIFactory.CreateJoinRoomEntry(room);
                    roomUIElement.y = i*150+100;
                    tab3.AddChild(roomUIElement);
                    i++;
                }
            }
        }
        

        public override void Unload() {
            Root.GetChildren().ForEach(child => child.LateDestroy());
            IsLoaded = false;
        }
    }
}