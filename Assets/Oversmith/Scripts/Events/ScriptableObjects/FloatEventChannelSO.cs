using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one float argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public UnityAction<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}