using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public event UnityAction<bool> OnEventRaised;

        public void RaiseEvent(bool value)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(value);
        }
    }
}