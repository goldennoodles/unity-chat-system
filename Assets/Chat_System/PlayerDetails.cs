using System;
using UnityEngine;

namespace Chat_System
{
    public class PlayerDetails : MonoBehaviour
    {
        public GameObject chatHolderUI;
        public GameObject playerDetailsUI;
        
        private ChatSystemManager _chatSystem;

        public static event EventHandler<string> OnPlayerNameChange;
        private void Start()
        {
            _chatSystem = GetComponent<ChatSystemManager>();
        }
        
        public void ReadUserNameInput(string playerName)
        {
            OnPlayerNameChange?.Invoke(this, playerName);
            SetObjectState();
        }

        private void SetObjectState()
        {
            chatHolderUI.SetActive(true);
            playerDetailsUI.SetActive(false);
        }

    }
}