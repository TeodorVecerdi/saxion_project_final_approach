using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Button = game.ui.Button;

namespace game {
    public class HomeScene : Scene {
        private TextField publicRoomNameTextField;
        private TextField publicRoomDescriptionTextField;
        private Checkbox publicRoomNSFWCheckbox;

        private TextField privateRoomNameTextField;
        private TextField privateRoomDescriptionTextField;
        private TextField privateRoomCodeTextField;
        private Checkbox privateRoomNSFWCheckbox;

        private GameObject tab2;
        private GameObject tab3;

        private AnimatedSprite loadingPrivateRooms;
        private AnimatedSprite loadingPublicRooms;

        public HomeScene() {
            SceneName = "Home";
        }
        
        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 16f, FontLoader.CenterCenterAlignment);
            var textFieldLabelStyle = new LabelStyle(Color.FromArgb(205, 205, 205), 14f, FontLoader.LeftCenterAlignment);

            var tab1 = new Pivot();
            tab2 = new Pivot();
            tab2.SetXY(Globals.WIDTH / 3f, 0f);
            tab3 = new Pivot();
            tab3.SetXY(2f * Globals.WIDTH / 3f, 0f);
            Root.AddChild(tab1);
            Root.AddChild(tab2);
            Root.AddChild(tab3);
            var tab1Main = new Pivot();
            var tab1CreatePublic = new Pivot {x = -100000f};
            var tab1CreatePrivate = new Pivot {x = -100000f};
            tab1.AddChild(tab1Main);
            tab1.AddChild(tab1CreatePublic);
            tab1.AddChild(tab1CreatePrivate);
            tab1Main.AddChild(new Label(0, 50, Globals.WIDTH / 3f, 50, "CREATE ROOM", primaryTitleStyle));
            tab1Main.AddChild(new Button(40, 100, Globals.WIDTH / 3f - 80, 40, "Public room", onClick: () => {
                tab1CreatePublic.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1Main.AddChild(new Button(40, 150, Globals.WIDTH / 3f - 80, 40, "Private room", onClick: () => {
                tab1CreatePrivate.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40, Globals.WIDTH / 3f - 80f, 40, "Create public room", primaryTitleStyle));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH / 3f - 80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePublic.AddChild(publicRoomNameTextField = new TextField(40, 24 + 40 + 60, Globals.WIDTH / 3f - 80, 40, ""));
            tab1CreatePublic.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH / 3f - 80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePublic.AddChild(publicRoomDescriptionTextField = new TextField(40, 108 + 40 + 60, Globals.WIDTH / 3f - 80, 40, ""));
            tab1CreatePublic.AddChild(publicRoomNSFWCheckbox = new Checkbox(40, 168 + 40 + 60, Globals.WIDTH / 3f - 80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePublic.AddChild(new Button(40, 168 + 40 + 60 + 40 + 20, Globals.WIDTH / 3f - 80, 40, "Create", onClick: () => {
                if (string.IsNullOrEmpty(publicRoomNameTextField.Text) || string.IsNullOrEmpty(publicRoomDescriptionTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(publicRoomNameTextField.Text, publicRoomDescriptionTextField.Text, "", publicRoomNSFWCheckbox.IsChecked, true);
                publicRoomNameTextField.Text = "";
                publicRoomDescriptionTextField.Text = "";
                publicRoomNSFWCheckbox.IsChecked = false;
                tab1CreatePublic.x = -100000f;
                tab1Main.x = 0f;
                RefreshRooms();
            }));
            tab1CreatePublic.AddChild(new Button(40, Globals.HEIGHT - 80, Globals.WIDTH / 3f - 80, 40, "Back", onClick: () => {
                tab1CreatePublic.x = -100000f;
                tab1Main.x = 0f;
            }));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40, Globals.WIDTH / 3f - 80f, 40, "Create private room", primaryTitleStyle));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH / 3f - 80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomNameTextField = new TextField(40, 24 + 40 + 60, Globals.WIDTH / 3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH / 3f - 80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomDescriptionTextField = new TextField(40, 108 + 40 + 60, Globals.WIDTH / 3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 168 + 40 + 60, Globals.WIDTH / 3f - 80, 24, "Room Code", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(privateRoomCodeTextField = new TextField(40, 192 + 40 + 60, Globals.WIDTH / 3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(privateRoomNSFWCheckbox = new Checkbox(40, 252 + 40 + 60, Globals.WIDTH / 3f - 80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePrivate.AddChild(new Button(40, 252 + 40 + 60 + 40 + 20, Globals.WIDTH / 3f - 80, 40, "Create", onClick: () => {
                if (string.IsNullOrEmpty(privateRoomNameTextField.Text) || string.IsNullOrEmpty(privateRoomDescriptionTextField.Text) || string.IsNullOrEmpty(privateRoomCodeTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(privateRoomNameTextField.Text, privateRoomDescriptionTextField.Text, privateRoomCodeTextField.Text, privateRoomNSFWCheckbox.IsChecked, false);
                privateRoomNameTextField.Text = "";
                privateRoomDescriptionTextField.Text = "";
                privateRoomCodeTextField.Text = "";
                privateRoomNSFWCheckbox.IsChecked = false;
                tab1CreatePrivate.x = -100000f;
                tab1Main.x = 0f;
                RefreshRooms();
            }));
            tab1CreatePrivate.AddChild(new Button(40, Globals.HEIGHT - 80, Globals.WIDTH / 3f - 80, 40, "Back", onClick: () => {
                tab1CreatePrivate.x = -100000f;
                tab1Main.x = 0f;
            }));

            tab2.AddChild(new Label(0, 50, Globals.WIDTH / 3f, 50, "JOIN PRIVATE ROOM", primaryTitleStyle));
            tab2.AddChild(loadingPrivateRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 150 - 48f, scale = 0.75f});
            tab2.AddChild(new Button(40, Globals.HEIGHT - 80f, 2 * Globals.WIDTH / 3f - 80, 40, "Refresh", onClick: () => {
                RefreshRooms();
            }));

            tab3.AddChild(new Label(0, 50, Globals.WIDTH / 3f, 50, "JOIN PUBLIC ROOM", primaryTitleStyle));
            tab3.AddChild(loadingPublicRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 150 - 48f, scale = 0.75f});

            RefreshRooms();
            IsLoaded = true;
        }

        private void RefreshRooms() {
            NetworkManager.Instance.RequestRooms();
            loadingPrivateRooms.x = Globals.WIDTH / 6f - 48f;
            loadingPublicRooms.x = Globals.WIDTH / 6f - 48f;
            tab2.GetChildren().ForEach(child => {
                if (child.name.StartsWith("ROOM:")) child.LateDestroy();
            });
                
            tab3.GetChildren().ForEach(child => {
                if (child.name.StartsWith("ROOM:")) child.LateDestroy();
            });
        }

        private void Update() {
            if (NetworkManager.Instance.RoomsReady) {
                NetworkManager.Instance.RoomsReady = false;
                loadingPrivateRooms.x = -100000f;
                loadingPublicRooms.x = -100000f;
                
                var iPrivate = 0;
                var iPublic = 0;
                foreach (var room in NetworkManager.Instance.Rooms) {
                    if(room.Type != NetworkManager.Instance.PlayerData.Location) 
                        continue;
                    if (room.IsPublic) {
                        var roomUIElement = UIFactory.CreateJoinPublicRoomEntry(room);
                        roomUIElement.y = iPublic * 150 + 100;
                        tab3.AddChild(roomUIElement);
                        iPublic++;
                    } else {
                        var roomUIElement = UIFactory.CreateJoinPrivateRoomEntry(room);
                        roomUIElement.y = iPrivate * 150 + 100;
                        tab2.AddChild(roomUIElement);
                        iPrivate++;
                    }
                }
            }
        }
    }
}