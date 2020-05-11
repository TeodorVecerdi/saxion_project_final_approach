using System;
using System.Collections.Generic;
using GXPEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace game {
    public class NetworkManager : GameObject {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ?? (instance = new NetworkManager());

        public NetworkPlayer playerData;
        private NetworkRoom activeRoom;

        private Socket socket;
        private bool initialized;
        private bool shouldSwitchScene;

        public bool RoomsReady = false;
        public List<NetworkRoom> Rooms;
        
        private NetworkManager() {}

        public void Initialize(string username, int avatarIndex, bool consent) {
            playerData = new NetworkPlayer(username, Guid.NewGuid().ToString(), "none", "none", avatarIndex, consent);
            // Remote URL: "https://saxion-0.ey.r.appspot.com"
            socket = IO.Socket("http://localhost:8080");
            socket.On("connect", data => {
                Debug.Log("Client connected.");
                if (!initialized) 
                    shouldSwitchScene = true;
                initialized = true;
            });
            SetupSocket();
        }

        public void CreateAndJoinRoom(string roomName, string roomDesc, string code, bool isNSFW, bool isPublic) {
            activeRoom = new NetworkRoom(roomName, roomDesc, Guid.NewGuid().ToString(), code, playerData.Location, isPublic, isNSFW);
            playerData.RoomID = activeRoom.GUID;
            socket.Emit("create_room", activeRoom.JSONString);
        }

        public void RequestRooms() {
            socket.Emit("request_rooms", "");
        }        
        
        public void JoinLocation(string location) {
            playerData.Location = location;
            socket.Emit("set_location", playerData.Location);
            SceneManager.Instance.LoadScene("Home");
        }


        private void Update() {
            if (shouldSwitchScene) {
                SceneManager.Instance.LoadScene("Map");
                shouldSwitchScene = false;
            }
        }

        private void SetupSocket() {
            socket.On("request_account", data => {
                socket.Emit("request_account_success", playerData.JSONString);
            });

            socket.On("disconnect", data => {
                Debug.Log($"Client disconnected. Reason: {data}");
            });

            socket.On("request_rooms_success", data => {
                RoomsReady = true;
                Rooms = new List<NetworkRoom>();
                var objData = (JObject) data;
                Debug.Log(objData);
                foreach (var prop in objData.Properties()) {
                    var roomData = (JObject)objData[prop.Name];
                    var room = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"),roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                    Rooms.Add(room);
                }
            });
            socket.On("create_room_success", data => {
                Debug.LogWarning("Socket.IO response not implemented for 'create_room_success'"); 
                Debug.Log(data);
            });
            socket.On("join_room_failed", data => { Debug.LogWarning("Socket.IO response not implemented for 'join_room_failed'"); });
            socket.On("join_room_success", data => { Debug.LogWarning("Socket.IO response not implemented for 'join_room_success'"); });

            socket.On("client_joined", data => { Debug.LogWarning("Socket.IO response not implemented for 'client_joined'"); });
            socket.On("new_message", data => { Debug.LogWarning("Socket.IO response not implemented for 'new_message'"); });
            socket.On("client_disconnected", data => { Debug.LogWarning("Socket.IO response not implemented for 'client_disconnected'"); });
        }
    }
}