using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public enum Vote {
        Unvoted,
        Yes,
        No
    }
    public struct NetworkVotingSession {
        public string GUID;
        public string Creator;
        public string Reason;
        public Dictionary<string, Vote> Votes;

        public NetworkVotingSession(string guid, string creator, string reason, NetworkRoom room) {
            GUID = guid;
            Creator = creator;
            Reason = reason;
            Votes = new Dictionary<string, Vote>();
            foreach (var playerId in room.Players.Keys) {
                Votes.Add(playerId, Vote.Unvoted);
            }
        }

        public void AddVote(string playerId, Vote vote) {
            Votes[playerId] = vote;
        }

        public (int, int) Results() {
            var yes = 0;
            var no = 0;
            foreach (var value in Votes.Values) {
                if (value == Vote.Yes) yes++;
                if (value == Vote.No) no++;
            }

            return (yes, no);
        }

        private JObject CreateVotesObject() {
            var obj = new JObject();
            foreach (var key in Votes.Keys) {
                obj[key] = Votes[key] == Vote.Unvoted ? -1 : Votes[key] == Vote.Yes ? 1 : 0;
            }

            return obj;
        }
        
        public JObject JSONObject => new JObject {["guid"] = GUID, ["reason"] = Reason, ["votes"] = CreateVotesObject()};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}