using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using GXPEngine.Core;
using Quobject.EngineIoClientDotNet.Modules;
using Button = game.ui.Button;
using Debug = game.utils.Debug;
using Image = game.ui.Image;

namespace game {
    public class RocksMenuScene : Scene {
        private const float startX = 74f;
        private const float endX = 1831f;
        private const float width = 1757f;

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

        public RocksMenuScene() {
            SceneName = "Rock's-Menu";
        }

        public override void Load() {
            var textFieldBackground = Color.FromArgb(255, 73, 65, 54);
            var textFieldText = Color.FromArgb(255, 227, 227, 222);
            var textFieldStyle = TextFieldStyle.Default.Alter(backgroundNormal:textFieldBackground,backgroundFocused:textFieldBackground, borderNormal:Color.Transparent, borderFocused:Color.Transparent, borderSizeNormal: 4f, borderSizeFocused:4f, caretNormal:textFieldText, caretFocused:textFieldText, textNormal:textFieldText, textFocused:textFieldText);
            var checkboxStyle = CheckboxStyle.Default.Alter(backgroundColorNormal: textFieldBackground, backgroundColorHover: textFieldBackground,backgroundColorPressed: textFieldBackground,borderColorNormal: Color.Transparent, borderColorHover: Color.Transparent, borderColorPressed: Color.Transparent, tickColorHover:textFieldText, tickColorNormal:textFieldText, tickColorPressed:textFieldText); 

            backgroundImage1 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/menu_1.jpg", false, false)) {x = 0f};
            backgroundImage2 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/menu_2.jpg", false, false)) {x = -100000f};
            backgroundImage3 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/rocks/menu_3.jpg", false, false)) {x = -100000f};
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
            tab1Main.AddChild(new Button(133, 358, 449, 83, "Create public room", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePublic.x = 0f;
                backgroundImage2.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(133, 509, 449, 83, "Create private room", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePrivate.x = 0;
                backgroundImage3.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(267, 836, 177, 83, "Back", ButtonStyle.Transparent, () => {
                NetworkManager.Instance.JoinLocation("none", false);
            }));
            tab1CreatePublic.AddChild(publicRoomNameTextField = new TextField(111, 315, 499, 60 , "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomDescriptionTextField = new TextField(111, 462, 499, 60, "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomNSFWCheckbox = new Checkbox(80, 617, 564, 40, "", checkboxStyle));
            tab1CreatePublic.AddChild(new Button(190, 679, 338, 83, "Create", ButtonStyle.Transparent, onClick: () => {
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
            tab1CreatePublic.AddChild(new Button(267, 836, 177, 83, "Back", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePublic.x = -100000f;
                backgroundImage2.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));
            tab1CreatePrivate.AddChild(privateRoomNameTextField = new TextField(111, 315, 499, 60, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomDescriptionTextField = new TextField(111, 442, 499, 60, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomCodeTextField = new TextField(111, 568, 499, 60, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomNSFWCheckbox = new Checkbox(85, 650, 560, 40, "", checkboxStyle));
            tab1CreatePrivate.AddChild(new Button(191, 709, 338, 83, "Create", ButtonStyle.Transparent, onClick: () => {
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
            tab1CreatePrivate.AddChild(new Button(267, 836, 177, 83, "Back", ButtonStyle.Transparent, onClick: () => {
                tab1CreatePrivate.x = -100000f;
                backgroundImage3.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));

            tab2.AddChild(loadingPrivateRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 232 + 64f - 48f, scale = 0.75f});
            tab2.AddChild(new Button(815f-width/3f, 838, 283, 83, "Refresh", ButtonStyle.Transparent, onClick: () => { RefreshRooms(); }));

            tab3.AddChild(loadingPublicRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 232 + 64f - 48f, scale = 0.75f});
            tab3.AddChild(new Button(1390 - 2*width/3f, 838, 283, 83, "Refresh", ButtonStyle.Transparent, onClick: () => { RefreshRooms(); }));

            RefreshRooms();
            IsLoaded = true;
        }

        private void RefreshRooms() {
            NetworkManager.Instance.RequestRooms();
            loadingPrivateRooms.x = 947 - width/3f - 48f;
            loadingPublicRooms.x = 1538 - 2*width/3f - 48f;
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
                        roomUIElement.y = iPublic * 150 + 232;
                        tab3.AddChild(roomUIElement);
                        iPublic++;
                    } else {
                        var roomUIElement = UIFactory.CreateJoinPrivateRoomEntry(room);
                        roomUIElement.y = iPrivate * 200 + 232;
                        tab2.AddChild(roomUIElement);
                        iPrivate++;
                    }
                }
            }
        }
    }
}