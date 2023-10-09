using System;
using System.Collections.Generic;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    /// <summary>
    /// This class is used for Events that have one Order List argument.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Order List Event Channel")]
    public class OrderListUpdateEventChannelSO : ScriptableObject
    {
        public UnityAction<List<OrderData>> OnEventRaised;

        public void RaiseEvent(List<OrderData> value)
        {
            OnEventRaised?.Invoke(value);
        }
    }

    [Serializable]
    public class OrderData
    {
        public int Id;
        public float TimeRemaining01;
        public BaseItem BaseItem;

        public OrderData(int id, float timeRemaining01, BaseItem baseItem)
        {
            Id = id;
            TimeRemaining01 = timeRemaining01;
            BaseItem = baseItem;
        }
    }
}