using System.Collections.Generic;
using System.Linq;
using game.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct NetworkMostLikelyTo {
        public string GUID;
        public string Owner;
        
        public string ActiveQuestion;
        public Dictionary<string, string> ActiveQuestionVotes;

        public NetworkMostLikelyTo(string guid, string owner) {
            GUID = guid;
            Owner = owner;
            ActiveQuestion = "";
            ActiveQuestionVotes = new Dictionary<string, string>();
        }

        public bool IsQuestionDone => RemainingVotes == 0;
        public int RemainingVotes => ActiveQuestionVotes.Values.Count(string.IsNullOrEmpty);

        public (string, int) Result() {
            var count = NetworkManager.Instance.ActiveRoom.Players.ToDictionary(player => player.Key, player => 0);
            foreach (var vote in ActiveQuestionVotes.Values) {
                if(count.ContainsKey(vote))
                    count[vote]++;
            }
            var winners = new Dictionary<string, int>();
            int max = -1;
            foreach (var c in count) {
                if (c.Value > max) {
                    max = c.Value;
                    winners = new Dictionary<string, int>();
                    winners.Add(c.Key, c.Value);
                } else if (c.Value == max) {
                    winners.Add(c.Key, c.Value);
                }
            }

            var sortedGuids = winners.Keys.ToList().Copy();
            sortedGuids.Sort();
            var winnerGuid = sortedGuids[0];
            var result = winners[winnerGuid];
            return (winnerGuid, result);
        }

        public void SetVote(string player, string vote) {
            ActiveQuestionVotes[player] = vote;
        }

        public void StartNewQuestion(string question) {
            ActiveQuestion = question;
            ActiveQuestionVotes = NetworkManager.Instance.ActiveRoom.Players.ToDictionary(player => player.Key, player => "");
        }
        
        public JObject JSONObject => new JObject{["gameGuid"] = GUID, ["ownerGuid"] = Owner};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}