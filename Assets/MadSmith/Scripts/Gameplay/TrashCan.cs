using MadSmith.Scripts.Interaction;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    [RequireComponent(typeof(InteractableHolder))]
    public class TrashCan : NetworkBehaviour
    {
        [SerializeField] private Animator animator;
        private static readonly int Open = Animator.StringToHash("Open");

        public void DestroyItem()
        {
            animator.SetTrigger(Open);
            
        }
    }
}