using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Interactable : NetworkBehaviour
    {
        public virtual void Interact()
        {
            //Debug.Log("interact");
        }

        public virtual void Interact(PlayerInteractions obj)
        {
            //Debug.Log("gameobject interact");
        }
    }
}