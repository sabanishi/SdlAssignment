using System;
using Cysharp.Threading.Tasks;
using Live2D.Cubism.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sabanishi.SdiAssignment.Sandbox
{
    public class MouthController:MonoBehaviour
    {
        [SerializeField] private CubismParameter openParameter;
        [SerializeField] private CubismParameter formParameter;
        
        private bool _isTalking = true;
        private float _nowMouthOpen = 0.0f;
        private float _nowMouthForm = 0.0f;
        
        // ランダムな時間間隔
        public float minInterval = 0.1f;
        public float maxInterval = 0.5f;

        // 口の開閉の範囲
        public float minMouthOpen = 0.0f;
        public float maxMouthOpen = 1.0f;
        public float minMouthForm = -1.0f;
        public float maxMouthForm = 1.0f;

        
        public void SetTalking(bool isTalking)
        {
            _isTalking = isTalking;
        }

        private void Start()
        {
            OpenMouth().Forget();
        }

        private void LateUpdate()
        {
            openParameter.Value = _isTalking ? _nowMouthOpen : 1.0f;
            formParameter.Value = _isTalking ? _nowMouthForm : 0.0f;
        }

        private async UniTask OpenMouth()
        {
            while (true)
            {
                // ランダムな時間待機
                float waitTime = Random.Range(minInterval, maxInterval);
                await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

                // ランダムな口の開閉値を設定
                if(_nowMouthOpen == 0.0f)
                {
                    _nowMouthOpen = Random.Range(minMouthOpen, maxMouthOpen);
                    _nowMouthForm = Random.Range(minMouthForm, maxMouthForm);
                }
                else
                {
                    _nowMouthOpen = 0.0f;
                    _nowMouthForm = 1.0f;
                }
            }
        }
    }
}