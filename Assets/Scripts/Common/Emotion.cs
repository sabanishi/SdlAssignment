namespace Sabanishi.SdiAssignment
{
    public enum Emotion
    {
        Neutral,
        Joy,
        Angry,
        Sorrow,
        Fun,
        Surprised
    }

    public static class EmotionExtensions
    {
        public static Emotion ToEmotion(this string emotion)
        {
            return emotion switch
            {
                "Neutral" => Emotion.Neutral,
                "Joy" => Emotion.Joy,
                "Angry" => Emotion.Angry,
                "Sorrow" => Emotion.Sorrow,
                "Fun" => Emotion.Fun,
                "Surprised" => Emotion.Surprised,
                _ => Emotion.Neutral
            };
        }
    }
}