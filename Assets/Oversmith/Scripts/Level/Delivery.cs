using System;
using System.Collections.Generic;
using System.Threading;
using _Developers.Vitor;
using Oversmith.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Oversmith.Scripts.Level
{
    [Serializable]
    public struct ItemStruct
    {
        public int Amount;
        public BaseItem BaseItem;
    }
    
    
    [RequireComponent(typeof(InteractableHolder))]
    public class Delivery : MonoBehaviour
    {
        public BaseItem[] availableItems; // Maybe remove from this script
        public ItemStruct[] requiredItems; // private
        public List<ItemStruct> currentItems = new();
        public int itemsDelivered; 
        [Range(2,10)] 
        public int maxItems;
        public GameObject[] pointsToSpawn;
        public float totalTime = 120;
        public float currentTime;
        public bool isRunning;

        public GameObject visual;
        public GameObject visualItems;
        public GameObject wagonMan;

        public AlertMessage alertMessagePrefab;
        public GameObject alertMessageHolder;
        int totalItems = 0;
        
        private void Start()
        {
            int amount = Random.Range(1, maxItems + 1);
            requiredItems = new ItemStruct[amount];
            List<int> listNumbers = new List<int>();
            var rand = new System.Random();
            for (int i = 0; i < amount; i++)
            {
                int number;
                do {
                    number = rand.Next(0, availableItems.Length);
                } while (listNumbers.Contains(number));
                listNumbers.Add(number);
            }

            for (int i = 0; i < listNumbers.Count; i++)
            {
                int itemAmount = Random.Range(1, 4);
                totalItems += itemAmount;
                requiredItems[i] = new ItemStruct()
                {
                    Amount = itemAmount,
                    BaseItem = availableItems[listNumbers[i]]
                };   
            }
            SetVisual(false);
            Invoke(nameof(StartTimer),5);
        }

        public bool CanSetItem()
        {
            return itemsDelivered < totalItems; 
        }
        
        public void SetItem(BaseItem newItem)
        {
            if (CanSetItem())
            {
                int itemIndex = currentItems.FindIndex(x => x.BaseItem.itemName == newItem.itemName);
                if (itemIndex != -1)
                {
                    ItemStruct itemStruct = currentItems[itemIndex];
                    itemStruct.Amount++;
                    currentItems[itemIndex] = itemStruct;
                }
                else
                {
                    currentItems.Add(new ItemStruct()
                    {
                        Amount = 1,
                        BaseItem = newItem
                    });
                }
                SpawnItem(newItem, itemsDelivered);
                itemsDelivered++;
                SpawnTextStatus(newItem);
                
                
                if (itemsDelivered >= totalItems)
                {
                    Finish();
                }
            }
        }

        private void SpawnTextStatus(BaseItem newItem)
        {
            bool correctItem = false;
            for (int i = 0; i < requiredItems.Length; i++)
            {
                if (requiredItems[i].BaseItem.itemName == newItem.itemName)
                {
                    correctItem = true;
                }
            }

            string message;
            if (correctItem)
            {
                message = "Voce entregou um item correto!";
            }
            else
            {
                message = "Voce entregou um item errado!";
            }
            
            AlertMessage alertMessage = Instantiate(alertMessagePrefab, alertMessageHolder.transform).GetComponent<AlertMessage>();
            alertMessage.text.text = message;

        }
        private void SpawnItem(BaseItem item, int index)
        {
            Instantiate(item.prefab, pointsToSpawn[index].transform.position, pointsToSpawn[index].transform.rotation, pointsToSpawn[index].transform.parent);
        }

        private void Finish()
        { 
            isRunning = false; 
            Invoke(nameof(FinishTimer),1);
        }

        private void FinishTimer()
        {
            SetVisual(false);
            
            //  Avisar pro game manager que foi finalizado
            
            // Enviar para ele a lista currentItem e a Required items e deixa ele se virar
        }

        public void StartTimer()
        {
            SetVisual(true);
            isRunning = true;
        }

        private void SetVisual(bool state) // This is just to simulate the Wagon delivery to the Store
        {
            visual.SetActive(state);            
            visualItems.SetActive(state);            
            wagonMan.SetActive(state);
        }

        private void FixedUpdate()
        {
            if (!isRunning) return;
            currentTime += Time.fixedDeltaTime;
            if (currentTime >= totalTime)
            {
                Finish();
            }
        }
    }
}