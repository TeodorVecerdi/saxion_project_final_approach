using System.Collections.Generic;
using GXPEngine;
using Newtonsoft.Json.Linq;

namespace game.network {
    public class NetworkManager : GameObject {
        public static NetworkManager Instance;

        private Dictionary<string, NetworkPlayer> clients;

        public NetworkManager() {
            clients = new Dictionary<string, NetworkPlayer>();
        }

        public void InitialiseClient(NetworkData playerData) {
            var client = new NetworkPlayer(playerData.Name, playerData.GUID, playerData.Position);
            App.Instance.AddChild(client);
            clients.Add(playerData.GUID, client);
        }

        public void RemoveClient(string playerId) {
            if (!clients.ContainsKey(playerId)) {
                Debug.LogError($"Client with GUID {playerId} could not be found. Make sure you initialise this client before trying to remove it.");
                return;
            }
            
            clients[playerId].Remove();
            clients[playerId].LateDestroy();
            clients.Remove(playerId);
        }

        public void UpdateClient(NetworkData playerData) {
            if (!clients.ContainsKey(playerData.GUID)) {
                Debug.LogError($"Client with GUID {playerData.GUID} could not be found. Make sure you initialise this client before trying to update it.");
                return;
            }

            clients[playerData.GUID].UpdateClient(playerData);
        }
    }
}