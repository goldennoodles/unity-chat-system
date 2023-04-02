using System;
using UnityEngine;

namespace Chat_System.Model
{
    [Serializable]
    public record ChatMessageDto
    {
        [SerializeField] private int _orderID;
        [SerializeField] private Scene _scene;

        [SerializeField] private string _name;
        [SerializeField] private string _message;
        [SerializeField] private string _picturePath;
        [SerializeField] private bool _skipAble;

        public ChatMessageDto(int orderID, Scene scene, string message, bool skipAble)
        {
            this._orderID = orderID;
            this._scene = scene;
            this._message = message;
            this._skipAble = skipAble;
        }

        public ChatMessageDto(int order, Scene scene, string message, bool skipAble, string name) :
            this(order, scene, message, skipAble)
        {
            this._name = name;
        }

        public ChatMessageDto(
            int orderID, Scene scene, string message,
            bool skipAble, string name, string picturePath) : this(orderID, scene, message, skipAble)
        {
            this._name = name;
            this._picturePath = picturePath;
        }

        public int GetOrderID => _orderID;
        public Scene GetScene => _scene;
        public string GetName => _name;
        public void SetName(string name) => _name = name;
        public string GetPicturePath => _picturePath;
        public string GetMessage => _message;
        public bool IsSkippable => _skipAble;
    }
}