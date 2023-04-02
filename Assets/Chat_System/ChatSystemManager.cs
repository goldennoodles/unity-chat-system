using System;
using System.Collections.Generic;
using System.Linq;
using Chat_System.Model;
using Chat_System.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chat_System
{
    [RequireComponent(typeof(PlayerDetails))]
    public class ChatSystemManager : MonoBehaviour
    {
        public TextMeshProUGUI messageHolder;
        public GameObject rawImage;
        public GameObject nameTag;

        public List<ChatMessageDto> _chatMessages;
        private ChatMessageSystem _chatMessageSystem;
        private int chatMessageIndex = 0;
        
        public static event EventHandler OnEndOfScene;

        private void Start()
        {

            _chatMessageSystem = new ChatMessageSystem(new ChatSystemJsonReader("ChatMessage"));
            _chatMessages = _chatMessageSystem.RetrieveAllChatMessagesInOrderForScene(Scene.sceneA);
            
            var chatMessage = GetMessage(chatMessageIndex);
            
            PlayerDetails.OnPlayerNameChange += (sender, playerName) =>
            {
                ChangePlayerName(playerName);
                CheckDisplayUnits(chatMessage);
                messageHolder.text = $"{chatMessage.GetMessage}";
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IncrementAndShowNextMessage();
            }
        }

        private void CheckDisplayUnits(ChatMessageDto chatMessage)
        {
            if (chatMessage.GetPicturePath == null)
                rawImage.SetActive(false);
            else
                rawImage.GetComponent<RawImage>().texture =
                    _chatMessageSystem.LoadImage(chatMessage);
            
            if (chatMessage.GetName == null)
                nameTag.SetActive(false);
            else
                nameTag.GetComponent<TextMeshProUGUI>().text = chatMessage.GetName;

        }
        
        public void OnNextButtonClick()
        {
            IncrementAndShowNextMessage();
        }

        private void ChangePlayerName(string playerName)
        {
            var i = 0;
            foreach (var chatMessage in _chatMessages.Where(e => e.GetName == "player"))
            {
                i++;
                chatMessage.SetName(playerName);
            }

            Debug.Log($"Changed: {i} objects, to have name: {playerName}");
        }

        private bool IsEndOfScene()
        {
            if (chatMessageIndex < _chatMessages.Count - 1) return false;
            Debug.Log("End Of Scene");
            OnEndOfScene?.Invoke(this, EventArgs.Empty);
            return true;

        }

        private void IncrementAndShowNextMessage()
        {
            if (IsEndOfScene()) return;
            
            ++chatMessageIndex;
                
            ChatMessageDto chatMessage = GetMessage(chatMessageIndex);
            CheckDisplayUnits(chatMessage);
            
            messageHolder.text = chatMessage.GetMessage;
        }

        private ChatMessageDto GetMessage(int index)
        {
            return _chatMessages[index];
        }
    }
}