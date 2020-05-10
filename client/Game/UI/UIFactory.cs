using System.Drawing;
using game.utils;
using GXPEngine;
using Newtonsoft.Json.Linq;
using Debug = GXPEngine.Debug;

namespace game.ui {
    public static class UIFactory {
        private static ButtonStyle RoomButtonStyle = ButtonStyle.Default.Alter(backgroundColorNormal: Color.FromArgb(0, 255, 255, 255), backgroundColorHover: Color.FromArgb(0, 255, 255, 255), backgroundColorPressed: Color.FromArgb(0, 255, 255, 255), borderSizeHover: 0, borderSizeNormal: 0, borderSizePressed: 0);
        private static LabelStyle RoomTitleStyle = new LabelStyle(Color.White, 20f, textAlignmentNormal: FontLoader.LeftCenterAlignment, fontLoaderInstance: FontLoader.SourceCodeBold);
        private static LabelStyle RoomDescStyle = new LabelStyle(Color.White, textAlignmentNormal: FontLoader.LeftTopAlignment, textSizeNormal: 13f);

        public static GameObject CreateJoinPublicRoomEntry(NetworkRoom roomData) {
            return CreateJoinPublicRoomEntry(roomData.Name, roomData.Description, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPublicRoomEntry(string roomName, string roomDesc, bool isNSFW, string roomId) {
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, Globals.WIDTH / 3f - 80f, 20, roomName, RoomTitleStyle));
            roomContainer.AddChild(new Label(40, 30, Globals.WIDTH / 3f - 80f, 90f, roomDesc, RoomDescStyle));
            roomContainer.AddChild(new Button(40, 0, Globals.WIDTH / 3f - 80f, 130, "", RoomButtonStyle, onClick: () => {
                Debug.LogWarning($"Attempting to join room with id {roomId}. Not implemented yet");

                // NetworkManager.Instance.JoinRoom(roomId);
            }));
            if (isNSFW) {
                roomContainer.AddChild(new Image(Globals.WIDTH / 3f - 32 - 40, 0, 32, 32, new Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(Globals.WIDTH / 3f - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyle.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }

        public static GameObject CreateJoinPrivateRoomEntry(NetworkRoom roomData) {
            return CreateJoinPrivateRoomEntry(roomData.Name, roomData.Description, roomData.Code, roomData.IsNSFW, roomData.GUID);
        }

        public static GameObject CreateJoinPrivateRoomEntry(string roomName, string roomDesc, string roomCode, bool isNSFW, string roomId) {
            TextField textfield;
            var roomContainer = new Pivot();
            roomContainer.name = $"ROOM: {roomId}";
            roomContainer.AddChild(new Label(40, 0, Globals.WIDTH / 3f - 80f, 20, roomName, RoomTitleStyle));
            roomContainer.AddChild(new Label(40, 30, Globals.WIDTH / 3f - 80f, 90f, roomDesc, RoomDescStyle));
            roomContainer.AddChild(textfield = new TextField(40f, 130f, Globals.WIDTH / 3f - 80f, 40, "Enter room code"));
            roomContainer.AddChild(new Button(40, 0, Globals.WIDTH / 3f - 80f, 130, "", RoomButtonStyle, onClick: () => {
                if (textfield.Text != roomCode) return;
                Debug.LogWarning($"Attempting to join room with id {roomId}. Not implemented yet");

                // NetworkManager.Instance.JoinRoom(roomId);
            }));
            if (isNSFW) {
                roomContainer.AddChild(new Image(Globals.WIDTH / 3f - 32 - 40, 0, 32, 32, new Sprite("data/sprites/warning.png", false, false) {scale = 32f / 480f}));
                roomContainer.AddChild(new Label(Globals.WIDTH / 3f - 32 - 40 - 32 - 24, 0, 50, 32, "NSFW", RoomDescStyle.Alter(textAlignmentNormal: FontLoader.RightCenterAlignment)));
            }

            return roomContainer;
        }
    }
}