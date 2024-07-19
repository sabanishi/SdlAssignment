using System;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class ConsoleSizeUpdater:MonoBehaviour
    {
        [SerializeField] private RectTransform root;
        
        private static ConsoleSizeUpdater _instance;
        public static ConsoleSizeUpdater Instance => _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void Start()
        {
            var screenSize = new Vector2(Screen.width, Screen.height);
            var width = screenSize.x/2;
            var height = screenSize.y/2;
                
            UpdateSize((int)width,(int)height);
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public void UpdateSize(int width= -1,int height = -1)
        {
            //現在の画面サイズを取得する
            var screenSize = new Vector2(Screen.width, Screen.height);
            var screenWidth = screenSize.x;

            var scale = width / screenWidth * 1600/1180f;
            
            root.transform.localScale = new Vector3(scale, scale, 1);

            var realHeight = height / (float)width * 1180f;
            root.sizeDelta = new Vector2(root.sizeDelta.x, realHeight);
            
            //データを保存する
            var data = new ConsoleSizeData
            {
                width = width,
                height = height
            };
            
            SaveUtils.TrySave(data);
        }
    }
}