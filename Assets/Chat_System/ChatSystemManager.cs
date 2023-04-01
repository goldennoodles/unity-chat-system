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

            Debug.Log(
                $"ChatMessageID: {GetMessage(chatMessageIndex).GetOrderID},Scene: {GetMessage(chatMessageIndex).GetScene}, Image Path {GetMessage(chatMessageIndex).GetPicturePath}");
            messageHolder.text = $"{chatMessage.GetMessage}";
            
            if (chatMessage.GetPicturePath == null)
                rawImage.SetActive(false);
            else
                rawImage.GetComponent<RawImage>().texture =
                    _chatMessageSystem.LoadImage(_chatMessageSystem.RetrieveChatMessage(1, Scene.sceneA));
            
            if (chatMessage.GetName == null)
                nameTag.SetActive(false);
            else
                nameTag.GetComponent<TextMeshProUGUI>().text = chatMessage.GetName;
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (chatMessageIndex >= _chatMessages.Count - 1)
                {
                    Debug.Log("End Of Scene");
                    return;
                }
                
                ChatMessageDto chatMessage = GetMessage(++chatMessageIndex);
                
                if (chatMessage.GetPicturePath == null)
                    rawImage.SetActive(false);
                else
                    rawImage.GetComponent<RawImage>().texture =
                        _chatMessageSystem.LoadImage(_chatMessageSystem.RetrieveChatMessage(chatMessageIndex, Scene.sceneA));
            
                if (chatMessage.GetName == null)
                    nameTag.SetActive(false);
                else
                    nameTag.GetComponent<TextMeshProUGUI>().text = chatMessage.GetName;
                
            }
        }

        private ChatMessageDto GetMessage(int index)
        {
            return _chatMessages[index];
        }
    }
}