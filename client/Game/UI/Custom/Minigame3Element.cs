using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;
using game.utils;

namespace game.ui {
    public class Minigame3Element : GameObject {
        public static Minigame3Element ActiveMinigame;

        private readonly Rectangle bounds;
        private LabelStyle questionStyle;
        private LabelStyle choiceLabelStyle;
        private LabelStyle remainingVotesStyle;
        private LabelStyle playerNameStyle;
        private LabelStyle alertStyle;
        private ButtonStyle buttonStyle;
        private ButtonStyle choiceButtonStyle;

        private Label questionLabel;
        private Label remainingVotesLabel;
        private Label resultLabel;
        private Label yesChoiceLabel;
        private Label noChoiceLabel;
        private Button stopPlayingButton;
        private Button nextQuestionButton;
        private Button yesChoiceButton;
        private Button noChoiceButton;
        private readonly Pivot rootElement;

        public Minigame3Element(float x, float y, float width, float height, LabelStyle questionStyle, LabelStyle remainingVotesStyle, LabelStyle playerNameStyle, LabelStyle alertStyle, LabelStyle choiceLabelStyle, ButtonStyle buttonStyle, ButtonStyle choiceButtonStyle) {
            bounds = new Rectangle(x, y, width, height);
            this.questionStyle = questionStyle;
            this.remainingVotesStyle = remainingVotesStyle;
            this.playerNameStyle = playerNameStyle;
            this.alertStyle = alertStyle;
            this.choiceLabelStyle = choiceLabelStyle;
            this.buttonStyle = buttonStyle;
            this.choiceButtonStyle = choiceButtonStyle;
            rootElement = new Pivot();
            AddChild(rootElement);
            SetXY(x, y);
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }

        public void Initialize(int state) {
            Deinitialize();
            var activeMinigame = NetworkManager.Instance.ActiveMinigame3;
            var isOwner = NetworkManager.Instance.PlayerData.GUID == activeMinigame.Owner;
            var backgroundPath = "data/sprites/minigames/3/background";
            backgroundPath += (isOwner ? "_owner" : "") + ".png";
            var topOffset = isOwner ? 0f : 50;
            rootElement.AddChild(new Image(0, 0, bounds.width, bounds.height, new Sprite(backgroundPath, true, false)));

            //title

            // question
            questionLabel = new Label(0, 100, bounds.width, 60, activeMinigame.ActiveQuestion, questionStyle);
            rootElement.AddChild(questionLabel);
            if (state == 0) {
                yesChoiceButton = new Button(57, 170 + topOffset, 420, 160, "s1", choiceButtonStyle, () => {OnVoteClicked("yes");});
                noChoiceButton = new Button(620, 170 + topOffset, 420, 160, "s2", choiceButtonStyle, () => {OnVoteClicked("no");});
                yesChoiceLabel = new Label(57, 170 + topOffset, 420, 160, "I have", choiceLabelStyle);
                noChoiceLabel = new Label(620, 170 + topOffset, 420, 160, "I have never", choiceLabelStyle);
                rootElement.AddChild(yesChoiceButton);
                rootElement.AddChild(noChoiceButton);
                rootElement.AddChild(yesChoiceLabel);
                rootElement.AddChild(noChoiceLabel);
            } else if (state == 1) {
                var votedSituation = activeMinigame.ActiveQuestionVotes[NetworkManager.Instance.PlayerData.GUID];
                if (votedSituation == "yes") {
                    yesChoiceButton = new Button(57, 170 + topOffset, 420, 160, "s1", choiceButtonStyle, () => {OnVoteClicked("yes");});
                    yesChoiceLabel = new Label(57, 170 + topOffset, 420, 160, "I have", choiceLabelStyle);
                    rootElement.AddChild(yesChoiceButton);
                    rootElement.AddChild(yesChoiceLabel);
                } else {
                    noChoiceButton = new Button(57, 170 + topOffset, 420, 160, "s2", choiceButtonStyle, () => {OnVoteClicked("no");});
                    noChoiceLabel = new Label(57, 170 + topOffset, 420, 160, "I have never", choiceLabelStyle);
                    rootElement.AddChild(noChoiceButton);
                    rootElement.AddChild(noChoiceLabel);
                }
                var totalVotes = NetworkManager.Instance.ActiveRoom.Players.Count;
                var remainingVotes = activeMinigame.RemainingVotes;
                var totalVoted = totalVotes - remainingVotes;
                remainingVotesLabel = new Label(bounds.width / 2f, bounds.height / 2f + topOffset, bounds.width / 2f, 170, $"Voted: {totalVoted}/{totalVotes} ({remainingVotes} remaining)", remainingVotesStyle);
                rootElement.AddChild(remainingVotesLabel);
            } else {
                var results = activeMinigame.Result();
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
                    var playerAvatar = new PlayerAvatarElement(x, bounds.height / 2f - spriteSize / 2f + topOffset, player.Username, player.AvatarIndex, playerNameStyle, () => {}, spriteSize);
                    index++;
                    rootElement.AddChild(playerAvatar);
                    var sprite = new Sprite($"data/sprites/{results[playerGUID]}Icon.png", true, false);
                    rootElement.AddChild(new Image(x+spriteSize-64f, bounds.height / 2f + spriteSize/2f + topOffset - 64f, 64f, 64f, sprite));
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
            NetworkManager.Instance.NextQuestionMinigame3();
        }

        private void OnStopPlayingClicked() {
            NetworkManager.Instance.StopPlayingMinigame3();
        }

        private void OnVoteClicked(string choice) {
            NetworkManager.Instance.VoteMinigame3(choice);
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
            File.OpenText("data/never_have_i_ever.txt").ReadToEnd().Split('\n').ToList().ForEach(s => {
                var chr = s[0];
                chr = char.ToUpperInvariant(chr);
                questionList.Add(new string(chr, 1) + s.Substring(1));
            });
            isQuestionListInitialized = true;
        }
    }
}