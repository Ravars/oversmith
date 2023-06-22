    using _Developers.Vitor;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Interactable : MonoBehaviour
    {
        public virtual void Interact()
        {
            Debug.Log("interact");
        }

        public virtual void Interact(GameObject obj)
        {
            Debug.Log("gameobject interact");
        }
    }
}