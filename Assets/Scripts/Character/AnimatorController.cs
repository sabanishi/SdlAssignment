using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class AnimatorController:MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int IsAwaawa = Animator.StringToHash("is_awaawa");

        public void SetIsDragging(bool isDragging)
        {
            animator.SetBool(IsAwaawa, isDragging);
        }
    }
}