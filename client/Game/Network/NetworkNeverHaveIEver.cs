using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct NetworkNeverHaveIEver {
        public string GUID;
        public string Owner;

        public string ActiveQuestion;
        public Dictionary<string, string> ActiveQuestionVotes;

        public NetworkNeverHaveIEver(string guid, string owner) {
            GUID = guid;
            Owner = owner;

            ActiveQuestion = "";
            ActiveQuestionVotes = new Dictionary<string, string>();
        }

        public bool IsQuestionDone => RemainingVotes == 0;
        public int RemainingVotes => ActiveQuestionVotes.Values.Count(string.IsNullOrEmpty);

        public Dictionary<string, string> Result() {
            return ActiveQuestionVotes;
        }

        public void SetVote(string player, string vote) {
            ActiveQuestionVotes[player] = vote;
        }

        public void StartNewQuestion(string question) {
            ActiveQuestion = question;
            ActiveQuestionVotes = NetworkManager.Instance.ActiveRoom.Players.ToDictionary(player => player.Key, player => "");
        }

        public JObject JSONObject => new JObject {["gameGuid"] = GUID, ["ownerGUID"] = Owner};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}