using System.Drawing;
using game.utils;
using GXPEngine;

namespace game.ui {
    public static class UIFactory {
        private static readonly ButtonStyle RoomButtonStyleFellini = ButtonStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(0, 255, 255, 255), backgroundColorHover: Color.FromArgb(0, 255, 255, 255), backgroundColorPressed: Color.FromArgb(0, 255, 255, 255), borderSizeHover: 4, borderSizeNormal: 4, borderSizePressed: 4, borderColorNormal:Color.Yellow, borderColorHover:Color.Yellow, borderColorPressed:Color.Yellow, textColorHover:Color.Yellow, textColorNormal:Color.Yellow, textColorPressed:Color.Yellow);
        private static LabelStyle RoomDescStyleFellini = new LabelStyle(Color.White, textAlignmentNormal: FontLoader.LeftTopAlignment, textSizeNormal: 13f);
        private static readonly LabelStyle RoomTitleStyleFellini = new LabelStyle(Color.Yellow, 20f, FontLoader.LeftCenterAlignment, FontLoader.SourceCodeBold);
        private static readonly TextFieldStyle RoomCodeStyleFellini = TextFieldStyle.Default.Alter(backgroundNormal: Color.Transparent, backgroundFocused: Color.Transparent, borderNormal: Color.FromArgb(255, 255, 255, 0), borderFocused: Color.FromArgb(255, 255, 255, 0), borderSizeNormal: 4f, borderSizeFocused: 4f);
        
        private static readonly ButtonStyle RoomButtonStyleCoffee = ButtonStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(255,73,73,81), backgroundColorHover:Color.FromArgb(255,73,73,81), backgroundColorPressed: Color.FromArgb(255,73,73,81), borderSizeHover: 4, borderSizeNormal: 4, borderSizePressed: 4, borderColorNormal:Color.White, borderColorHover:Color.White, borderColorPressed:Color.White, textColorHover:Color.White, textColorNormal:Color.White, textColorPressed:Color.White);
        private static LabelStyle RoomDescStyleCoffee = new LabelStyle(Color.White, textAlignmentNormal: FontLoader.LeftTopAlignment, textSizeNormal: 13f);
        private static readonly LabelStyle RoomTitleStyleCoffee = new LabelStyle(Color.White, 20f, FontLoader.LeftCenterAlignment, FontLoader.SourceCodeBold);
        private static readonly TextFieldStyle RoomCodeStyleCoffee = TextFieldStyle.Default.Alter(backgroundNormal:Color.FromArgb(255,73,73,81),backgroundFocused:Color.FromArgb(255,73,73,81), borderNormal:Color.Transparent, borderFocused:Color.Transparent, borderSizeNormal: 4f, borderSizeFocused:4f, caretNormal:Color.White, caretFocused:Color.White);
         
        private static readonly ButtonStyle RoomButtonStyleRocks = ButtonStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(255, 73, 65, 54), backgroundColorHover:Color.FromArgb(255, 73, 65, 54), backgroundColorPressed: Color.FromArgb(255, 73, 65, 54), borderSizeHover: 4, borderSizeNormal: 4, borderSizePressed: 4, borderColorNormal:Color.FromArgb(255, 227, 227, 222), borderColorHover:Color.FromArgb(255, 227, 227, 222), borderColorPressed:Color.FromArgb(255, 227, 227, 222), textColorHover:Color.FromArgb(255, 227, 227, 222), textColorNormal:Color.FromArgb(255, 227, 227, 222), textColorPressed:Color.FromArgb(255, 227, 227, 222));
        private static LabelStyle RoomDescStyleRocks = new LabelStyle(Color.FromArgb(255, 73, 65, 54), textAlignmentNormal: FontLoader.LeftTopAlignment, textSizeNormal: 13f);
        private static readonly LabelStyle RoomTitleStyleRocks = new LabelStyle(Color.FromArgb(255, 73, 65, 54), 20f, FontLoader.LeftCenterAlignment, FontLoader.SourceCodeBold);
        private static readonly TextFieldStyle RoomCodeStyleRocks = TextFieldStyle.Default.Alter(backgroundNormal:Color.FromArgb(255, 73, 65, 54),backgroundFocused:Color.FromArgb(255, 73, 65, 54), borderNormal:Color.Transparent, borderFocused:Color.Transparent, borderSizeNormal: 4f, borderSizeFocused:4f, caretNormal:Color.FromArgb(255, 227, 227, 222), caretFocused:Color.FromArgb(255, 227, 227, 222), textNormal:Color.FromArgb(255, 227, 227, 222), textFocused:Color.FromArgb(255, 227, 227, 222));
        
        public static GameObject CreateJoinPublicRoomEntryFellini(NetworkRoom roomData) {
            return CreateJoinPublicRoomEntryFellini(roomData.Name, roomData.Description, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPublicRoomEntryFellini(string roomName, string roomDesc, bool isNSFW, string roomId) {
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleFellini));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleFellini));
            roomContainer.AddChild(new Button(40, 110, 500f- 80f, 40f, "JOIN", RoomButtonStyleFellini, () => NetworkManager.Instance.TryJoinRoom(roomId, "")));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleFellini.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }

        public static GameObject CreateJoinPrivateRoomEntryFellini(NetworkRoom roomData) {
            return CreateJoinPrivateRoomEntryFellini(roomData.Name, roomData.Description, roomData.Code, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPrivateRoomEntryFellini(string roomName, string roomDesc, string roomCode, bool isNSFW, string roomId) {
            TextField textfield;
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleFellini));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleFellini));
            roomContainer.AddChild(textfield = new TextField(40, 110f, 500f - 80f, 40f, "Room code", RoomCodeStyleFellini));
            roomContainer.AddChild(new Button(40, 160, 500f- 80f, 40f, "JOIN", RoomButtonStyleFellini, () => {
                if (textfield.Text != roomCode) {
                    textfield.Text = "";
                    return;
                }
                NetworkManager.Instance.TryJoinRoom(roomId, roomCode);
            }));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleFellini.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }
        
        public static GameObject CreateJoinPublicRoomEntryCoffee(NetworkRoom roomData) {
            return CreateJoinPublicRoomEntryCoffee(roomData.Name, roomData.Description, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPublicRoomEntryCoffee(string roomName, string roomDesc, bool isNSFW, string roomId) {
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleCoffee));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleCoffee));
            roomContainer.AddChild(new Button(40, 110, 500f- 80f, 40f, "JOIN", RoomButtonStyleCoffee, () => NetworkManager.Instance.TryJoinRoom(roomId, "")));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleCoffee.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }

        public static GameObject CreateJoinPrivateRoomEntryCoffee(NetworkRoom roomData) {
            return CreateJoinPrivateRoomEntryCoffee(roomData.Name, roomData.Description, roomData.Code, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPrivateRoomEntryCoffee(string roomName, string roomDesc, string roomCode, bool isNSFW, string roomId) {
            TextField textfield;
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleCoffee));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleCoffee));
            roomContainer.AddChild(textfield = new TextField(40, 110f, 500f - 80f, 40f, "Room code", RoomCodeStyleCoffee));
            roomContainer.AddChild(new Button(40, 160, 500f- 80f, 40f, "JOIN", RoomButtonStyleCoffee, () => {
                if (textfield.Text != roomCode) {
                    textfield.Text = "";
                    return;
                }
                NetworkManager.Instance.TryJoinRoom(roomId, roomCode);
            }));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleCoffee.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }
        
        
        public static GameObject CreateJoinPublicRoomEntryRocks(NetworkRoom roomData) {
            return CreateJoinPublicRoomEntryRocks(roomData.Name, roomData.Description, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPublicRoomEntryRocks(string roomName, string roomDesc, bool isNSFW, string roomId) {
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleRocks));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleRocks));
            roomContainer.AddChild(new Button(40, 110, 500f- 80f, 40f, "JOIN", RoomButtonStyleRocks, () => NetworkManager.Instance.TryJoinRoom(roomId, "")));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleRocks.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }

        public static GameObject CreateJoinPrivateRoomEntryRocks(NetworkRoom roomData) {
            return CreateJoinPrivateRoomEntryRocks(roomData.Name, roomData.Description, roomData.Code, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPrivateRoomEntryRocks(string roomName, string roomDesc, string roomCode, bool isNSFW, string roomId) {
            TextField textfield;
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, 500f-80f, 20, roomName, RoomTitleStyleRocks));
            roomContainer.AddChild(new Label(40, 30, 500f - 80f, 70f, roomDesc, RoomDescStyleRocks));
            roomContainer.AddChild(textfield = new TextField(40, 110f, 500f - 80f, 40f, "Room code", RoomCodeStyleRocks));
            roomContainer.AddChild(new Button(40, 160, 500f- 80f, 40f, "JOIN", RoomButtonStyleRocks, () => {
                if (textfield.Text != roomCode) {
                    textfield.Text = "";
                    return;
                }
                NetworkManager.Instance.TryJoinRoom(roomId, roomCode);
            }));
            if (isNSFW) {
                roomContainer.AddChild(new Image(500 - 32 - 40, 0, 32, 32, new game.Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(500 - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyleRocks.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }

        public static SpriteButton CreateAvatarSelectionEntry(float x, float y, string fileName, SpriteButton avatarButton, Pivot avatarContainer, ButtonStyle avatarButtonStyle) {
            return new SpriteButton(x, y, 128, 128, "", new game.Sprite(fileName, true), avatarButtonStyle, () => {
                avatarButton.Sprite = new game.Sprite(fileName, true);
                avatarButton.ShouldRepaint = true;
                avatarButton.x = 856f;
                avatarContainer.x = -100000f;
            });
        }
    }
}