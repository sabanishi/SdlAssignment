using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class AnimatorController:MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int IsAwaawa = Animator.StringToHash("is_awaawa");
        private static readonly int IsBoring = Animator.StringToHash("is_boring");
        private static readonly int BoringRand = Animator.StringToHash("boring_rand");
        private static readonly int IsDance = Animator.StringToHash("is_dance");

        public void SetIsDragging(bool isDragging)
        {
            animator.SetBool(IsAwaawa, isDragging);
        }
        
        public void SetIsBoring(bool isBoring)
        {
            animator.SetBool(IsBoring, isBoring);
        }
        
        public void SetBoringRand(int rand)
        {
            animator.SetFloat(BoringRand, rand);
        }
        
        public void SetIsDance(bool isDance)
        {
            animator.SetBool(IsDance, isDance);
        }
    }
}