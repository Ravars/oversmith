using System.Collections.Generic;
using MadSmith.Scripts.Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Gameplay
{
    [RequireComponent(typeof(InteractableHolder))]
    public class DeliveryBox : MonoBehaviour
    {
        [SerializeField] private Transform[] pointToPlaceItems;
        private ItemDeliveryList _requiredItems;
        public List<int> remainingItems;
        
        public Slider slider;
        
        public float totalTime = 120;
        public float currentTime;
        public bool isRunning;

        public void Init(ItemDeliveryList requiredItems)
        {
            _requiredItems = requiredItems;
            for (int i = 0; i < requiredItems.Items.Count; i++)
            {
                remainingItems.Add(requiredItems.Items[i].Amount);
            }
            slider.value = 1 - (currentTime / totalTime);
            isRunning = true;
        }
    }
}