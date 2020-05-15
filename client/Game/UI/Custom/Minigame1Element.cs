using System.Collections.Generic;
using System.IO;
using System.Linq;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;
using game.utils;

namespace game.ui {
    public class Minigame1Element : GameObject {
        public static Minigame1Element ActiveMinigame;

        private readonly Rectangle bounds;
        private LabelStyle questionStyle;
        private LabelStyle remainingVotesStyle;
        private LabelStyle playerNameStyle;
        private LabelStyle resultStyle;
        private LabelStyle alertStyle;
        private ButtonStyle buttonStyle;

        private List<PlayerAvatarElement> playerAvatars;
        private PlayerAvatarElement votedPlayer;
        private PlayerAvatarElement resultPlayer;
        private Label questionLabel;
        private Label remainingVotesLabel;
        private Label resultLabel;
        private Button stopPlayingButton;
        private Button nextQuestionButton;
        private readonly Pivot rootElement;

        public Minigame1Element(float x, float y, float width, float height, LabelStyle questionStyle, LabelStyle remainingVotesStyle, LabelStyle playerNameStyle, LabelStyle resultStyle, LabelStyle alertStyle,ButtonStyle buttonStyle) {
            bounds = new Rectangle(x, y, width, height);
            this.questionStyle = questionStyle;
            this.remainingVotesStyle = remainingVotesStyle;
            this.playerNameStyle = playerNameStyle;
            this.resultStyle = resultStyle;
            this.alertStyle = alertStyle;
            this.buttonStyle = buttonStyle;
            rootElement = new Pivot();
            AddChild(rootElement);
            SetXY(x, y);
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }

        public void Initialize(int state) {
            Deinitialize();
            var isOwner = NetworkManager.Instance.PlayerData.GUID == NetworkManager.Instance.ActiveMinigame1.Owner;
            var backgroundPath = "data/sprites/minigames/1/background";
            backgroundPath += (isOwner ? "_owner" : "") + ".png";
            var topOffset = isOwner ? 0f : 50;
            rootElement.AddChild(new Image(0, 0, bounds.width, bounds.height, new game.Sprite(backgroundPath, true, false)));

            //title

            // question
            questionLabel = new Label(0, 100, bounds.width, 60, NetworkManager.Instance.ActiveMinigame1.ActiveQuestion, questionStyle);
            rootElement.AddChild(questionLabel);
            if (state == 0) {
                playerAvatars = new List<PlayerAvatarElement>();
                var players = NetworkManager.Instance.ActiveRoom.Players;
                var index = 0;
                var spriteSize = 64f;
                var spacing = 8f;
                if (players.Count <= 8) {
                    spriteSize = 128f;
                    spacing = 11f;
                }

                foreach (var playerGUID in players.Keys.ToList().Sorted()) {
                    var player = players[playerGUID];
                    var x = index * spriteSize + (index == 0 ? 0 : spacing);
                    var playerAvatar = new PlayerAvatarElement(x, bounds.height / 2f - spriteSize / 2f + topOffset, player.Username, player.AvatarIndex, playerNameStyle, () => { OnVoteClicked(player.GUID); }, spriteSize);
                    index++;
                    playerAvatars.Add(playerAvatar);
                }

                playerAvatars.ForEach(avatar => rootElement.AddChild(avatar));
            } else if (state == 1) {
                var votedGUID = NetworkManager.Instance.ActiveMinigame1.ActiveQuestionVotes[NetworkManager.Instance.PlayerData.GUID];
                var voted = NetworkManager.Instance.ActiveRoom.Players[votedGUID];
                votedPlayer = new PlayerAvatarElement(bounds.width / 4f - 64f, bounds.height / 2f - 64f + topOffset, voted.Username, voted.AvatarIndex, playerNameStyle);
                rootElement.AddChild(votedPlayer);
                var totalVotes = NetworkManager.Instance.ActiveRoom.Players.Count;
                var remainingVotes = NetworkManager.Instance.ActiveMinigame1.RemainingVotes;
                var totalVoted = totalVotes - remainingVotes;
                remainingVotesLabel = new Label(bounds.width / 2f, bounds.height / 2f + topOffset, bounds.width / 2f, 170, $"Voted: {totalVoted}/{totalVotes} ({remainingVotes} remaining)", remainingVotesStyle);
                rootElement.AddChild(remainingVotesLabel);
            } else {
                var totalVotes = NetworkManager.Instance.ActiveRoom.Players.Count;
                var (winnerGuid, winnerVotes) = NetworkManager.Instance.ActiveMinigame1.Result();
                var winner = NetworkManager.Instance.ActiveRoom.Players[winnerGuid];
                resultPlayer = new PlayerAvatarElement(20, bounds.height / 2f - 64f + topOffset, winner.Username, winner.AvatarIndex, playerNameStyle);
                rootElement.AddChild(resultPlayer);
                resultLabel = new Label(168f, 100 + topOffset, bounds.width / 2f, 250, $"WINNER\nWith {winnerVotes}/{totalVotes} votes", resultStyle);
                rootElement.AddChild(resultLabel);
                if (!isOwner) {
                    rootElement.AddChild(new Label(bounds.width/2f, 100 + topOffset, bounds.width/2f, 250, "WAITING FOR CREATOR TO MOVE ON TO THE NEXT QUESTION", alertStyle));
                }
            }

            if (!isOwner)
                return;
            nextQuestionButton = new Button(0, 350, bounds.width / 2f, 100, "NEXT QUESTION", buttonStyle, OnNextQuestionClicked);
            stopPlayingButton = new Button(bounds.width / 2f, 350, bounds.width / 2f, 100, "STOP PLAYING", buttonStyle, OnStopPlayingClicked);
            rootElement.AddChild(nextQuestionButton);
            rootElement.AddChild(stopPlayingButton);
        }

        private void OnNextQuestionClicked() {
            NetworkManager.Instance.NextQuestionMinigame1();
        }

        private void OnStopPlayingClicked() {
            NetworkManager.Instance.StopPlayingMinigame1();
        }

        private void OnVoteClicked(string playerGuid) {
            NetworkManager.Instance.VoteMinigame1(playerGuid);
            Initialize(1);
        }

        private static List<string> questionList;
        private static bool isQuestionListInitialized;
        public static string GetQuestion(int questionIndex) {
            if (!isQuestionListInitialized) InitializeQuestionList();
            return questionList[questionIndex];
        }

        private static void InitializeQuestionList() {
            questionList = new List<string>();
            File.OpenText("data/most_likely_to.txt").ReadToEnd().Split('\n').ToList().ForEach(s => {
                var chr = s[0];
                chr = char.ToUpperInvariant(chr);
                questionList.Add(new string(chr, 1) + s.Substring(1));
            });
            isQuestionListInitialized = true;
        }
    }
}