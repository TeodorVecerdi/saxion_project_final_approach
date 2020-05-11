using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using GXPEngine;
using Rectangle = GXPEngine.Core.Rectangle;

namespace game.ui.Custom {
    public class ChatElement : EasyDraw {
        public static ChatElement ActiveChat = null;
        
        public Action<ChatMessage> OnMessageSent;
        public Action<ChatMessage> OnMessageReceived;

        private readonly Rectangle bounds;
        private LabelStyle messageStyle;
        private Stack<ChatMessage> chatMessages;

        private TextField messageInputTextField;

        public ChatElement(float x, float y, float width, float height, Action<ChatMessage> onMessageSent = null, Action<ChatMessage> onMessageReceived = null, Action<string, string> textFieldOnValueChanged = null, Action<int> textFieldOnKeyTyped = null, Action<int> textFieldOnKeyRepeat = null, Action textFieldOnGainFocus = null, Action textFieldOnLoseFocus = null, Action textFieldOnMouseClick = null, Action textFieldOnMouseEnter = null, Action textFieldOnMouseLeave = null, Action textFieldOnMousePress = null, Action textFieldOnMouseRelease = null)
            : this(x, y, width, height, TextFieldStyle.Default, LabelStyle.DefaultChat, onMessageSent, onMessageReceived, textFieldOnValueChanged, textFieldOnKeyTyped, textFieldOnKeyRepeat, textFieldOnGainFocus, textFieldOnLoseFocus, textFieldOnMouseClick, textFieldOnMouseEnter, textFieldOnMouseLeave, textFieldOnMousePress, textFieldOnMouseRelease) { }

        public ChatElement(float x, float y, float width, float height, TextFieldStyle textFieldStyle, Action<ChatMessage> onMessageSent = null, Action<ChatMessage> onMessageReceived = null,  Action<string, string> textFieldOnValueChanged = null, Action<int> textFieldOnKeyTyped = null, Action<int> textFieldOnKeyRepeat = null, Action textFieldOnGainFocus = null, Action textFieldOnLoseFocus = null, Action textFieldOnMouseClick = null, Action textFieldOnMouseEnter = null, Action textFieldOnMouseLeave = null, Action textFieldOnMousePress = null, Action textFieldOnMouseRelease = null)
            : this(x, y, width, height, textFieldStyle, LabelStyle.DefaultChat, onMessageSent, onMessageReceived, textFieldOnValueChanged, textFieldOnKeyTyped, textFieldOnKeyRepeat, textFieldOnGainFocus, textFieldOnLoseFocus, textFieldOnMouseClick, textFieldOnMouseEnter, textFieldOnMouseLeave, textFieldOnMousePress, textFieldOnMouseRelease) { }

        public ChatElement(float x, float y, float width, float height, LabelStyle messageStyle, Action<ChatMessage> onMessageSent = null, Action<ChatMessage> onMessageReceived = null, Action<string, string> textFieldOnValueChanged = null, Action<int> textFieldOnKeyTyped = null, Action<int> textFieldOnKeyRepeat = null, Action textFieldOnGainFocus = null, Action textFieldOnLoseFocus = null, Action textFieldOnMouseClick = null, Action textFieldOnMouseEnter = null, Action textFieldOnMouseLeave = null, Action textFieldOnMousePress = null, Action textFieldOnMouseRelease = null)
            : this(x, y, width, height, TextFieldStyle.Default, messageStyle, onMessageSent, onMessageReceived, textFieldOnValueChanged, textFieldOnKeyTyped, textFieldOnKeyRepeat, textFieldOnGainFocus, textFieldOnLoseFocus, textFieldOnMouseClick, textFieldOnMouseEnter, textFieldOnMouseLeave, textFieldOnMousePress, textFieldOnMouseRelease) { }

        public ChatElement(float x, float y, float width, float height, TextFieldStyle textFieldStyle, LabelStyle messageStyle, Action<ChatMessage> onMessageSent = null, Action<ChatMessage> onMessageReceived = null, Action<string, string> textFieldOnValueChanged = null, Action<int> textFieldOnKeyTyped = null, Action<int> textFieldOnKeyRepeat = null, Action textFieldOnGainFocus = null, Action textFieldOnLoseFocus = null, Action textFieldOnMouseClick = null, Action textFieldOnMouseEnter = null, Action textFieldOnMouseLeave = null, Action textFieldOnMousePress = null, Action textFieldOnMouseRelease = null)
            : base(Mathf.Ceiling(width), Mathf.Ceiling(height), false) {
            bounds = new Rectangle(x, y, width, height);
            this.messageStyle = messageStyle;
            OnMessageSent += onMessageSent;
            OnMessageReceived += onMessageReceived;
            chatMessages = new Stack<ChatMessage>();
            
            textFieldOnKeyTyped += key => {
                if (key == Key.ENTER && !string.IsNullOrEmpty(messageInputTextField.Text)) {
                    SendMessage();
                }
            };
            messageInputTextField = new TextField(0, height - 40f, width, 40f, "send message", textFieldStyle, textFieldOnValueChanged, textFieldOnKeyTyped, textFieldOnKeyRepeat, textFieldOnGainFocus, textFieldOnLoseFocus, textFieldOnMouseClick, textFieldOnMouseEnter, textFieldOnMouseLeave, textFieldOnMousePress, textFieldOnMouseRelease);
            AddChild(messageInputTextField);
            SetXY(x, y);
            Draw();
        }

        private void SendMessage() {
            var messageText = messageInputTextField.Text;
            messageInputTextField.Text = "";
            var chatMessage = new ChatMessage(NetworkManager.Instance.PlayerData.Username, NetworkManager.Instance.PlayerData.GUID, messageText);
            chatMessages.Push(chatMessage);
            OnMessageSent?.Invoke(chatMessage);
            NetworkManager.Instance.SendMessage(chatMessage);
            Draw();
        }

        public void ReceiveMessage(ChatMessage message) {
            OnMessageReceived?.Invoke(message);
            chatMessages.Push(message);
            Draw();
        }

        private void Draw() {
            const float messageHeight = 20f;
            
            Clear(Color.Transparent);
            NoStroke();
            Fill(Color.Bisque, 204);
            ShapeAlign(CenterMode.Min, CenterMode.Min);
            Rect(0, 0, bounds.width, bounds.height);
            
            var currentMessageHeight = bounds.height - 40f - messageHeight * 1.5f;
            Fill(messageStyle.TextColor);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            foreach (var chatMessage in chatMessages) {
                var stringMessage = $"{chatMessage.SenderName}: {chatMessage.Message}";
                var textX = 0f;
                var textY = currentMessageHeight;
                if (messageStyle.TextAlignment.Alignment == StringAlignment.Far)
                    textX += bounds.width;
                else if (messageStyle.TextAlignment.Alignment == StringAlignment.Center) textX += bounds.width / 2;
                if (messageStyle.TextAlignment.LineAlignment == StringAlignment.Far)
                    textY += messageHeight;
                else if (messageStyle.TextAlignment.LineAlignment == StringAlignment.Center) textY += messageHeight / 2;
                graphics.DrawString(stringMessage, messageStyle.Font, brush, new RectangleF(textX, textY, bounds.width, messageHeight), messageStyle.TextAlignment);
                currentMessageHeight -= messageHeight;
                
                if(currentMessageHeight < 0) 
                    break;
            }
        }
    }
}