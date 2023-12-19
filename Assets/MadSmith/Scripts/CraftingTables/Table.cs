    using System;
using System.Linq;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Systems;
using MadSmith.Scripts.UI;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Table : NetworkBehaviour
    {
        [SyncVar] private GameObject _itemObject;
        public Item ItemScript { get; private set; }
        // [SyncVar] public int num;
        private InteractableHolder _interactableHolder;
        [field: SerializeField] public Transform PointToSpawnItem { get; private set; }

        private void Awake()
        {
            _interactableHolder = GetComponent<InteractableHolder>();
        }

        public bool HasItem()
        {
            return _itemObject != null;
        }
        public void SetObject(GameObject itemObject)
        {
            if (itemObject != null)
            {
                _itemObject = itemObject;
                ItemScript = _itemObject.GetComponent<Item>();
            }
            else
            {
                _itemObject = null;
                ItemScript = null;
            }
        }

        public bool CanSetItem(Item newItem)
        {
                Debug.Log("CanSetItem");
            if (_itemObject == null && !_interactableHolder.hasCraftingTable)
            {
                return true;
            }
            if (_interactableHolder.hasCraftingTable)
            {
                Debug.Log("CanSetItem 1");
                if (_itemObject == null)
                {
                    return true;
                }
                Debug.Log("CanSetItem 2");
                
                
                foreach (var process in newItem.baseItem.processes)
                {
                    Debug.Log("process: "+process.craftingTable);
                    if (process.craftingTable == _interactableHolder.craftingTable.type && process.craftingTable != CraftingTableType.WorkingTable)
                    {
                        return true;
                    }
                }
                return false;
            }
            //Debug.Log("CanSetItem 3");

            return false;
        }

        public bool CanMergeItem(Item newItem)
        {
            if (_itemObject == null) return false;
            
            // var itemScript = _itemObject.GetComponent<Item>();
            if (ItemScript == null) return false;

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
                if (canMerge && _interactableHolder.craftingTable.type == process.craftingTable)
                {
                    return true;
                }
            }

            return false;
        }

        public int MergeItem(Item newItem)
        {
            // var itemScript = _itemObject.GetComponent<Item>();
            BaseItem[] itemsInUse = {
                newItem.baseItem,
                ItemScript.baseItem
            };
            Process[] processes = newItem.baseItem.processes.Concat(ItemScript.baseItem.processes).ToArray();
            
            foreach (var process in processes)
            {
                Debug.Log("A");
                var canMerge = false;
                if (process.itemsNeeded.Length > 0)
                {
                    canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                }

                Debug.Log("B");
                if (canMerge && _interactableHolder.craftingTable.type == process.craftingTable)
                {
                    Debug.Log("C: " + process.itemGenerated.id);
                    // AlertMessageManager.Instance.SpawnAlertMessage($"Item {process.itemGenerated.itemName} construído com sucesso.", MessageType.Normal);
                    // DestroyItem();
                    // Destroy(newItem.transform);
                    // SpawnNewItem(process.itemGenerated);
                    return process.itemGenerated.id;
                }
            }

            return -1;
        }
    }
}