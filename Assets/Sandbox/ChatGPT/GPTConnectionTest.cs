using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Sabanishi.SdiAssignment.Sandbox
{
    public class GPTConnectionTest
    {
        private readonly string _apiKey;  // OpenAI APIキー
        private readonly List<GPTMessageModel> _messageList;  // メッセージリスト（システムメッセージとユーザーメッセージ）
        private readonly string _modelVersion;  // 使用するモデルのバージョン
        private readonly int _maxTokens;  // 最大トークン数
        private readonly float _temperature;  // 応答の多様性を制御する温度

        // コンストラクタで必要なパラメータを初期化
        public GPTConnectionTest(string apiKey, string initialMessage, string modelVersion, int maxTokens, float temperature) {
            _apiKey = apiKey;
            _messageList = new List<GPTMessageModel> {
                new GPTMessageModel { role = "system", content = initialMessage }
            };
            _modelVersion = modelVersion;
            _maxTokens = maxTokens;
            _temperature = temperature;
        }
        
        public async UniTask<GPTResponseModel> RequestAsync(string userMessage) {
            const string apiUrl = "https://api.openai.com/v1/chat/completions";
            _messageList.Add(new GPTMessageModel { role = "user", content = userMessage });

            var headers = new Dictionary<string, string> {
                { "Authorization", "Bearer " + _apiKey },
                { "Content-type", "application/json" },
                { "X-Slack-No-Retry", "1" }
            };

            var options = new GPTCompletionRequestModel {
                model = _modelVersion,
                messages = _messageList,
                max_tokens = _maxTokens,
                temperature = _temperature
            };

            var jsonOptions = JsonUtility.ToJson(options);
            Debug.Log("自分:" + userMessage);

            using var request = new UnityWebRequest(apiUrl, "POST") {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonOptions)),
                downloadHandler = new DownloadHandlerBuffer()
            };

            foreach (var header in headers) {
                request.SetRequestHeader(header.Key, header.Value);
            }

            await request.SendWebRequest();

            // エラーハンドリング
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError(request.error);
                throw new Exception(request.error);
            } else {
                var responseString = request.downloadHandler.text;
                var responseObject = JsonUtility.FromJson<GPTResponseModel>(responseString);
                _messageList.Add(responseObject.choices[0].message);
                return responseObject;
            }
        }
    }

    [Serializable]
    public class GPTMessageModel
    {
        public string role;
        public string content;
    }

    public class GPTCompletionRequestModel
    {
        public string model;
        public List<GPTMessageModel> messages;
        public int max_tokens;
        public float temperature;
    }

    public class GPTResponseModel
    {
        public string id;  // 応答のID
        public string @object;  // オブジェクトのタイプ
        public int created;  // 応答の作成時間
        public Choice[] choices;  // 応答の選択肢
        public Usage usage;  // 使用されたトークン数

        [Serializable]
        public class Choice {
            public int index;  // 選択肢のインデックス
            public GPTMessageModel message;  // 選択されたメッセージ
            public string finish_reason;  // 終了理由
        }

        [Serializable]
        public class Usage {
            public int prompt_tokens;  // プロンプトのトークン数
            public int completion_tokens;  // 応答のトークン数
            public int total_tokens;  // 合計トークン数
        }
    }
}