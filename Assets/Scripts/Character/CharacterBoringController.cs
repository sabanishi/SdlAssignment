using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    /// <summary>
    /// 一定時間何の操作も無かったら暇だと判断する機構
    /// </summary>
    public class CharacterBoringController:MonoBehaviour
    {
        [SerializeField] private AnimatorController animatorController;

        private static CharacterBoringController _instance;
        public static CharacterBoringController Instance => _instance;
        
        private float _counter;
        
        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        private void Update()
        {
            var catchCounter = _counter;
            _counter += Time.deltaTime;
            
            var isBoring = _counter > 15;
            
            animatorController.SetIsBoring(isBoring);
            
            //30秒に一回抽選を行う
            if (catchCounter < 45 && _counter >= 45)
            {
                var rand = Random.Range(0, 2);
                animatorController.SetBoringRand(rand);
                _counter = 15f;
            }
        }

        public void ResetCounter()
        {
            _counter = 0;
        }
    }
}