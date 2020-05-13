using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Quobject.EngineIoClientDotNet.Modules;
using Button = game.ui.Button;
using Debug = GXPEngine.Debug;
using Image = game.ui.Image;

namespace game {
    public class CoffeeMenuScene : Scene {
        private const float startX = 70f;
        private const float endX = 1835f;
        private const float width = 1765f;

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
        private Image backgroundImage1;
        private Image backgroundImage2;
        private Image backgroundImage3;

        public CoffeeMenuScene() {
            SceneName = "Coffee Fellows-Menu";
        }

        public override void Load() {
            var textFieldStyle = TextFieldStyle.Default.Alter(backgroundNormal:Color.FromArgb(255,73,73,81),backgroundFocused:Color.FromArgb(255,73,73,81), borderNormal:Color.Transparent, borderFocused:Color.Transparent, borderSizeNormal: 4f, borderSizeFocused:4f, caretNormal:Color.White, caretFocused:Color.White);
            var checkboxStyle = CheckboxStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(255,73,73,81), backgroundColorHover: Color.FromArgb(255,73,73,81),backgroundColorPressed: Color.FromArgb(255,73,73,81),borderColorNormal: Color.Transparent, borderColorHover: Color.Transparent, borderColorPressed: Color.Transparent); 

            backgroundImage1 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/coffee_fellows/menu_1.jpg", false, false)) {x = 0f};
            backgroundImage2 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/coffee_fellows/menu_2.jpg", false, false)) {x = -100000f};
            backgroundImage3 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/coffee_fellows/menu_3.jpg", false, false)) {x = -100000f};
            Root.AddChild(backgroundImage1);
            Root.AddChild(backgroundImage2);
            Root.AddChild(backgroundImage3);
            var tab1 = new Pivot();
            tab2 = new Pivot();
            tab2.SetXY(width / 3f, 0f);
            tab3 = new Pivot();
            tab3.SetXY(2f * width / 3f, 0f);
            Root.AddChild(tab1);
            Root.AddChild(tab2);
            Root.AddChild(tab3);
            var tab1Main = new Pivot();
            var tab1CreatePublic = new Pivot {x = -100000f};
            var tab1CreatePrivate = new Pivot {x = -100000f};
            tab1.AddChild(tab1Main);
            tab1.AddChild(tab1CreatePublic);
            tab1.AddChild(tab1CreatePrivate);
            tab1Main.AddChild(new Button(135, 373, 448, 55, "Create public room", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePublic.x = 0f;
                backgroundImage2.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(111, 521, 494, 58, "Create private room", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePrivate.x = 0;
                backgroundImage3.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(253, 851, 168, 65, "Back", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.JoinLocation("none", false);
            }));
            tab1CreatePublic.AddChild(publicRoomNameTextField = new TextField(103, 284, 518, 40, "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomDescriptionTextField = new TextField(103, 422, 518, 40, "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomNSFWCheckbox = new Checkbox(103-16f, 507, 518+16f, 40, "", checkboxStyle));
            tab1CreatePublic.AddChild(new Button(200, 597, 293, 57, "Create", ButtonStyle.Transparent, onClick: () => {
                if (string.IsNullOrEmpty(publicRoomNameTextField.Text) || string.IsNullOrEmpty(publicRoomDescriptionTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(publicRoomNameTextField.Text, publicRoomDescriptionTextField.Text, "", publicRoomNSFWCheckbox.IsChecked, true);
                publicRoomNameTextField.Text = "";
                publicRoomDescriptionTextField.Text = "";
                publicRoomNSFWCheckbox.IsChecked = false;
                tab1CreatePublic.x = -100000f;
                backgroundImage2.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
                RefreshRooms();
            }));
            tab1CreatePublic.AddChild(new Button(253, 851, 168, 65, "Back", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePublic.x = -100000f;
                backgroundImage2.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));
            tab1CreatePrivate.AddChild(privateRoomNameTextField = new TextField(103, 284, 518, 40, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomDescriptionTextField = new TextField(103, 403, 518, 40, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomCodeTextField = new TextField(103, 528, 518, 40, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomNSFWCheckbox = new Checkbox(103-16f, 595, 518+16f, 40, "", checkboxStyle));
            tab1CreatePrivate.AddChild(new Button(200, 640, 293, 57, "Create", ButtonStyle.Transparent, onClick: () => {
                if (string.IsNullOrEmpty(privateRoomNameTextField.Text) || string.IsNullOrEmpty(privateRoomDescriptionTextField.Text) || string.IsNullOrEmpty(privateRoomCodeTextField.Text))
                    return;
                NetworkManager.Instance.CreateAndJoinRoom(privateRoomNameTextField.Text, privateRoomDescriptionTextField.Text, privateRoomCodeTextField.Text, privateRoomNSFWCheckbox.IsChecked, false);
                privateRoomNameTextField.Text = "";
                privateRoomDescriptionTextField.Text = "";
                privateRoomCodeTextField.Text = "";
                privateRoomNSFWCheckbox.IsChecked = false;
                tab1CreatePrivate.x = -100000f;
                backgroundImage3.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
                RefreshRooms();
            }));
            tab1CreatePrivate.AddChild(new Button(253, 851, 168, 65, "Back", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePrivate.x = -100000f;
                backgroundImage3.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));

            tab2.AddChild(loadingPrivateRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 250 + 64f - 48f, scale = 0.75f});
            tab2.AddChild(new Button(809f-width/3f, 858, 270, 65, "Refresh", ButtonStyle.Transparent, onClick: () => { RefreshRooms(); }));

            tab3.AddChild(loadingPublicRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 250 + 64f - 48f, scale = 0.75f});
            tab3.AddChild(new Button(1400- 2*width/3f, 858, 270, 65, "Refresh", ButtonStyle.Transparent, onClick: () => { RefreshRooms(); }));

            RefreshRooms();
            IsLoaded = true;
        }

        private void RefreshRooms() {
            NetworkManager.Instance.RequestRooms();
            loadingPrivateRooms.x = 948 - width/3f - 48f;
            loadingPublicRooms.x = 1540 - 2*width/3f - 48f;
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
                    if (room.Type != NetworkManager.Instance.PlayerData.Location)
                        continue;
                    if (room.IsPublic) {
                        var roomUIElement = UIFactory.CreateJoinPublicRoomEntry(room);
                        roomUIElement.y = iPublic * 150 + 250;
                        tab3.AddChild(roomUIElement);
                        iPublic++;
                    } else {
                        var roomUIElement = UIFactory.CreateJoinPrivateRoomEntry(room);
                        roomUIElement.y = iPrivate * 200 + 250;
                        tab2.AddChild(roomUIElement);
                        iPrivate++;
                    }
                }
            }
        }
    }
}