using System;
using System.Collections.Generic;
using _Developers.Vitor;
using Oversmith.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Oversmith.Scripts.Level
{

    // Definido em Delivery. Descomentar se remover Delivery
    //[Serializable]
    //public struct ItemStruct
    //{
    //    public int Amount;
    //    public BaseItem BaseItem;
    //}
    
    
    [RequireComponent(typeof(InteractableHolder))]
    public class DeliveryBox : MonoBehaviour
    {
        public ItemDeliveryList requiredItems;
        public List<int> remainingItems;
        public int itemsDelivered;
        public BaseItem boxItem;
        public Transform[] pointsToSpawn;
        //public GameObject itemsHolder;
        public float totalTime = 120;
        public float currentTime;
        public bool isRunning;

        public GameObject visual;
        public GameObject wagonMan;

        public bool isActive = false;

        public GameObject nextWagon;
        public Slider slider;

        int totalItems = 5;
        private BoxCollider trigger;

        private void Start()
        {
            for (int i = 0; i < requiredItems.Items.Count; i++)
            {
                remainingItems.Add(requiredItems.Items[i].Amount);
            }
            trigger = GetComponent<BoxCollider>();
            SetTrigger(false);
            SetVisual(false);
            slider.value = 1 - (currentTime / totalTime);
            Invoke(nameof(StartTimer), 5);
        }

        public bool CanSetItem()
        {
            return itemsDelivered < totalItems;
        }

        public void SetItem(Transform itemTransform, Item itemScript)
        {
            int itemIndex = requiredItems.Items.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
            if (itemIndex != -1)
            {
                remainingItems[itemIndex]--;

                SpawnTextStatus(true);
            }
            else
            {
                itemIndex = pointsToSpawn.Length - 1;

                SpawnTextStatus(false);
            }
            
            SetParent(itemTransform, itemIndex);

            if (CheckCompletion())
            {
                Debug.Log("Ready to Deliver");
            }
        }

        public bool CheckCompletion()
        {
            bool isComplete = true;
            foreach (var item in remainingItems)
            {
                if (item > 0)
                {
                    isComplete = false;
                    break;
                }
            }
            return isComplete;
        }

        private void SpawnTextStatus(bool correctItem)
        {
            
            //for (int i = 0; i < requiredItems.Length; i++)
            //{
            //    if (requiredItems[i].BaseItem.itemName == itemStruct.BaseItem.itemName)
            //    {
            //        correctAmount = requiredItems[i].Amount >= itemStruct.Amount;
            //        correctItem = true;
            //    }
            //}

            string message = correctItem ?
                "Voce entregou um item correto" :
                "Voce entregou um item errado!";
            AlertMessageManager.Instance.SpawnAlertMessage(message, correctItem ? MessageType.Success : MessageType.Error);


        }
        private void SetParent(Transform itemTransform, int index)
        {
            Debug.Log(index);
            itemTransform.SetParent(pointsToSpawn[index].transform);
            itemTransform.SetLocalPositionAndRotation(pointsToSpawn[index].transform.localPosition, pointsToSpawn[index].transform.localRotation);
        }

        public void Finish()
        {
            isRunning = false;
            Invoke(nameof(FinishTimer), 1);
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
            wagonMan.SetActive(true);
            isRunning = true;
        }

        private void SetVisual(bool state) // This is just to simulate the Wagon delivery to the Store
        {
            visual.SetActive(state);
            wagonMan.SetActive(state);

        }

        public void SetTrigger(bool state)
        {
            trigger.enabled = state;
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