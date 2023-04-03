using System.Collections.Generic;
using Chat_System.Exceptions;
using Chat_System.Model;
using Chat_System.System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Chat_System.ChatSystemTests
{
    public class TestChatSystemImplementation
    {
        private ChatMessageSystem _chatMessageSystem;
        
        [SetUp]
        public void Setup()
        {
            _chatMessageSystem = new ChatMessageSystem(new ChatSystemJsonReader("Test/ChatMessagesTest"));
        }
        
        [Test]
        public void OnSuccessMessageAddition()
        {
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(2, Scene.sceneA, "This is a test", false));
            ChatMessageDto chatMessage = _chatMessageSystem.RetrieveChatMessage(2, Scene.sceneA);
            
            Assert.AreEqual(chatMessage.GetMessage, "This is a test");
        }
        
        [Test]
        public void OnFailureMessageAddition()
        {
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "This is a test", false));
            ChatMessageDto chatMessage = _chatMessageSystem.RetrieveChatMessage(2, Scene.sceneA);
            
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 1, already exists, attempting to recover");
            Assert.AreEqual("This is a test",chatMessage.GetMessage);
        }

        [Test]
        public void OnNegativeIDInDTOException()
        {
            try
            {
                _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(-1, Scene.sceneA, "This is a test", false));
            }
            catch (ChatMessageMissingDataException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        
        [Test]
        public void OnMissingMessageInDTOException()
        {
            try
            {
                _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "", false));
            }
            catch (ChatMessageMissingDataException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void OnEmptyMessageListException()
        {
            try
            {
                _chatMessageSystem.RetrieveChatMessage(2, Scene.sceneA);
            }
            catch (ChatMessageNotFoundException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        
        [Test]
        public void OnMessageNotFoundException()
        {
            try
            {
                _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "This is a test", false));
                _chatMessageSystem.RetrieveChatMessage(3, Scene.sceneA);
            }
            catch (ChatMessageNotFoundException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void OnSameMessageIDAlreadyExists()
        {
            _chatMessageSystem.AddChatMessage("sceneA", new ChatMessageDto(1, Scene.sceneA, "some message1", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "some message3", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "some message2", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "some message4", false));
            
            List<ChatMessageDto> orderedList = _chatMessageSystem.RetrieveAllChatMessagesInOrderForScene(Scene.sceneA);
            
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 1, already exists, attempting to recover");
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 1, already exists, attempting to recover");
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 1, already exists, attempting to recover");
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 1, already exists, attempting to recover");
            
            
            orderedList.ForEach(e=> Debug.Log(e.ToString()));
            Assert.True(orderedList[0].GetMessage == "\"this is a test message for sceneA\"", "Message Index 1");
            Assert.True(orderedList[1].GetMessage == "some message1");
            Assert.True(orderedList[2].GetMessage == "some message3");
            Assert.True(orderedList[3].GetMessage == "some message2");
        }
        
        [Test]
        public void OnSameMessageIDAlreadyExistsOrdered()
        {
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "some message1", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(3, Scene.sceneA, "some message3", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(3, Scene.sceneA, "some message2", false));
            _chatMessageSystem.AddChatMessage("sceneA",new ChatMessageDto(1, Scene.sceneA, "some message4", false));
            
            List<ChatMessageDto> orderedList = _chatMessageSystem.RetrieveAllChatMessagesInOrderForScene(Scene.sceneA);

            LogAssert.Expect(LogType.Error,"ChatMessage with ID: 1, already exists, attempting to recover");
            LogAssert.Expect(LogType.Error, "ChatMessage with ID: 3, already exists, attempting to recover");
            LogAssert.Expect(LogType.Error,"ChatMessage with ID: 1, already exists, attempting to recover");
            
            Assert.True(orderedList[0].GetMessage == "\"this is a test message for sceneA\"", "Message Index 1");
            Assert.True(orderedList[1].GetMessage == "some message1", "Message Index 1");
            Assert.True(orderedList[2].GetMessage == "some message3", "Message Index 1");
            Assert.True(orderedList[3].GetMessage == "some message2", "Message Index 1");
        }
    }
}
