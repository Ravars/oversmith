using System;
using _Developers.Vitor;
using Oversmith.Scripts.Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oversmith.Scripts.Level
{
    public class Wagon : Interactable
    {
        public BaseItem[] availableItems;
        public BaseItem[] requiredItems;
        [Range(2,10)]
        public int maxItems;
        public float timeRemaining;
        public GameObject[] pointsToSpawn;
        
        private void Start()
        {
            int amount = Random.Range(1, maxItems + 1);
            Debug.Log("amount: " + amount);
            requiredItems = new BaseItem[amount];
            for (int i = 0; i < amount; i++)
            {
                int itemIndex = Random.Range(0, availableItems.Length);
                Debug.Log(itemIndex);
                requiredItems[i] = availableItems[itemIndex];
            }

        }

        // public override void Interact()
        // {
        //     
        // }
    }
}