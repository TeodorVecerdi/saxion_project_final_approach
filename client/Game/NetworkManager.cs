using System;
using System.Threading;
using System.Threading.Tasks;
using GXPEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace game {
    public class NetworkManager {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ?? (instance = new NetworkManager());

        public string Username;
        public string GUID;
        public string RoomID;
        public int AvatarIndex;

        private Socket socket;

        public void Initialize(string username, int avatarIndex) {
            Username = username;
            AvatarIndex = avatarIndex;
            GUID = Guid.NewGuid().ToString();
            RoomID = "none";

            // Remote URL: "https://saxion-0.ey.r.appspot.com"
            socket = IO.Socket("http://localhost:8080");
            socket.On("connect", data => { Debug.Log("Client connected."); });
            SetupSocket();
        }

        private void SetupSocket() {
            socket.On("request_account", data => { socket.Emit("request_account_success", UserData.ToString(Formatting.None)); });

            socket.On("disconnect", data => { Debug.Log($"Client disconnected. Reason: {data}"); });

            socket.On("request_rooms_success", data => { Debug.LogWarning("Socket.IO response not implemented for 'request_rooms_success'"); });
            socket.On("create_room_success", data => { Debug.LogWarning("Socket.IO response not implemented for 'create_room_success'"); });
            socket.On("join_room_failed", data => { Debug.LogWarning("Socket.IO response not implemented for 'join_room_failed'"); });
            socket.On("join_room_success", data => { Debug.LogWarning("Socket.IO response not implemented for 'join_room_success'"); });

            socket.On("client_joined", data => { Debug.LogWarning("Socket.IO response not implemented for 'client_joined'"); });
            socket.On("new_message", data => { Debug.LogWarning("Socket.IO response not implemented for 'new_message'"); });
            socket.On("client_disconnected", data => { Debug.LogWarning("Socket.IO response not implemented for 'client_disconnected'"); });
        }

        private JObject UserData => new JObject {["username"] = Username, ["avatar"] = AvatarIndex, ["guid"] = GUID, ["room"] = RoomID};
    }
}