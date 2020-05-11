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
    public class FelliniMenuScene : Scene {
        private const float startX = 73f;
        private const float endX = 1863f;
        private const float width = 1790f;

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

        public FelliniMenuScene() {
            SceneName = "Fellini-Menu";
        }

        public override void Load() {
            var transparentButtonStyle = ButtonStyle.Default.Alter(backgroundColorNormal: Color.Transparent, backgroundColorHover: Color.Transparent, backgroundColorPressed: Color.Transparent, borderColorNormal: Color.Transparent, borderColorHover: Color.Transparent, borderColorPressed: Color.Transparent, textColorNormal: Color.Transparent, textColorHover: Color.Transparent, textColorPressed: Color.Transparent);
            var textFieldStyle = TextFieldStyle.Default.Alter(backgroundNormal:Color.Transparent,backgroundFocused:Color.Transparent, borderNormal:Color.FromArgb(255,255,255,0), borderFocused:Color.FromArgb(255,255,255,0), borderSizeNormal: 4f, borderSizeFocused:4f);
            var checkboxStyle = CheckboxStyle.Default.Alter(tickColorNormal: Color.FromArgb(255,255,255,0),tickColorHover: Color.FromArgb(255,255,255,0),tickColorPressed: Color.FromArgb(255,255,255,0));

            backgroundImage1 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/fellini/menu_1.png", false, false)) {x = 0f};
            backgroundImage2 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/fellini/menu_2.png", false, false)) {x = -100000f};
            backgroundImage3 = new Image(0, 0, Globals.WIDTH, Globals.HEIGHT, new Sprite("data/sprites/locations/fellini/menu_3.png", false, false)) {x = -100000f};
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
            tab1Main.AddChild(new Button(148, 266, 434, 134, "Create public room", transparentButtonStyle, onClick: () => {
                tab1CreatePublic.x = 0f;
                backgroundImage2.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(148, 458, 434, 134, "Create private room", transparentButtonStyle, onClick: () => {
                tab1CreatePrivate.x = 0;
                backgroundImage3.x = 0f;
                tab1Main.x = -100000f;
                backgroundImage1.x = -100000f;
            }));
            tab1Main.AddChild(new Button(253, 900, 210, 95, "Back", transparentButtonStyle, () => { Debug.LogWarning("Main back button is not implemented yet!"); }));
            tab1CreatePublic.AddChild(publicRoomNameTextField = new TextField(99, 341-2, 540, 52, "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomDescriptionTextField = new TextField(99, 562-2, 540, 52, "", textFieldStyle));
            tab1CreatePublic.AddChild(publicRoomNSFWCheckbox = new Checkbox(99+64f, 670, 540-64f, 52, "", checkboxStyle));
            tab1CreatePublic.AddChild(new Button(180, 797, 372, 67, "Create", transparentButtonStyle, onClick: () => {
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
            tab1CreatePublic.AddChild(new Button(253, 900, 210, 95, "Back", transparentButtonStyle, onClick: () => {
                tab1CreatePublic.x = -100000f;
                backgroundImage2.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));
            tab1CreatePrivate.AddChild(privateRoomNameTextField = new TextField(99, 341-2, 540, 52, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomDescriptionTextField = new TextField(99, 473-2, 540, 52, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomCodeTextField = new TextField(100, 613-2, 540, 52, "", textFieldStyle));
            tab1CreatePrivate.AddChild(privateRoomNSFWCheckbox = new Checkbox(99+64f, 682, 540-64f, 52, "", checkboxStyle));
            tab1CreatePrivate.AddChild(new Button(180, 797, 372, 67, "Create", transparentButtonStyle, onClick: () => {
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
            tab1CreatePrivate.AddChild(new Button(253, 900, 210, 95, "Back", transparentButtonStyle, onClick: () => {
                tab1CreatePrivate.x = -100000f;
                backgroundImage3.x = -100000f;
                tab1Main.x = 0f;
                backgroundImage1.x = 0f;
            }));

            tab2.AddChild(loadingPrivateRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 207 + 64f - 48f, scale = 0.75f});
            tab2.AddChild(new Button(370f, 900, 360, 95, "Refresh", transparentButtonStyle, onClick: () => { RefreshRooms(); }));

            tab3.AddChild(loadingPublicRooms = new AnimatedSprite(Texture2D.GetInstance("data/sprites/spinner.png", true), 12, 1, 0.083F) {x = -100000f, y = 207 + 64f - 48f, scale = 0.75f});

            RefreshRooms();
            IsLoaded = true;
        }

        private void RefreshRooms() {
            NetworkManager.Instance.RequestRooms();
            loadingPrivateRooms.x = 966 - width/3f - 48f;
            loadingPublicRooms.x = 1531 - 2*width/3f - 48f;
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
                        roomUIElement.y = iPublic * 150 + 207;
                        tab3.AddChild(roomUIElement);
                        iPublic++;
                    } else {
                        var roomUIElement = UIFactory.CreateJoinPrivateRoomEntry(room);
                        roomUIElement.y = iPrivate * 200 + 207;
                        tab2.AddChild(roomUIElement);
                        iPrivate++;
                    }
                }
            }
        }
    }
}