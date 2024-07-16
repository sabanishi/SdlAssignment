namespace Sabanishi.SdiAssignment
{
    public class ChatUser
    {
        private const string FaceTagPattern = @"\[face:([^\]_]+)_?(\d*)\]";
        private const string InterestTagPattern = @"\[interest:(\d)\]";
        private const string InitialMessage = "こんにちは！";
        
        private ChatGPTConnection _connection;
        private string _apiKey;

        public ChatUser()
        {
            if (SaveUtils.TryLoad<ApiData>(out var data))
            {
                SetupConnection(data.apiKey);
            }
        }

        /// <summary>
        /// APIキーを設定し、ChatGPTConnectionを初期化する
        /// </summary>
        public void SetupConnection(string apiKey)
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