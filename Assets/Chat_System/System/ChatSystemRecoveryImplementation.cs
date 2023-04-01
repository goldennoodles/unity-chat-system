using System.Collections.Generic;
using System.Linq;
using Chat_System.Model;
using UnityEngine;

namespace Chat_System.System
{
    public class ChatSystemRecoveryImplementation
    {
        public void DoRecoveryOnExistingMessageID(
            ChatMessageDto chatMessageDto,
            ChatMessageSystem chatMessageSystem,
            int lastMessageID,
            string scene)
        {
            chatMessageSystem.AddChatMessage(scene, new ChatMessageDto(
                (lastMessageID + 1),
                chatMessageDto.GetScene,
                chatMessageDto.GetMessage,
                chatMessageDto.IsSkippable));
            
            Debug.Log("Recovery Successful");
        }
    }
}