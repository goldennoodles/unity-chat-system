using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands;
using Codice.CM.SemanticMerge.Gui;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace Chat_System.Model
{
    public record ChatMessageDto
    {
        private int _orderID;
        private Scene _scene;

        private string _name;
        private string _message;
        private string _picturePath;
        private bool _skipAble;

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
        public string GetPicturePath => _picturePath;
        public string GetMessage => _message;
        public bool IsSkippable => _skipAble;
    }
}