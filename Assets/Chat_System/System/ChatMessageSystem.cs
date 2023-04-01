using System.Collections.Generic;
using System.Linq;
using Chat_System.Exceptions;
using Chat_System.Model;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UI;

namespace Chat_System.System
{
    public class ChatMessageSystem
    {
        private readonly Dictionary<string, List<ChatMessageDto>> _chatMessages;
        private readonly ChatSystemRecoveryImplementation _chatSystemRecoveryImplementation = new();

        public ChatMessageSystem(ChatSystemJsonReader jsonReader)
        {
            _chatMessages = jsonReader.PopulateDictionaryWithChatMessagesFromJson();
        }

        public void AddChatMessage(string scene, ChatMessageDto chatMessageDto)
        {

            if (!CheckChatMessageValidity(chatMessageDto, scene) &&
                _chatMessages.Values.Any(e=> e.Contains(chatMessageDto)))
                return;

            DuplicatedIDVerification(chatMessageDto, scene);
            
            Debug.Log(
                $"Adding Message With MessageIndex: {chatMessageDto.GetOrderID}, and Scene {scene}, to cache");
        }

        public ChatMessageDto RetrieveChatMessage(int messageID, Scene scene)
        {
            if (_chatMessages.Count == 0) throw new ChatMessageNotFoundException(messageID, scene);

            // Onioning, Loop through scene, then loop through list, then pull out dto.
            foreach (var unused in _chatMessages.Keys)
            {
                foreach (var chatMessageDto in _chatMessages.Values
                             .SelectMany(
                                 chatMessagesValue => chatMessagesValue
                                     .Where(cM => cM.GetOrderID == messageID && cM.GetScene == scene)
                             )
                        )
                {
                    return chatMessageDto;
                }
            }

            throw new ChatMessageNotFoundException(messageID, scene);
        }

        public List<ChatMessageDto> RetrieveAllChatMessagesInOrderForScene(Scene scene)
        {
            foreach (var unused in _chatMessages.Keys)
            {
                foreach (var chatMessages in _chatMessages.Values)
                {
                    return chatMessages
                        .Where(e => e.GetScene == scene)
                        .OrderBy((e => e.GetOrderID))
                        .ToList();
                }
            }

            throw new ChatMessageNotFoundException("Unable to retrieve ChatMessages, No Data Found");
        }

        public Texture LoadImage(ChatMessageDto chatMessageDto)
        {
            string pathRoot = "ChatSystem/ChatPictures/" + chatMessageDto.GetPicturePath;
            return Resources.Load<Texture2D>(pathRoot);
        }

        private void DuplicatedIDVerification(ChatMessageDto chatMessageDto, string scene)
        {
            List<ChatMessageDto> chatMessages = _chatMessages[scene];

            int lastIndex = GetMessageIdOfLastMessageForScene(chatMessageDto.GetScene);
            try
            {
                if (_chatMessages[scene].Any(e => e.GetOrderID == chatMessageDto.GetOrderID))
                    throw new ChatMessageInvalidException(chatMessageDto);
                
                chatMessages.Add(chatMessageDto);
            }
            catch (ChatMessageInvalidException)
            {
                _chatSystemRecoveryImplementation.DoRecoveryOnExistingMessageID(
                    chatMessageDto,
                    this,
                    lastIndex, scene);
            }

            _chatMessages[scene] = chatMessages;
        }

        private bool CheckChatMessageValidity(ChatMessageDto chatMessageDto, string scene)
        {
            if (chatMessageDto.GetOrderID < 0 || chatMessageDto.GetMessage == "")
                throw new ChatMessageMissingDataException();

            return true;
        }

        private int GetMessageIdOfLastMessageForScene(Scene scene)
        {
            return RetrieveAllChatMessagesInOrderForScene(scene).Last().GetOrderID;
        }

        public void DebugScene(Scene scene)
        {
            _chatMessages[scene.ToString()].ForEach(e => Debug.Log(e.ToString()));
        }
    }
}