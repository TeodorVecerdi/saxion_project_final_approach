using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace game {
    public struct NetworkWouldYouRatherQuestion {
        public string Question;
        public string SituationA;
        public string SituationB;

        public NetworkWouldYouRatherQuestion(string question, string situationA, string situationB) {
            Question = question;
            SituationA = situationA;
            SituationB = situationB;
        }

        public JObject JSONObject => new JObject {["question"] = Question, ["situationA"] = SituationA, ["situationB"] = SituationB};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }

    public struct NetworkWouldYouRather {
        public string GUID;
        public string Owner;

        public NetworkWouldYouRatherQuestion ActiveQuestion;
        public Dictionary<string, string> ActiveQuestionVotes;

        public NetworkWouldYouRather(string guid, string owner) {
            GUID = guid;
            Owner = owner;

            ActiveQuestion = new NetworkWouldYouRatherQuestion();
            ActiveQuestionVotes = new Dictionary<string, string>();
        }

        public bool IsQuestionDone => RemainingVotes == 0;
        public int RemainingVotes => ActiveQuestionVotes.Values.Count(string.IsNullOrEmpty);

        public (string, int) Result() {
            var situationA = ActiveQuestion.SituationA;
            var situationB = ActiveQuestion.SituationB;
            var situationACount = ActiveQuestionVotes.Values.Count(s => s == situationA);
            var situationBCount = ActiveQuestionVotes.Values.Count(s => s == situationB);
            return situationACount >= situationBCount ? (situationA, situationACount) : (situationB, situationBCount);
        }

        public void SetVote(string player, string vote) {
            ActiveQuestionVotes[player] = vote;
        }

        public void StartNewQuestion(NetworkWouldYouRatherQuestion question) {
            ActiveQuestion = question;
            ActiveQuestionVotes = NetworkManager.Instance.ActiveRoom.Players.ToDictionary(player => player.Key, player => "");
        }

        public JObject JSONObject => new JObject {["gameGuid"] = GUID, ["ownerGUID"] = Owner};
        public string JSONString => JSONObject.ToString(Formatting.None);
    }
}