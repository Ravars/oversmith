using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Menu.Old
{
    public abstract class MenuCanvas : MonoBehaviour
    {
        public UnityEvent<System.Action> OnReturn;
        public abstract void Begin();
    }
}

