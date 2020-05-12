using System;
using System.Collections.Generic;
using game.ui;
using GXPEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace game {
    public class NetworkManager : GameObject {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ?? (instance = new NetworkManager());

        public NetworkPlayer PlayerData;
        public NetworkRoom ActiveRoom;
        public Dictionary<string, NetworkVotingSession> ActiveVotingSessions;

        private Socket socket;
        private bool initialized;
        private bool clientLoggedIn;
        private bool joinRoomSuccess;
        
        private bool votingSessionDone;
        private string finishedVotingSessionGUID;

        private bool gotNewMessage;
        private ChatMessage newestMessage;

        public bool RoomsReady;
        public List<NetworkRoom> Rooms;

        private NetworkManager() {
            ActiveVotingSessions = new Dictionary<string, NetworkVotingSession>();
        }

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
            ActiveRoom = new NetworkRoom(roomName, roomDesc, Guid.NewGuid().ToString(), code, PlayerData.Location, isPublic, isNSFW);
            ActiveRoom.Players.Add(PlayerData.GUID);
            PlayerData.RoomID = ActiveRoom.GUID;
            socket.Emit("create_room", ActiveRoom.JSONString);
            SceneManager.Instance.LoadScene($"{PlayerData.Location}-Bar");
        }

        public void TryJoinRoom(string roomGuid, string roomCode) {
            var roomData = new JObject {["guid"] = roomGuid, ["code"] = roomCode};
            socket.Emit("join_room", roomData.ToString(Formatting.None));
        }

        public void RequestRooms() {
            socket.Emit("request_rooms");
        }

        public void JoinLocation(string location, bool alsoSwitchScene = true) {
            PlayerData.Location = location;
            socket.Emit("set_location", PlayerData.Location);
            SceneManager.Instance.LoadScene($"{location}-Menu");
        }

        public void SendMessage(ChatMessage message) {
            socket.Emit("send_message", message.JSONString);
        }

        public void ReceivedMessage(ChatMessage message) {
            ChatElement.ActiveChat.ReceiveMessage(message);
        }

        public void StartVotingSession(string reason) {
            var votingSession = new NetworkVotingSession(Guid.NewGuid().ToString(), reason, ActiveRoom);
            socket.Emit("start_voting_session", votingSession.JSONString);
        }

        private void FinishVotingSession(NetworkVotingSession votingSession) {
            ActiveVotingSessions.Remove(finishedVotingSessionGUID);
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
                SceneManager.Instance.LoadScene($"{PlayerData.Location}-Bar");
                joinRoomSuccess = false;
            }

            if (votingSessionDone) {
                FinishVotingSession(ActiveVotingSessions[finishedVotingSessionGUID]);
                votingSessionDone = false;
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
                ActiveRoom = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"), roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                ActiveRoom.Players.Add(PlayerData.GUID);
                PlayerData.RoomID = ActiveRoom.GUID;
                joinRoomSuccess = true;
            });

            socket.On("client_joined", data => {
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` joined the room!");
                ActiveRoom.Players.Add(playerData.Value<string>("guid"));
                gotNewMessage = true;
            });
            socket.On("new_message", data => {
                newestMessage = ChatMessage.FromJSON(JObject.Parse(((JObject) data).Value<string>("message")));
                gotNewMessage = true;
            });
            socket.On("client_disconnected", data => {
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` left the room!");
                ActiveRoom.Players.Remove(playerData.Value<string>("guid"));
                gotNewMessage = true;
            });

            socket.On("started_voting_session", data => {
                Debug.LogWarning("Socket.IO response not implemented for 'started_voting_session'");
                var voteData = (JObject) data;
                var votingSession = new NetworkVotingSession(voteData.Value<string>("guid"), voteData.Value<string>("reason"), ActiveRoom);
                ActiveVotingSessions.Add(votingSession.GUID, votingSession);
            });
            
            socket.On("update_voting_session", data => {
                var voteData = (JObject) data;
                var votingSessionData = voteData["voteSession"];
                var votingSessionGUID = votingSessionData.Value<string>("guid");
                if (!ActiveVotingSessions.ContainsKey(votingSessionGUID)) {
                    return;
                }
                var votes = (JObject) votingSessionData["votes"];
                foreach (var prop in votes.Properties()) {
                    var vote = votes.Value<int>(prop.Name);
                    if (vote != -1) {
                        ActiveVotingSessions[votingSessionGUID].Votes[prop.Name] = vote == 1 ? Vote.Yes : Vote.No;
                    }
                }
            });
            
            socket.On("finish_voting_session", data => {
                Debug.LogWarning("Socket.IO response not implemented for 'finish_voting_session'");
                var voteData = (JObject) data;
                var votingSessionData = voteData["voteSession"];
                var votingSessionGUID = votingSessionData.Value<string>("guid");
                votingSessionDone = true;
                finishedVotingSessionGUID = votingSessionGUID;
            });
        }
    }
}