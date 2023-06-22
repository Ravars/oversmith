using System;
using System.Collections.Generic;
using _Developers.Vitor;
using MadSmith.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace MadSmith.Scripts.Level
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
        public GameObject itemsHolder;
        public float totalTime = 120;
        public float currentTime;
        public bool isRunning;

        public GameObject visual;
        public GameObject visualItems;
        public GameObject wagonMan;

        

        public GameObject nextWagon;
        public Slider slider;
        
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
            slider.value = 1 - (currentTime / totalTime);
            Invoke(nameof(StartTimer),5);
        }

        public bool CanSetItem()
        {
            return itemsDelivered < totalItems; 
        }
        
        public void SetItem(Transform itemTransform, Item itemScript)
        {
            // if (CanSetItem())
            // {
                ItemStruct itemStruct;
                int itemIndex = currentItems.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
                if (itemIndex != -1)
                {
                    
                    itemStruct = currentItems[itemIndex];
                    itemStruct.Amount++;
                    currentItems[itemIndex] = itemStruct;
                }
                else
                {
                    itemStruct = new ItemStruct()
                    {
                        Amount = 1,
                        BaseItem = itemScript.baseItem
                    };
                    itemIndex = 0;
                    currentItems.Add(itemStruct);
                }


                SetParent(itemTransform, itemIndex);
                itemsDelivered++;
                SpawnTextStatus(itemStruct);
                
                
                if (itemsDelivered >= totalItems)
                {
                    Finish();
                }
            // }
        }

        private void SpawnTextStatus(ItemStruct itemStruct)
        {
            bool correctItem = false;
            bool correctAmount = false;
            for (int i = 0; i < requiredItems.Length; i++)
            {
                if (requiredItems[i].BaseItem.itemName == itemStruct.BaseItem.itemName)
                {
                    correctAmount = requiredItems[i].Amount >= itemStruct.Amount;
                    correctItem = true;
                }
            }

            string message = correctItem ? 
                correctAmount ? "Voce entregou um item correto" : "Voce entregou mais itens do que solicitado"  : 
                "Voce entregou um item errado!";
            AlertMessageManager.Instance.SpawnAlertMessage(message,correctItem && correctAmount? MessageType.Success : MessageType.Error);


        }
        private void SetParent(Transform itemTransform,int index)
        {
            Debug.Log(index);
            itemTransform.SetParent(pointsToSpawn[index].transform);
            itemTransform.SetLocalPositionAndRotation(pointsToSpawn[index].transform.localPosition,pointsToSpawn[index].transform.localRotation);
        }

        private void Finish()
        { 
            isRunning = false; 
            Invoke(nameof(FinishTimer),1);
        }

        private void FinishTimer()
        {
            SetVisual(false);
            AlertMessageManager.Instance.SpawnAlertMessage("O entregador foi embora com os items", MessageType.Normal);

            if (nextWagon != null)
            {
                nextWagon.SetActive(true);
            }
            else
            {
                AlertMessageManager.Instance.SpawnAlertMessage("Fim do prototipo.", MessageType.Alert);
            }
            
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
            slider.value = 1 - (currentTime / totalTime);
            if (currentTime >= totalTime)
            {
                Finish();
            }
        }
    }
}