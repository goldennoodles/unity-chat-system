using System.Collections.Generic;
using Chat_System.Model;
using Chat_System.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chat_System
{
    public class ChatSystemManager : MonoBehaviour
    {
        public TextMeshProUGUI messageHolder;
        public GameObject rawImage;
        public GameObject nameTag;

        private List<ChatMessageDto> _chatMessages;
        private ChatMessageSystem _chatMessageSystem;
        private int chatMessageIndex = 0;

        private void Start()
        {
            _chatMessageSystem = new ChatMessageSystem(new ChatSystemJsonReader("ChatMessage"));
            _chatMessages = _chatMessageSystem.RetrieveAllChatMessagesInOrderForScene(Scene.sceneA);

            ChatMessageDto chatMessage = GetMessage(chatMessageIndex);
            messageHolder.text = $"{chatMessage.GetMessage}";
            
            CheckDisplayUnits(chatMessage);
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

        private bool isEndOfScene()
        {
            if (chatMessageIndex >= _chatMessages.Count - 1)
            {
                Debug.Log("End Of Scene");
                return true;
            }

            return false;
        }

        private void IncrementAndShowNextMessage()
        {
            if (isEndOfScene()) return;
            
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