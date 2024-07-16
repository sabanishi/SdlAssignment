using System;
using System.Collections.Generic;

namespace Sabanishi.SdiAssignment
{
    [Serializable]
    public class ChatGPTMessageModel
    {
        public string role;
        public string content;
    }
    
    [Serializable]
    public class ChatGPTCompletionRequestModel
    {
        public string model;
        public List<ChatGPTMessageModel> messages;
        public int max_tokens;
        public float temperature;
    }

    public class ChatGPTResponceModel
    {
        public string id;
        public string @object;
        public int created;
        public Choice[] choices;
        public Usage usage;
        
        [Serializable]
        public class Choice
        {
            public int index;
            public ChatGPTMessageModel message;
            public string finish_reason;
        }

        [Serializable]
        public class Usage
        {
            public int prompt_tokens;
            public int completion_tokens;
            public int total_tokens;
        }
    }
}