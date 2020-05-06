using System;
using System.Drawing;
using GXPEngine;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace game {
    public class Player : EasyDraw {
        public Socket Socket;

        public string Name;
        public string GUID;
        public float Speed;
        public Vector2 Position;

        public Player(string name) : this(name, name.GetHashCode().ToString()) { }
        public Player(string name, string guid) : this(name, guid, Vector2.zero) { }
        public Player(string name, Vector2 position) : this(name, name.GetHashCode().ToString(), position) { }

        public Player(string name, string guid, Vector2 position, float speed = 1 << 8) : base(Globals.WIDTH, Globals.HEIGHT, false) {
            Name = name;
            GUID = guid;
            Position = position;
            Speed = speed;
            Debug.LogInfo($"Self: GUID: {guid}");
            Draw();
            SetupSocket();
        }

        private void Update() {
            var movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
            if (movement != Vector2.zero) {
                Position += Speed * Time.deltaTime * movement;

                Draw();
            }

            if (Input.GetKeyDown(Key.SPACE)) Socket.Emit("debug");
        }

        private void Draw() {
            var container = base.graphics.BeginContainer();
            Clear(Color.Transparent);
            Fill(Color.Coral);
            NoStroke();
            Rect(Position.x, Position.y, 64f, 64f);
            Fill(Color.Chartreuse);
            TextSize(18f);
            TextAlign(CenterMode.Center, CenterMode.Max);
            Text(Name, Position.x, Position.y - 36f);
            base.graphics.EndContainer(container);
        }

        private void SetupSocket() {
            Socket = IO.Socket("http://localhost:8080");
            Socket.On("connect", () => {
                Debug.LogInfo("Requesting own data");
            });
            Socket.On("disconnect", (data) => { Console.WriteLine($"Disconnected. Reason: {data}"); });
            Socket.On("debug", data => { Console.WriteLine("DEBUG DATA:\n{0}", data); });
            
            // OTHER CLIENTS

            Socket.On("client_joined", data => {
                var playerDataJObject = (JObject) data;
                // TODO: HANDLE client_joined

                /*var playerData = new NetworkData(
                    playerDataJObject["name"].Value<string>(),
                    playerDataJObject["id"].Value<string>(),
                    new Vector2(playerDataJObject["position"]["x"].Value<float>(), playerDataJObject["position"]["y"].Value<float>())
                );
                NetworkManager.Instance.InitialiseClient(playerData);*/
            });

            Socket.On("client_disconnected", data => {
                var playerId = ((JObject) data)["playerId"].Value<string>();
                // TODO: HANDLE client_disconnected
                
                // NetworkManager.Instance.RemoveClient(playerId);
            });
            
        }

        ~Player() {
            Socket.Disconnect();
            Socket.Close();
        }
    }
}