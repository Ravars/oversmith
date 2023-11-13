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
        public UnityAction<List<OrderTimes>> OnEventRaised;

        public void RaiseEvent(List<OrderTimes> value)
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

    [Serializable]
    public class OrderTimes
    {
        public int Id;
        public float TimeRemaining01;

        public OrderTimes()
        {
            Id = 0;
            TimeRemaining01 = 0;
        }

        public OrderTimes(int id, float timeRemaining01)
        {
            Id = id;
            TimeRemaining01 = timeRemaining01;
        }
    }
    
}