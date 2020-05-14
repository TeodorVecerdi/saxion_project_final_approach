using System.Collections.Generic;
using System.IO;
using System.Linq;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;
using game.utils;

namespace game.ui {
    public class Minigame2Element : GameObject {
        public static Minigame2Element ActiveMinigame;

        private readonly Rectangle bounds;
        private LabelStyle questionStyle;
        private LabelStyle remainingVotesStyle;
        private LabelStyle resultStyle;
        private LabelStyle situationLabelStyle;
        private LabelStyle alertStyle;
        private ButtonStyle buttonStyle;
        private ButtonStyle situationButtonStyle;

        private Label questionLabel;
        private Label remainingVotesLabel;
        private Label resultLabel;
        private Label situationALabel;
        private Label situationBLabel;
        private Button situationAButton;
        private Button situationBButton;
        private Button stopPlayingButton;
        private Button nextQuestionButton;
        private readonly Pivot rootElement;

        public Minigame2Element(float x, float y, float width, float height, LabelStyle questionStyle, LabelStyle remainingVotesStyle, LabelStyle resultStyle, LabelStyle alertStyle, LabelStyle situationLabelStyle, ButtonStyle buttonStyle, ButtonStyle situationButtonStyle) {
            bounds = new Rectangle(x, y, width, height);
            this.questionStyle = questionStyle;
            this.remainingVotesStyle = remainingVotesStyle;
            this.resultStyle = resultStyle;
            this.alertStyle = alertStyle;
            this.situationLabelStyle = situationLabelStyle;
            this.buttonStyle = buttonStyle;
            this.situationButtonStyle = situationButtonStyle;

            rootElement = new Pivot();
            AddChild(rootElement);
            SetXY(x, y);
        }

        public void Deinitialize() {
            rootElement.GetChildren().ForEach(obj => obj.Destroy());
        }

        public void Initialize(int state) {
            Deinitialize();
            var activeMinigameCopy = NetworkManager.Instance.ActiveMinigame2;
            var isOwner = NetworkManager.Instance.PlayerData.GUID == activeMinigameCopy.Owner;
            var backgroundPath = "data/sprites/minigames/2/background";
            backgroundPath += (isOwner ? "_owner" : "") + ".png";
            var topOffset = isOwner ? 0f : 50;
            rootElement.AddChild(new Image(0, 0, bounds.width, bounds.height, new Sprite(backgroundPath, true, false)));

            // question
            questionLabel = new Label(0, 0, bounds.width, 100, activeMinigameCopy.ActiveQuestion.Question, questionStyle);
            rootElement.AddChild(questionLabel);
            if (state == 0) { // not voted
                situationAButton = new Button(20, 120 + topOffset, 520, 210, "s1", situationButtonStyle, () => {OnVoteClicked(activeMinigameCopy.ActiveQuestion.SituationA);});
                situationBButton = new Button(560, 120 + topOffset, 520, 210, "s2", situationButtonStyle, () => {OnVoteClicked(activeMinigameCopy.ActiveQuestion.SituationB);});
                situationALabel = new Label(20, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationA, situationLabelStyle);
                situationBLabel = new Label(560, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationB, situationLabelStyle);
                rootElement.AddChild(situationAButton);
                rootElement.AddChild(situationALabel);
                rootElement.AddChild(situationBButton);
                rootElement.AddChild(situationBLabel);
            } else if (state == 1) { // voted
                var votedSituation = activeMinigameCopy.ActiveQuestionVotes[NetworkManager.Instance.PlayerData.GUID];
                if (votedSituation == activeMinigameCopy.ActiveQuestion.SituationA) {
                    situationAButton = new Button(20, 120 + topOffset, 520, 210, "s1", situationButtonStyle);
                    situationALabel = new Label(20, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationA, situationLabelStyle);
                    rootElement.AddChild(situationAButton);
                    rootElement.AddChild(situationALabel);
                } else {
                    situationBButton = new Button(20, 120 + topOffset, 520, 210, "s2", situationButtonStyle);
                    situationBLabel = new Label(20, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationB, situationLabelStyle);
                    rootElement.AddChild(situationBButton);
                    rootElement.AddChild(situationBLabel);
                }
                var totalVotes = NetworkManager.Instance.ActiveRoom.Players.Count;
                var remainingVotes = activeMinigameCopy.RemainingVotes;
                var totalVoted = totalVotes - remainingVotes;
                remainingVotesLabel = new Label(bounds.width / 2f, bounds.height / 2f + topOffset, bounds.width / 2f, 170, $"Voted: {totalVoted}/{totalVotes} ({remainingVotes} remaining)", remainingVotesStyle);
                rootElement.AddChild(remainingVotesLabel);
            } else { // results
                var totalVotes = NetworkManager.Instance.ActiveRoom.Players.Count;
                var (winnerSituation, winnerVotes) = activeMinigameCopy.Result();
                if (winnerSituation == activeMinigameCopy.ActiveQuestion.SituationA) {
                    situationAButton = new Button(20, 120 + topOffset, 520, 210, "s1", situationButtonStyle);
                    situationALabel = new Label(20, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationA, situationLabelStyle);
                    rootElement.AddChild(situationAButton);
                    rootElement.AddChild(situationALabel);
                } else {
                    situationBButton = new Button(20, 120 + topOffset, 520, 210, "s2", situationButtonStyle);
                    situationBLabel = new Label(20, 120 + topOffset, 520, 210, activeMinigameCopy.ActiveQuestion.SituationB, situationLabelStyle);
                    rootElement.AddChild(situationBButton);
                    rootElement.AddChild(situationBLabel);
                }
                resultLabel = new Label(bounds.width / 2f, 100 + topOffset, bounds.width / 2f, 100, $"WINNER\nWith {winnerVotes}/{totalVotes} votes", resultStyle);
                rootElement.AddChild(resultLabel);
                if (!isOwner) {
                    rootElement.AddChild(new Label(bounds.width / 2f, 200 + topOffset, bounds.width / 2f, 150, "WAITING FOR CREATOR TO MOVE ON TO THE NEXT QUESTION", alertStyle));
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
            NetworkManager.Instance.NextQuestionMinigame2();
        }

        private void OnStopPlayingClicked() {
            NetworkManager.Instance.StopPlayingMinigame2();
        }

        private void OnVoteClicked(string situation) {
            NetworkManager.Instance.VoteMinigame2(situation);
            Initialize(1);
        }

        private static List<NetworkWouldYouRatherQuestion> questionList;
        private static bool isQuestionListInitialized;

        public static NetworkWouldYouRatherQuestion GetQuestion(int questionIndex) {
            if (!isQuestionListInitialized) InitializeQuestionList();
            return questionList[questionIndex];
        }

        private static void InitializeQuestionList() {
            questionList = new List<NetworkWouldYouRatherQuestion>();
            File.OpenText("data/wyr_questions.txt").ReadToEnd().Split('\n').ToList().ForEach(s => {
                var split = s.Split('#');
                var question = split[0].Capitalize();
                var situationA = split[1].Capitalize();
                var situationB = split[2].Capitalize();
                questionList.Add(new NetworkWouldYouRatherQuestion(question, situationA, situationB));
            });
            isQuestionListInitialized = true;
        }
    }
}