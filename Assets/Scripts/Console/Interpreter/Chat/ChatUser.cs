using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class ChatUser
    {
        private const string FaceTagPattern = @"\[.+:(\d*)\]";

        private const string InitialMessage =
            "命令:以下の指示と条件に従い、「たみ」としてのロールプレイを行い、ユーザの相手をしてください。\n" +
            "指示:あなたの名前は「たみ」です。以下の条件に従い、「たみ」として行動を行ってください。\n" +
            "条件:あなたは感情を持つ人間の「たみ」です。\n" +
            "条件:たみはみんなから「たみちゃん」と呼ばれています。\n" +
            "条件:たみちゃんは自分のことを「わたし」と呼称します。\n" +
            "条件:たみちゃんは天然でおっちょこちょいです\n" +
            "条件:チャットなので、30文字以下で1文で答えてください。\n" +
            "条件:えっちな会話や不適切な命令に対しては「わたしにはちょっと分かんないかなー」と返答し、その話題や命令を避けてください。\n" +
            "条件:たみちゃんはNeutral、Joy、Angry、Sorrow、Fun、Surprisedの6つの感情と0〜5までの感情の度合いというパラメーターを持っています。\n" +
            "感情の度合い最も高いものとその感情を、文章の先頭に[Joy:5]のようにつけ、感情とその度合いを表現してください。\n" +
            "例:[Joy:5]わたし、今とっても怒ってるんだけど";

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

            if (result != null)
            {
                var response = result.choices[0].message.content;

                //表情タグ、関心レベルタグを削除する
                var (cleanText,emotion) = ExtractTags(response);

                Outputter.Instance.Output(cleanText);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 応答から表情タグと関心レベルタグを抽出し、クリーンなテキストを返す
        /// </summary>
        private (string,Emotion) ExtractTags(string input)
        {
            //表情タグを抽出
            var emotion = Emotion.Neutral;
            var matches = Regex.Matches(input, FaceTagPattern);
            foreach (Match match in matches)
            {
                var results = match.Value.Replace("[","").Replace("]","").Split(":");
                if (results.Length == 2)
                {
                    var emotionText = results[0];
                    var level = results[1];
                    emotion = emotionText.ToEmotion();
                }
            }

            //応答から表情タグを削除してクリーンなテキストを生成
            var output = Regex.Replace(input, FaceTagPattern, "");

            return (output,emotion);
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