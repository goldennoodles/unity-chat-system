using System;
using System.Collections.Generic;
using System.Linq;
using Chat_System.Model;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace Chat_System.System
{
    public class ChatSystemJsonReader
    {
        private readonly string _fileName;

        public ChatSystemJsonReader(string fileName)
        {
            this._fileName = fileName;
        }

        private JArray GetJsonArray()
        {
            string filePath = $"ChatSystem/{_fileName}";
            String json = Resources.Load<TextAsset>(filePath).ToString();

            JObject jsonConverted = JObject.Parse(json);
            
            return JArray.Parse(jsonConverted["chatMessage"]?["scenes"]!.ToString() ?? string.Empty);
        }

        public Dictionary<string, List<ChatMessageDto>> PopulateDictionaryWithChatMessagesFromJson()
        {
            JArray scenesArray = GetJsonArray();

            var fullMessageList = 
                    scenesArray.Children<JObject>()
                        .Select(rootScenes => rootScenes.Properties()
                            .Select(p => p.Name)
                            .ToList())
                        .SelectMany(sceneNamesList => sceneNamesList)
                        .ToDictionary(scene => scene, scene => 
                            GrabSceneSpecificDataFromJson(scene, scenesArray, (Scene) Enum.Parse(typeof(Scene), scene)));

            Debug.Log("Chat Messages Retrieved, and saved.");

            return fullMessageList;
        }

        private List<ChatMessageDto> GrabSceneSpecificDataFromJson(string sceneName, JArray scenes, Scene scene)
        {
            int index = 1;
            int count = (scenes[0][sceneName]?[0] ?? 0).Count();

            List<ChatMessageDto> chatMessageDtos = new List<ChatMessageDto>();

            for (int i = 0; i < count; i++)
            {
                try
                {
                    foreach (var xScene in scenes[0][sceneName]![i]?.Children().Values()!)
                    {
                        String message = "\"" + xScene.Value<string>("message") + "\"";
                        bool isSkippable = xScene.Value<bool>("skip-able");

                        string name = xScene.Value<string>("name");
                        string photoPath = xScene.Value<string>("photoPath");

                        if(name != null && photoPath == null)
                            chatMessageDtos.Add(new ChatMessageDto(index++, scene, message, isSkippable,name));
                        else if(photoPath != null)
                            chatMessageDtos.Add(new ChatMessageDto(index++, scene, message, isSkippable, name, photoPath));
                        else
                            chatMessageDtos.Add(new ChatMessageDto(index++, scene, message, isSkippable));

                    }
                }
                catch (ArgumentException)
                {
                }
            }

            return chatMessageDtos;
        }
    }
}