using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have no argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}