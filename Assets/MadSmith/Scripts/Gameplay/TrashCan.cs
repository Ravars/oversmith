using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    [RequireComponent(typeof(InteractableHolder))]
    public class TrashCan : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int Open = Animator.StringToHash("Open");

        public void DestroyItem()
        {
            animator.SetTrigger(Open);
            
        }
    }
}