using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Sabanishi.SdiAssignment
{
    public class ChatGPTConnection
    {
        private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
        
        private readonly string _apiKey;
        private readonly List<ChatGPTMessageModel> _messageList;
        private readonly string _modelVersion;
        private readonly int _maxTokens;
        private readonly float _temperature;
        
        public ChatGPTConnection(string apiKey, string initialMessage, string modelVersion, int maxTokens, float temperature) {
            _apiKey = apiKey;
            _messageList = new List<ChatGPTMessageModel> {
                new ChatGPTMessageModel { role = "system", content = initialMessage }
            };
            _modelVersion = modelVersion;
            _maxTokens = maxTokens;
            _temperature = temperature;
        }

        public async UniTask<ChatGPTResponceModel> RequestAsync(string userMessage)
        {
            _messageList.Add(new ChatGPTMessageModel(){role="user",content = userMessage});

            var headers = new Dictionary<string, string>()
            {
                { "Authorization", $"Bearer {_apiKey}" },
                { "Content-type", "application/json" },
                { "X-Slack-No-Retry", "1" }
            };
            
            var options = new ChatGPTCompletionRequestModel
            {
                model = _modelVersion,
                messages = _messageList,
                max_tokens = _maxTokens,
                temperature = _temperature
            };
            
            var jsonOptions = JsonUtility.ToJson(options);

            using var request = new UnityWebRequest(ApiUrl, "POST")
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };

            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key,header.Value);
            }

            try
            {
                await request.SendWebRequest();
                if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception(request.error);
                }

                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<ChatGPTResponceModel>(responseString);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
            catch (Exception e)
            {
                //401エラーの時
                if (e.Message.Contains("401"))
                {
                    await UniTask.Delay(500);
                    Outputter.Instance.Output("APIキーが間違っているみたいだね...");
                }
            }

            return null;
        }
    }
}