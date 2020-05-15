using System;
using System.Collections.Generic;
using System.IO;
using game.ui;
using GXPEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using Debug = game.utils.Debug;

namespace game {
    public class NetworkManager : GameObject {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ?? (instance = new NetworkManager());

        public NetworkPlayer PlayerData;
        public NetworkRoom ActiveRoom;
        public NetworkMostLikelyTo ActiveMinigame1;
        public NetworkWouldYouRather ActiveMinigame2;
        public NetworkNeverHaveIEver ActiveMinigame3;

        private Socket socket;
        private bool initialized;
        private bool clientLoggedIn;
        private bool joinRoomSuccess;
        private bool startedMinigame1;
        private bool newVoteMinigame1;
        private bool newQuestionMinigame1;
        private bool finishedMinigame1;
        private bool showResultsMinigame1;

        private bool startedMinigame2;
        private bool newVoteMinigame2;
        private bool newQuestionMinigame2;
        private bool showResultsMinigame2;
        private bool finishedMinigame2;

        private bool startedMinigame3;
        private bool newVoteMinigame3;
        private bool newQuestionMinigame3;
        private bool showResultsMinigame3;
        private bool finishedMinigame3;

        private bool gotNewMessage;
        private ChatMessage newestMessage;

        public bool RoomsReady;
        public List<NetworkRoom> Rooms;

        private NetworkManager() { }

        public void Initialize() {
            PlayerData = new NetworkPlayer("", Guid.NewGuid().ToString(), "none", "none", -1, false);

            var hosted = File.ReadAllText("data/hosted.txt") == "1";
            var socketURL = "http://35.238.111.228:3000";

            // if (!hosted) socketURL = "http://localhost:3000";
            socket = IO.Socket(socketURL);

            SetupSocket();
        }

        public void CreateAccount(string username, int avatarIndex, bool consent) {
            PlayerData.Username = username;
            PlayerData.AvatarIndex = avatarIndex;
            PlayerData.Consent = consent;
            socket.Emit("update_account", PlayerData.JSONString);
            Debug.LogInfo($"Socket.IO Emit: 'update_account', sent data:\n{PlayerData.JSONString}\n", "NETWORK");
        }

        public void CreateAndJoinRoom(string roomName, string roomDesc, string code, bool isNSFW, bool isPublic) {
            ActiveRoom = new NetworkRoom(roomName, roomDesc, Guid.NewGuid().ToString(), code, PlayerData.Location, isPublic, isNSFW);
            ActiveRoom.Players.Add(PlayerData.GUID, PlayerData);
            PlayerData.RoomID = ActiveRoom.GUID;
            socket.Emit("create_room", ActiveRoom.JSONString);
            Debug.LogInfo($"Socket.IO Emit: 'create_room', sent data:\n{ActiveRoom.JSONString}\n", "NETWORK");
            SceneManager.Instance.LoadScene($"{PlayerData.Location}-Bar");
            SoundManager.Instance.PlaySound("bar_ambiance");
        }

        public void TryJoinRoom(string roomGuid, string roomCode) {
            var roomData = new JObject {["guid"] = roomGuid, ["code"] = roomCode};
            socket.Emit("join_room", roomData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'join_room', sent data:\n{roomData.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void RequestRooms() {
            socket.Emit("request_rooms");
            Debug.LogInfo($"Socket.IO Emit: 'join_room', sent data: \"\"", "NETWORK");
        }

        public void JoinLocation(string location, bool joiningLocation = true) {
            PlayerData.Location = location;
            socket.Emit("set_location", PlayerData.Location);
            Debug.LogInfo($"Socket.IO Emit: 'set_location', sent data:\n{PlayerData.Location}\n", "NETWORK");
            SceneManager.Instance.LoadScene(joiningLocation ? $"{location}-Menu" : "Map");
        }

        public void LeaveRoom() {
            socket.Emit("leave_room");
            Debug.LogInfo($"Socket.IO Emit: 'leave_room', sent data: \"\"", "NETWORK");
            SoundManager.Instance.StopPlaying("bar_ambiance");
            JoinLocation(PlayerData.Location);
        }

        public void SendMessage(ChatMessage message) {
            socket.Emit("send_message", message.JSONString);
            Debug.LogInfo($"Socket.IO Emit: 'send_message', sent data:\n{message.JSONString}\n", "NETWORK");
        }

        private void ReceivedMessage(ChatMessage message) {
            ChatElement.ActiveChat.ReceiveMessage(message);
            if (message.SenderGUID != "00000000-0000-0000-0000-000000000000")
                SoundManager.Instance.PlaySound("new_message");
        }

        public void StartMinigame1() {
            var minigame1Data = new NetworkMostLikelyTo(Guid.NewGuid().ToString(), PlayerData.GUID);
            ActiveMinigame1 = minigame1Data;
            var jsonData = minigame1Data.JSONObject;
            jsonData["roomGuid"] = ActiveRoom.GUID;
            socket.Emit("start_minigame_1", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'start_minigame_1', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
            socket.Emit("request_minigame_1", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_1', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void NextQuestionMinigame1() {
            socket.Emit("request_minigame_1", new JObject {["gameGuid"] = ActiveMinigame1.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_1', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame1.GUID}.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void StopPlayingMinigame1() {
            socket.Emit("finish_minigame_1", new JObject {["gameGuid"] = ActiveMinigame1.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'finish_minigame_1', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame1.GUID}.ToString(Formatting.None)}\n", "NETWORK");
            Minigame1Element.ActiveMinigame.Deinitialize();
        }

        public void VoteMinigame1(string playerGuid) {
            ActiveMinigame1.SetVote(PlayerData.GUID, playerGuid);
            if (ActiveMinigame1.Owner == PlayerData.GUID && ActiveMinigame1.IsQuestionDone) {
                socket.Emit("results_minigame_1", "");
                Debug.LogInfo($"Socket.IO Emit: 'results_minigame_1', sent data: \"\"", "NETWORK");
                socket.Emit("voted_minigame_1", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_1', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None)}\n", "NETWORK");
                showResultsMinigame1 = true;
            } else {
                socket.Emit("voted_minigame_1", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_1', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None)}\n", "NETWORK");
            }
        }

        public void StartMinigame2() {
            var minigame2Data = new NetworkWouldYouRather(Guid.NewGuid().ToString(), PlayerData.GUID);
            ActiveMinigame2 = minigame2Data;
            var jsonData = minigame2Data.JSONObject;
            jsonData["roomGuid"] = ActiveRoom.GUID;
            socket.Emit("start_minigame_2", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'start_minigame_2', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
            socket.Emit("request_minigame_2", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_2', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void NextQuestionMinigame2() {
            socket.Emit("request_minigame_2", new JObject {["gameGuid"] = ActiveMinigame2.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_2', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame2.GUID}.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void StopPlayingMinigame2() {
            socket.Emit("finish_minigame_2", new JObject {["gameGuid"] = ActiveMinigame2.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'finish_minigame_2', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame2.GUID}.ToString(Formatting.None)}\n", "NETWORK");
            Minigame2Element.ActiveMinigame.Deinitialize();
        }

        public void VoteMinigame2(string playerGuid) {
            ActiveMinigame2.SetVote(PlayerData.GUID, playerGuid);
            if (ActiveMinigame2.Owner == PlayerData.GUID && ActiveMinigame2.IsQuestionDone) {
                socket.Emit("results_minigame_2", "");
                Debug.LogInfo($"Socket.IO Emit: 'results_minigame_2', sent data: \"\"", "NETWORK");
                socket.Emit("voted_minigame_2", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_2', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None)}\n", "NETWORK");
                showResultsMinigame2 = true;
            } else {
                socket.Emit("voted_minigame_2", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_2', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None)}\n", "NETWORK");
            }
        }

        public void StartMinigame3() {
            var minigame3Data = new NetworkNeverHaveIEver(Guid.NewGuid().ToString(), PlayerData.GUID);
            ActiveMinigame3 = minigame3Data;
            var jsonData = minigame3Data.JSONObject;
            jsonData["roomGuid"] = ActiveRoom.GUID;
            socket.Emit("start_minigame_3", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'start_minigame_3', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
            socket.Emit("request_minigame_3", jsonData.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_3', sent data:\n{jsonData.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void NextQuestionMinigame3() {
            socket.Emit("request_minigame_3", new JObject {["gameGuid"] = ActiveMinigame3.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'request_minigame_3', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame3.GUID}.ToString(Formatting.None)}\n", "NETWORK");
        }

        public void StopPlayingMinigame3() {
            socket.Emit("finish_minigame_3", new JObject {["gameGuid"] = ActiveMinigame3.GUID}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'finish_minigame_3', sent data:\n{new JObject {["gameGuid"] = ActiveMinigame3.GUID}.ToString(Formatting.None)}\n", "NETWORK");
            Minigame3Element.ActiveMinigame.Deinitialize();
        }

        public void VoteMinigame3(string playerGuid) {
            ActiveMinigame3.SetVote(PlayerData.GUID, playerGuid);
            if (ActiveMinigame3.Owner == PlayerData.GUID && ActiveMinigame3.IsQuestionDone) {
                socket.Emit("results_minigame_3", "");
                Debug.LogInfo($"Socket.IO Emit: 'results_minigame_3', sent data: \"\"", "NETWORK");
                socket.Emit("voted_minigame_3", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_3', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid, ["redirect"] = false}.ToString(Formatting.None)}\n", "NETWORK");
                showResultsMinigame3 = true;
            } else {
                socket.Emit("voted_minigame_3", new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None));
                Debug.LogInfo($"Socket.IO Emit: 'voted_minigame_3', sent data:\n{new JObject {["guid"] = PlayerData.GUID, ["vote"] = playerGuid}.ToString(Formatting.None)}\n", "NETWORK");
            }
        }

        public void PlaySound(string soundId, bool stopAlreadyPlaying) {
            socket.Emit("play_sound", new JObject {["soundId"] = soundId, ["stopAlreadyPlaying"] = stopAlreadyPlaying}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'play_sound', sent data:\n{new JObject {["soundId"] = soundId, ["stopAlreadyPlaying"] = stopAlreadyPlaying}.ToString(Formatting.None)}\n", "NETWORK");
            SoundManager.Instance.PlaySound(soundId, stopAlreadyPlaying);
        }

        public void StopPlayingSound(string soundId) {
            socket.Emit("stop_playing_sound", new JObject {["soundId"] = soundId}.ToString(Formatting.None));
            Debug.LogInfo($"Socket.IO Emit: 'stop_playing_sound', sent data:\n{new JObject {["soundId"] = soundId}.ToString(Formatting.None)}\n", "NETWORK");
            SoundManager.Instance.StopPlaying(soundId);
        }

        private void Update() {
            if (clientLoggedIn) {
                SceneManager.Instance.LoadScene("FakeLoading");
                clientLoggedIn = false;
            }

            if (gotNewMessage) {
                ReceivedMessage(newestMessage);
                gotNewMessage = false;
            }

            if (joinRoomSuccess) {
                SceneManager.Instance.LoadScene($"{PlayerData.Location}-Bar");
                SoundManager.Instance.PlaySound("bar_ambiance");
                joinRoomSuccess = false;
            }

            if (startedMinigame1) {
                startedMinigame1 = false;
            }

            if (newVoteMinigame1) {
                if (ActiveMinigame1.ActiveQuestionVotes[PlayerData.GUID] != "")
                    Minigame1Element.ActiveMinigame.Initialize(1);
                newVoteMinigame1 = false;
            }

            if (newQuestionMinigame1) {
                Minigame1Element.ActiveMinigame.Initialize(0);
                newQuestionMinigame1 = false;
            }

            if (showResultsMinigame1) {
                Minigame1Element.ActiveMinigame.Initialize(2);
                showResultsMinigame1 = false;
            }

            if (finishedMinigame1) {
                Minigame1Element.ActiveMinigame.Deinitialize();
                finishedMinigame1 = false;
            }

            if (startedMinigame2) {
                startedMinigame2 = false;
            }

            if (newVoteMinigame2) {
                if (ActiveMinigame2.ActiveQuestionVotes[PlayerData.GUID] != "")
                    Minigame2Element.ActiveMinigame.Initialize(1);
                newVoteMinigame2 = false;
            }

            if (newQuestionMinigame2) {
                Minigame2Element.ActiveMinigame.Initialize(0);
                newQuestionMinigame2 = false;
            }

            if (showResultsMinigame2) {
                Minigame2Element.ActiveMinigame.Initialize(2);
                showResultsMinigame2 = false;
            }

            if (finishedMinigame2) {
                Minigame2Element.ActiveMinigame.Deinitialize();
                finishedMinigame2 = false;
            }

            if (startedMinigame3) {
                startedMinigame3 = false;
            }

            if (newVoteMinigame3) {
                if (ActiveMinigame3.ActiveQuestionVotes[PlayerData.GUID] != "")
                    Minigame3Element.ActiveMinigame.Initialize(1);
                newVoteMinigame3 = false;
            }

            if (newQuestionMinigame3) {
                Minigame3Element.ActiveMinigame.Initialize(0);
                newQuestionMinigame3 = false;
            }

            if (showResultsMinigame3) {
                Minigame3Element.ActiveMinigame.Initialize(2);
                showResultsMinigame3 = false;
            }

            if (finishedMinigame3) {
                Minigame3Element.ActiveMinigame.Deinitialize();
                finishedMinigame3 = false;
            }
        }

        private void SetupSocket() {
            SetupSocket_Basic();
            SetupSocket_Rooms();
            SetupSocket_Sound();
            SetupSocket_Minigames();
        }

        private void SetupSocket_Basic() {
            socket.On("connect", data => {
                Debug.LogInfo($"Socket.IO: 'connect', received data:\n{data?.ToString()}\n", "NETWORK");
                if (!initialized)
                    clientLoggedIn = true;
                initialized = true;
            });
            socket.On("disconnect", data => {
                Debug.LogInfo($"Socket.IO: 'disconnect', received data:\n{data?.ToString()}\n", "NETWORK");
                Debug.Log($"Client disconnected. Reason: {data}");
            });
            socket.On("request_account", data => {
                Debug.LogInfo($"Socket.IO: 'request_account', received data:\n{data?.ToString()}\n", "NETWORK");
                socket.Emit("request_account_success", PlayerData.JSONString);
                Debug.LogInfo($"Socket.IO Emit: 'request_account_success', sent data:\n{PlayerData.JSONString}\n", "NETWORK");
            });
        }

        private void SetupSocket_Rooms() {
            socket.On("request_rooms_success", data => {
                Debug.LogInfo($"Socket.IO: 'request_rooms_success', received data:\n{data?.ToString()}\n", "NETWORK");
                Rooms = new List<NetworkRoom>();
                var objData = (JObject) data;
                foreach (var prop in objData.Properties()) {
                    var roomData = (JObject) objData[prop.Name];
                    var room = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"), roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                    Rooms.Add(room);
                }

                RoomsReady = true;
            });
            socket.On("create_room_success", data => { Debug.LogInfo($"Socket.IO: 'create_room_success', received data:\n{data?.ToString()}\n", "NETWORK"); });
            socket.On("join_room_failed", data => {
                Debug.LogInfo($"Socket.IO: 'join_room_failed', received data:\n{data?.ToString()}\n", "NETWORK");
                Debug.LogWarning("Socket.IO response not implemented for 'join_room_failed'");
            });
            socket.On("join_room_success", data => {
                Debug.LogInfo($"Socket.IO: 'join_room_success', received data:\n{data?.ToString()}\n", "NETWORK");
                var roomData = (JObject) data;
                ActiveRoom = new NetworkRoom(roomData.Value<string>("name"), roomData.Value<string>("desc"), roomData.Value<string>("guid"), roomData.Value<string>("code"), roomData.Value<string>("type"), roomData.Value<bool>("pub"), roomData.Value<bool>("nsfw"));
                PlayerData.RoomID = ActiveRoom.GUID;
                ActiveRoom.Players.Add(PlayerData.GUID, PlayerData);
                joinRoomSuccess = true;
                socket.Emit("request_players", ActiveRoom.GUID);
                Debug.LogInfo($"Socket.IO Emit: 'request_players', sent data:\n{ActiveRoom.GUID}\n", "NETWORK");
            });
            socket.On("request_players_success", data => {
                Debug.LogInfo($"Socket.IO: 'request_players_success', received data:\n{data?.ToString()}\n", "NETWORK");
                var playerData = (JObject) data;
                foreach (var playerId in playerData.Properties()) {
                    var networkPlayerData = new NetworkPlayer(playerData[playerId.Name].Value<string>("username"), playerData[playerId.Name].Value<string>("guid"), playerData[playerId.Name].Value<string>("room"), playerData[playerId.Name].Value<string>("location"), playerData[playerId.Name].Value<int>("avatar"), playerData[playerId.Name].Value<bool>("consent"));
                    ActiveRoom.Players.Add(playerId.Name, networkPlayerData);
                }
            });
            socket.On("client_joined", data => {
                Debug.LogInfo($"Socket.IO: 'client_joined', received data:\n{data?.ToString()}\n", "NETWORK");
                if (PlayerData.RoomID == "none") return;
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` joined the room!");
                ActiveRoom.Players.Add(playerData.Value<string>("guid"), new NetworkPlayer() {AvatarIndex = playerData.Value<int>("avatar"), Username = playerData.Value<string>("username"), GUID = playerData.Value<string>("guid")});
                SoundManager.Instance.PlaySound("client_joined");
                gotNewMessage = true;
            });

            socket.On("new_message", data => {
                Debug.LogInfo($"Socket.IO: 'new_message', received data:\n{data?.ToString()}\n", "NETWORK");
                newestMessage = ChatMessage.FromJSON(JObject.Parse(((JObject) data).Value<string>("message")));
                gotNewMessage = true;
            });

            socket.On("client_disconnected", data => {
                Debug.LogInfo($"Socket.IO: 'client_disconnected', received data:\n{data?.ToString()}\n", "NETWORK");
                if (PlayerData.RoomID == "none") return;
                var playerData = (JObject) data;
                newestMessage = new ChatMessage("SERVER", "00000000-0000-0000-0000-000000000000", $"`{playerData.Value<string>("username")}` left the room!");
                ActiveRoom.Players.Remove(playerData.Value<string>("guid"));
                SoundManager.Instance.PlaySound("client_left", true);
                gotNewMessage = true;
            });
        }

        private void SetupSocket_Sound() {
            socket.On("play_sound", data => {
                Debug.LogInfo($"Socket.IO: 'play_sound', received data:\n{data?.ToString()}\n", "NETWORK");
                var jsonData = (JObject) data;
                var soundId = jsonData.Value<string>("soundId");
                var stopAlreadyPlaying = jsonData.Value<bool>("stopAlreadyPlaying");
                if (soundId.StartsWith("Song")) {
                    JukeboxElement.ActiveJukebox.CurrentlyPlaying = soundId;
                }

                SoundManager.Instance.PlaySound(soundId, stopAlreadyPlaying);
            });

            socket.On("stop_playing_sound", data => {
                Debug.LogInfo($"Socket.IO: 'stop_playing_sound', received data:\n{data?.ToString()}\n", "NETWORK");
                var jsonData = (JObject) data;
                var soundId = jsonData.Value<string>("soundId");
                SoundManager.Instance.StopPlaying(soundId);
            });
        }

        private void SetupSocket_Minigames() {
            SetupSocket_Minigames_1();
            SetupSocket_Minigames_2();
            SetupSocket_Minigames_3();
        }

        private void SetupSocket_Minigames_1() {
            socket.On("started_minigame_1", data => {
                Debug.LogInfo($"Socket.IO: 'started_minigame_1', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                ActiveMinigame1 = new NetworkMostLikelyTo(minigameData.Value<string>("gameGuid"), minigameData.Value<string>("ownerGuid"));
                startedMinigame1 = true;
            });
            socket.On("voted_minigame_1", data => {
                Debug.LogInfo($"Socket.IO: 'voted_minigame_1', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var redirect = minigameData.TryGetValue("redirect", out _);
                ActiveMinigame1.SetVote(minigameData.Value<string>("guid"), minigameData.Value<string>("vote"));
                if (redirect) return;
                if (ActiveMinigame1.Owner == PlayerData.GUID && ActiveMinigame1.IsQuestionDone) {
                    socket.Emit("results_minigame_1", "");
                    Debug.LogInfo($"Socket.IO Emit: 'results_minigame_1', sent data: \"\"", "NETWORK");
                    showResultsMinigame1 = true;
                } else {
                    newVoteMinigame1 = true;
                }
            });

            socket.On("request_minigame_1", data => {
                Debug.LogInfo($"Socket.IO: 'request_minigame_1', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var questionIndex = minigameData.Value<int>("question");
                var question = Minigame1Element.GetQuestion(questionIndex);
                ActiveMinigame1.StartNewQuestion(question);
                newQuestionMinigame1 = true;
            });
            socket.On("results_minigame_1", data => {
                Debug.LogInfo($"Socket.IO: 'results_minigame_1', received data:\n{data?.ToString()}\n", "NETWORK");
                showResultsMinigame1 = true;
            });
            socket.On("finished_minigame_1", data => {
                Debug.LogInfo($"Socket.IO: 'finished_minigame_1', received data:\n{data?.ToString()}\n", "NETWORK");
                finishedMinigame1 = true;
            });
        }

        private void SetupSocket_Minigames_2() {
            socket.On("started_minigame_2", data => {
                Debug.LogInfo($"Socket.IO: 'started_minigame_2', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                ActiveMinigame2 = new NetworkWouldYouRather(minigameData.Value<string>("gameGuid"), minigameData.Value<string>("ownerGuid"));
                startedMinigame2 = true;
            });
            socket.On("voted_minigame_2", data => {
                Debug.LogInfo($"Socket.IO: 'voted_minigame_2', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var redirect = minigameData.TryGetValue("redirect", out _);
                ActiveMinigame2.SetVote(minigameData.Value<string>("guid"), minigameData.Value<string>("vote"));
                if (redirect) return;
                if (ActiveMinigame2.Owner == PlayerData.GUID && ActiveMinigame2.IsQuestionDone) {
                    socket.Emit("results_minigame_2", "");
                    Debug.LogInfo($"Socket.IO Emit: 'results_minigame_2', sent data: \"\"", "NETWORK");
                    showResultsMinigame2 = true;
                } else {
                    newVoteMinigame2 = true;
                }
            });

            socket.On("request_minigame_2", data => {
                Debug.LogInfo($"Socket.IO: 'request_minigame_2', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var questionIndex = minigameData.Value<int>("question");
                var question = Minigame2Element.GetQuestion(questionIndex);
                ActiveMinigame2.StartNewQuestion(question);
                newQuestionMinigame2 = true;
            });
            socket.On("results_minigame_2", data => {
                Debug.LogInfo($"Socket.IO: 'results_minigame_2', received data:\n{data?.ToString()}\n", "NETWORK");
                showResultsMinigame2 = true;
            });
            socket.On("finished_minigame_2", data => {
                Debug.LogInfo($"Socket.IO: 'finished_minigame_2', received data:\n{data?.ToString()}\n", "NETWORK");
                finishedMinigame2 = true;
            });
        }

        private void SetupSocket_Minigames_3() {
            socket.On("started_minigame_3", data => {
                Debug.LogInfo($"Socket.IO: 'started_minigame_3', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                ActiveMinigame3 = new NetworkNeverHaveIEver(minigameData.Value<string>("gameGuid"), minigameData.Value<string>("ownerGuid"));
                startedMinigame3 = true;
            });
            socket.On("voted_minigame_3", data => {
                Debug.LogInfo($"Socket.IO: 'voted_minigame_3', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var redirect = minigameData.TryGetValue("redirect", out _);
                ActiveMinigame3.SetVote(minigameData.Value<string>("guid"), minigameData.Value<string>("vote"));
                if (redirect) return;
                if (ActiveMinigame3.Owner == PlayerData.GUID && ActiveMinigame3.IsQuestionDone) {
                    socket.Emit("results_minigame_3", "");
                    Debug.LogInfo($"Socket.IO Emit: 'results_minigame_3', sent data: \"\"", "NETWORK");
                    showResultsMinigame3 = true;
                } else {
                    newVoteMinigame3 = true;
                }
            });

            socket.On("request_minigame_3", data => {
                Debug.LogInfo($"Socket.IO: 'request_minigame_3', received data:\n{data?.ToString()}\n", "NETWORK");
                var minigameData = (JObject) data;
                var questionIndex = minigameData.Value<int>("question");
                var question = Minigame3Element.GetQuestion(questionIndex);
                ActiveMinigame3.StartNewQuestion(question);
                newQuestionMinigame3 = true;
            });
            socket.On("results_minigame_3", data => {
                Debug.LogInfo($"Socket.IO: 'results_minigame_3', received data:\n{data?.ToString()}\n", "NETWORK");
                showResultsMinigame3 = true;
            });
            socket.On("finished_minigame_3", data => {
                Debug.LogInfo($"Socket.IO: 'finished_minigame_3', received data:\n{data?.ToString()}\n", "NETWORK");
                finishedMinigame3 = true;
            });
        }
    }
}