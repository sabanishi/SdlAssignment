using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class ChatUser
    {
        private const string FaceTagPattern = @"\[face:([^\]_]+)_?(\d*)\]";
        private const string InterestTagPattern = @"\[interest:(\d)\]";
        private const string InitialMessage = "こんにちは！";
        
        private string _apiKey;
        private ChatGPTConnection _connection;

        public ChatUser()
        {
        }

        public async UniTask<bool> TryRequest(string userMessage)
        {
            if (_connection == null)
            {
                if (!TryLoadData() || _connection == null)
                {
                    await UniTask.Delay(500);
                    Outputter.Instance.Output("ChatGPTのAPIキーが設定されていないみたい...");
                    await UniTask.Delay(300);
                    Outputter.Instance.Output("先に「!setup_chat」コマンドでAPIキーを設定してね！");
                    return false;
                }
            }

            //APIキーが変更された場合、ChatGPTConnectionを再設定する
            if (SaveUtils.TryLoad<ApiData>(out var data))
            {
                if (!data.apiKey.Equals(_apiKey))
                {
                    SetupConnection(data.apiKey);
                }
            }
            
            var result = await _connection.RequestAsync(userMessage);

            var response = result.choices[0].message.content;
            
            //表情タグ、関心レベルタグを削除する
            var cleanText = ExtractTags(ref response, out var interestLevel);
            
            Outputter.Instance.Output(cleanText);
            
            return true;
        }

        /// <summary>
        /// 応答から表情タグと関心レベルタグを抽出し、クリーンなテキストを返す
        /// </summary>
        /// <param name="input"></param>
        /// <param name="interestLevel"></param>
        /// <returns></returns>
        private string ExtractTags(ref string input, out int interestLevel)
        {
            interestLevel = -1;
            var uniqueTags = new HashSet<string>();
            
            //関心レベルタグを抽出
            var interestMatch = Regex.Match(input, InterestTagPattern);
            if (interestMatch.Success)
            {
                interestLevel = int.Parse(interestMatch.Groups[1].Value);
                input = Regex.Replace(input, InterestTagPattern, "");
            }
            
            //表情タグを抽出
            var matches = Regex.Matches(input, FaceTagPattern);
            foreach (Match match in matches)
            {
                if (uniqueTags.Add(match.Value))
                {
                    string emotionTag = match.Groups[1].Value;
                    string emotionIntensityString = match.Groups[2].Value;
                    
                    if (int.TryParse(emotionIntensityString, out int emotionIntensity))
                    {
                        //表情の強度を取得
                        Debug.Log($"表情: {emotionTag}, 強度: {emotionIntensity}");
                    }
                }
            }
            
            //応答から表情タグを削除してクリーンなテキストを生成
            input = Regex.Replace(input, FaceTagPattern, "");
            
            //関心レベルが0の場合、応答を括弧で囲んで特殊な扱いを示す
            if (interestLevel == 0)
            {
                input = $"({input})";
            }
            
            return input;
        }

        private bool TryLoadData()
        {
            if (SaveUtils.TryLoad<ApiData>(out var data))
            {
                SetupConnection(data.apiKey);
                return true;
            }

            return false;
        }

        /// <summary>
        /// APIキーを設定し、ChatGPTConnectionを初期化する
        /// </summary>
        private void SetupConnection(string apiKey)
        {
            _apiKey = apiKey;
            _connection = new ChatGPTConnection(
                apiKey: apiKey,
                initialMessage: InitialMessage,
                modelVersion: "gpt-4o",
                maxTokens: 100,
                temperature: 0.5f);
        }
    }
}