using System;
using Chat_System.Model;
using UnityEngine;

namespace Chat_System.Exceptions
{
    public class ChatMessageNotFoundException : Exception
    {
        public ChatMessageNotFoundException(string message) : base(message) {}

        public ChatMessageNotFoundException(int messageId, Scene scene)
        {
            Debug.LogError($"Chat Message Not found with id: {messageId}, for scene: {scene}");
        }
    }
}