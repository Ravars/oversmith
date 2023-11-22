using System;
using System.Linq;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Table : NetworkBehaviour
    {
        public Item ItemScript { get; private set; }
        // public bool isWorkTable = false;
        // public BaseItem BaseItem { get; private set; }
        private Transform _itemTransform;
        
        private InteractableHolder _interactableHolder;
        [SerializeField] private Transform pointToSpawnItem;
        //ideia

        //[SerializeField] private AudioSource catchSound;

        // public Base

        private void Awake()
        {
            _interactableHolder = GetComponent<InteractableHolder>();
        }

        public bool HasItem()
        {
            return ItemScript != null;
        }

        // public BaseItem GetItem()
        // {
        //     var tempItem = BaseItem;
        //     Destroy(_itemTransform.gameObject);
        //     BaseItem = null;
        //     _itemTransform = null;
        //     ItemScript = null;
        //     return tempItem;
        // }

        public Tuple<Transform,Item> RemoveFromTable(Transform newParent)
        {
            Transform tempTransform = _itemTransform;
            Item tempItem = ItemScript;
            _itemTransform.SetParent(newParent);
            _itemTransform = null;
            ItemScript = null;
            return new Tuple<Transform,Item>(tempTransform,tempItem);
        }

        public void PutOnTable(Transform itemTransform, Item itemScript)
        {
            //catchSound.Play();
            ItemScript = itemScript;
            _itemTransform = itemTransform;
            _itemTransform.SetParent(pointToSpawnItem);
            // _itemTransform.SetLocalPositionAndRotation(Vector3.zero, pointToSpawnItem.localRotation);
            _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            if (_interactableHolder.hasCraftingTable)
            {
                _interactableHolder.craftingTable.ItemAddedToTable();
            }
        }

        public bool CanSetItem(Item newItem)
        {
            if (_interactableHolder.hasCraftingTable)
            {
                if (ItemScript != null)
                {
                    return false;
                }
                
                
                foreach (var process in newItem.baseItem.processes)
                {
                    if (process.craftingTable == _interactableHolder.craftingTable.type && process.craftingTable != CraftingTableType.Table)
                    {
                        return true;
                    }
                }
                return false;
            }

            return ItemScript == null;
        }

        public bool CanMergeItem(Item newItem)
        {
            if (ItemScript == null)
            {
                return false;
            }
            
            BaseItem[] itemsInUse = {
                newItem.baseItem,
                ItemScript.baseItem
            };
            Process[] processes = newItem.baseItem.processes.Concat(ItemScript.baseItem.processes).ToArray();
            foreach (var process in processes)
            {
                var canMerge = false;
                if (process.itemsNeeded.Length > 0)
                {
                    canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                }
                if (canMerge)
                {
                    return true;
                }
            }

            return false;
        }

        public void CraftItem(BaseItem newBaseItem)
        {
            DestroyItem();
            SpawnNewItem(newBaseItem);
            
        }

        public void MergeItem(Item newItem)
        {
            BaseItem[] itemsInUse = {
                newItem.baseItem,
                ItemScript.baseItem
            };
            Process[] processes = newItem.baseItem.processes.Concat(ItemScript.baseItem.processes).ToArray();
            
            foreach (var process in processes)
            {
                var canMerge = false;
                if (process.itemsNeeded.Length > 0)
                {
                    canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                }
                if (canMerge)
                {
                    
                    AlertMessageManager.Instance.SpawnAlertMessage($"Item {process.itemGenerated.itemName} construído com sucesso.", MessageType.Normal);
                    DestroyItem();
                    // Destroy(newItem.transform);
                    SpawnNewItem(process.itemGenerated);
                    break;
                }
            }
        }

        private void DestroyItem()
        {
            if (_itemTransform != null)
            {
                Destroy(_itemTransform.gameObject);
            }
            _itemTransform = null;
            ItemScript = null;
        }
        private void SpawnNewItem(BaseItem newItem) // Provavelmente quebrado
        {
            Debug.Log($"Spawn {newItem.name}");
            _itemTransform = Instantiate(newItem.prefab, pointToSpawnItem.position, pointToSpawnItem.rotation,
                pointToSpawnItem).transform;
            ItemScript = _itemTransform.GetComponent<Item>();
            if (AlertMessageManager.InstanceExists)
            {
                AlertMessageManager.Instance.SpawnAlertMessage($"Item {ItemScript.baseItem.itemName} construído com sucesso.", MessageType.Normal);
            }
        }
    }
}