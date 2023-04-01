using System;
using UnityEngine;

namespace Chat_System.Exceptions
{
    public class ChatMessageMissingDataException : Exception
    {
        public ChatMessageMissingDataException(string message) : base(message) {}

        public ChatMessageMissingDataException()
        {
            Debug.LogError("Unable To Add Message As; Missing Data In Chat Message");
        }
    }
}