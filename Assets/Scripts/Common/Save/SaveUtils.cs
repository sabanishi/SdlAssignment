using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public static class SaveUtils
    {
        private static readonly Dictionary<Type,string> SavePathDict = new Dictionary<Type, string>
        {
            {typeof(ApiData), "Save"}
        };

        public static bool TrySave<T>(T saveData)
        {
            if (!SavePathDict.TryGetValue(typeof(T), out var path)) return false;
            
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(path, json);
            PlayerPrefs.Save();
            return true;
        }

        public static bool TryLoad<T>(out T saveData)
        {
            saveData = default;
            if (!SavePathDict.TryGetValue(typeof(T), out var path)) return false;
            
            string json = PlayerPrefs.GetString(path);
            if (string.IsNullOrEmpty(json)) return false;
            
            saveData = JsonUtility.FromJson<T>(json);
            return true;
        }

        public static bool TryDelete<T>()
        {
            if (!SavePathDict.TryGetValue(typeof(T), out var path)) return false;
            
            PlayerPrefs.DeleteKey(path);
            return true;
        }
        
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        /// <summary>
        /// 保存先キーがDictionaryに登録されているかを返す
        /// </summary>
        public static bool CanSave<T>()
        {
            return SavePathDict.ContainsKey(typeof(T));
        }
    }
}