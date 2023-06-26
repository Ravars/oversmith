using System.Collections.Generic;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Gameplay
{
    public enum BoxColor
    {
        Pink,
        Orange,
        Brown,
        Blue
    }
    
    
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
        public BoxColor boxColor;
        public int npcId;
        private int _numOfCorrectItems = 0;
        private int _totalItems = 0;
        private ClientsManager _clientsManager;
        public BoxCollider boxCollider;

        public void Init(ItemDeliveryList requiredItems, BoxColor newBoxColor, int npcId, ClientsManager clientsManager)
        {
            this.npcId = npcId;
            _requiredItems = requiredItems;
            _clientsManager = clientsManager;
            boxColor = newBoxColor;
            for (int i = 0; i < requiredItems.Items.Count; i++)
            {
                remainingItems.Add(requiredItems.Items[i].Amount);
            }

            // slider.value = 1 - (currentTime / totalTime);
            isRunning = true;
        }
        public bool CanSetItem(Item itemScript)
        {
            int itemIndex = _requiredItems.Items.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
            if (itemIndex != -1 && remainingItems[itemIndex] == 0)
            {
                AlertMessageManager.Instance.SpawnAlertMessage("Você já entregou todos os items deste tipo.", MessageType.Error);
                return false;
            }
            return true;
            // else
            // {
            //     return _totalItems < maxItems;
            // }
        }
        private void FixedUpdate()
        {
            if (!isRunning) return;
            currentTime += Time.fixedDeltaTime;
            // slider.value = 1 - (currentTime / totalTime);
            if (currentTime >= totalTime)
            {
                Finish();
            }
        }

        private void Finish()
        {
            if (!isRunning) return;
            isRunning = false;
            _clientsManager.ClientFinish(npcId);
        }
        public void SetItem(Transform itemTransform, Item itemScript)
        {
            int itemIndex = _requiredItems.Items.FindIndex(x => x.BaseItem.itemName == itemScript.baseItem.itemName);
            if (itemIndex != -1)
            {
                remainingItems[itemIndex]--;

                // SpawnTextStatus(true);
                HudController.Instance.SetItemCollected(_requiredItems.Items[itemIndex].BaseItem,npcId);
                _numOfCorrectItems++;
            }
            else
            {
                // itemIndex = pointsToSpawn.Length - 1;
                //
                // SpawnTextStatus(false);
            }
            
            SetParent(itemTransform, _totalItems);

            _totalItems++;

            if (CheckCompletion())
            {
                Invoke(nameof(Finish),1);
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
        private void SetParent(Transform itemTransform, int index)
        {
            var a = pointToPlaceItems[index];
            if (a == null) return;
            itemTransform.SetParent(a);
            itemTransform.SetLocalPositionAndRotation(a.localPosition, a.localRotation);
        }
    }
}