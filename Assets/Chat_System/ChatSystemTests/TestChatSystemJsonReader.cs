using System.Collections.Generic;
using Chat_System.Model;
using Chat_System.System;
using NUnit.Framework;

namespace Chat_System.ChatSystemTests
{
    public class TestChatSystemJsonReader
    {
        [Test]
        public void TestJsonReader()
        {
            ChatSystemJsonReader chatSystemJsonReader = new ChatSystemJsonReader("ChatMessageTests");

            Dictionary<string, List<ChatMessageDto>> chatMessages =
                chatSystemJsonReader.PopulateDictionaryWithChatMessagesFromJson();

            Assert.Pass();
        }
    }
}