using System.Collections.Generic;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one Order argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Order Update Event Channel")]
    public class OrderUpdateEventChannelSO : ScriptableObject
    {
        public UnityAction<OrderData> OnEventRaised;

        public void RaiseEvent(OrderData value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}