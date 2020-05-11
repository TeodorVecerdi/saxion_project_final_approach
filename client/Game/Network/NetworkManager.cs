using System;
using System.Collections.Generic;
using game.ui.Custom;
using GXPEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace game {
    public class NetworkManager : GameObject {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ?? (instance = new NetworkManager());

        public NetworkPlayer PlayerData;
        private NetworkRoom activeRoom;

        private Socket socket;
        private bool initialized;
        private bool clientLoggedIn;
        private bool joinRoomSuccess;

        private bool gotNewMessage;
        private ChatMessage newestMessage;

        public bool RoomsReady;
        public List<NetworkRoom> Rooms;

        private NetworkManager() { }

        public void Initialize(string username, int avatarIndex, bool consent) {
            PlayerData = new NetworkPlayer(username, Guid.NewGuid().ToString(), "none", "none", avatarIndex, consent);

            // Remote URL: "https://saxion-0.ey.r.appspot.com"
            socket = IO.Socket("http://localhost:8080");
            socket.On("connect", data => {
                Debug.Log("Client connected.");
                if (!initialized)
                    clientLoggedIn = true;
                initialized = true;
            });
            SetupSocket();
        }

        public void CreateAndJoinRoom(string roomName, string roomDesc, string code, bool isNSFW, bool isPublic) {
            activeRoom = new NetworkRoom(roomName, roomDesc, Guid.NewGuid().ToString(), code, PlayerData.Location, isPublic, isNSFW);
            PlayerData.RoomID = activeRoom.GUID;
            socket.Emit("create_room", activeRoom.JSONString);
            SceneManager.Instance.LoadScene("Chat");
        }

        public void JoinRoom(string roomGuid, string roomCode) {
            var roomData = new JObject {["guid"] = roomGuid, ["code"] = roomCode};
            socket.Emit("join_room", roomData.ToString(Formatting.None));
        }

        public void RequestRooms() {
            socket.Emit("request_rooms");
        }

        public void JoinLocation(string location) {
            PlayerData.Location = location;
            socket.Emit("set_location", PlayerData.Location);
            SceneManager.Instance.LoadScene("Home");
        }

        public void SendMessage(ChatMessage message) {
            socket.Emit("send_message", message.JSONString);
        }

        public void ReceivedMessage(ChatMessage message) {
            ChatElement.ActiveChat.ReceiveMessage(message);
        }

        private void Update() {
            if (clientLoggedIn) {
                SceneManager.Instance.LoadScene("Map");
                clientLoggedIn = false;
            }

            if (gotNewMessage) {
                ReceivedMessage(newestMessage);
                gotNewMessage = false;
            }

            if (joinRoomSuccess) {
                SceneManager.Instance.LoadScene("Chat");
                joinRoomSuccess = false;
            }
        }

        private void SetupSocket() {
            socket.On("request_account", data => { socket.Emit("request_account_success", PlayerData.JSONString); });

            socket.On("disconnect", data => { Debug.Log($"Client disconnected. Reason: {data}"); });

            socket.On("request_rooms_success", data => {
                Rooms = new List<NetworkRoom>();
                var objData = (JObject) data;
                Debug.Log(objData);
                foreach (var prop in objData.Properties()) {
                    var roomData = (JObject) objData[prop.Name];
                    var room = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"), roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                    Rooms.Add(room);
                }

                RoomsReady = true;
            });
            socket.On("create_room_success", data => {
                Debug.LogWarning("Socket.IO response not implemented for 'create_room_success'");
                Debug.Log(data);
            });
            socket.On("join_room_failed", data => { Debug.LogWarning("Socket.IO response not implemented for 'join_room_failed'"); });
            socket.On("join_room_success", data => {
                var roomData = (JObject) data;
                activeRoom = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"), roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                joinRoomSuccess = true;
            });

            socket.On("client_joined", data => {
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` joined the room!");
                gotNewMessage = true;
            });
            socket.On("new_message", data => {
                newestMessage = ChatMessage.FromJSON(JObject.Parse(((JObject) data).Value<string>("message")));
                gotNewMessage = true;
            });
            socket.On("client_disconnected", data => {
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` left the room!");
                gotNewMessage = true;
                
            });
        }
    }
}