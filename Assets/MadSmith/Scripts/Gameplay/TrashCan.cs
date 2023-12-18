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

        [Command]
        public void DestroyItem()
        {
            Animation();
        }

        [ClientRpc]
        public void Animation()
        {
            animator.SetTrigger(Open);
        }
    }
}