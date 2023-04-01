using System;
using Chat_System.Model;
using UnityEngine;

namespace Chat_System.Exceptions
{
    public class ChatMessageInvalidException : Exception
    {
        public ChatMessageInvalidException(ChatMessageDto chatMessageDto)
        {
            Debug.LogError($"ChatMessage with ID: {chatMessageDto.GetOrderID}, already exists, attempting to recover");
        }

        public ChatMessageInvalidException(int id)
        {
            Debug.LogError($"ChatMessage with ID: {id}, already exists, attempting to recover");

        }
    }
}