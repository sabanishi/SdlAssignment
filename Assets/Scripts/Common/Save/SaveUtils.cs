using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public static class SaveUtils
    {
        private static string GetSavePath<T>()
        {
            return typeof(T).Name;
        }
        
        public static bool TrySave<T>(T saveData)
        {
            var path = GetSavePath<T>();   
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(path, json);
            PlayerPrefs.Save();
            return true;
        }

        public static bool TryLoad<T>(out T saveData)
        {
            saveData = default;
            var path = GetSavePath<T>();
            
            string json = PlayerPrefs.GetString(path);
            if (string.IsNullOrEmpty(json)) return false;
            
            saveData = JsonUtility.FromJson<T>(json);
            if (saveData == null) return false;
            return true;
        }

        public static void Delete<T>()
        {
            var path = GetSavePath<T>();
            PlayerPrefs.DeleteKey(path);
        }
        
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}