using System.Collections.Generic;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.OLD
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
        // public BaseItem boxItem;
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

        int maxItems = 5;
        private BoxCollider trigger;
        public string wagonName;

        int numOfCorrectItems = 0;
        int totalItems = 0;

        private CustomerManager _customerManager;
        private int _costumerIndex;

        private void Start()
        {
            for (int i = 0; i < requiredItems.Items.Count; i++)
            {
                remainingItems.Add(requiredItems.Items[i].Amount);
            }
            trigger = GetComponent<BoxCollider>();
            SetTrigger(false);
            //SetVisual(false);

            slider.value = 1 - (currentTime / totalTime);

            wagonName = wagonMan.gameObject.name;

            isRunning = true;
        }

        public bool CanSetItem(Item itemScript)
        {
            int itemIndex = requiredItems.Items.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
            if (itemIndex != -1 && remainingItems[itemIndex] == 0)
            {
                AlertMessageManager.Instance.SpawnAlertMessage("Você já entregou todos os items deste tipo.", MessageType.Error);
                return false;
            }
            else
            {
                return totalItems < maxItems;
            }
        }

        public void SetItem(Transform itemTransform, Item itemScript)
        {
            int itemIndex = requiredItems.Items.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
            if (itemIndex != -1)
            {
                remainingItems[itemIndex]--;

                SpawnTextStatus(true);
                // HudController.Instance.SetItemCollected(requiredItems.Items[itemIndex].BaseItem,wagonName);
                numOfCorrectItems++;
            }
            else
            {
                itemIndex = pointsToSpawn.Length - 1;

                SpawnTextStatus(false);
            }
            
            SetParent(itemTransform, itemIndex);

            totalItems++;

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
            //FinishTimer();
            Invoke(nameof(FinishTimer), 1);
        }

        private void FinishTimer()
        {
            int boxScore = totalItems > 0 ? Mathf.RoundToInt((numOfCorrectItems / (float)totalItems) * 100) : 0;
            Debug.Log($"{numOfCorrectItems}, {totalItems}");
            SetVisual(false);
            AlertMessageManager.Instance.SpawnAlertMessage($"O entregador foi embora com os items. Nota: {boxScore}%", MessageType.Normal);

            if (_customerManager != null) //TODO mudar para um cancelamento do time ao finalizar o level
            {
                _customerManager.DisableCustomer(wagonMan, boxScore, (1 - (currentTime/totalTime)));
                gameObject.SetActive(false);
            }
            // Enviar para ele a lista currentItem e a Required items e deixa ele se virar
        }


        private void SetVisual(bool state) // This is just to simulate the Wagon delivery to the Store
        {
            visual.SetActive(state);
            wagonMan.gameObject.SetActive(state);
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
        public void SetCustomerManager(CustomerManager cm, int index)
        {
            _costumerIndex = index;
            _customerManager = cm;
        }
    }
}