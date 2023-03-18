using System;
using System.Linq;
using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(Interactable))]
    public class Table : MonoBehaviour
    {
        public BaseItem item { get; private set; }
        private Transform _itemTransform;
        [SerializeField] private Transform pointToSpawnItem;
        private Interactable _interactable;

        public Item itemScript { get; private set; }
        //ideia
        // public Base

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        public bool HasItem()
        {
            return item != null;
        }

        public BaseItem GetItem()
        {
            var tempItem = item;
            Destroy(_itemTransform.gameObject);
            item = null;
            _itemTransform = null;
            itemScript = null;
            return tempItem;
        }

        public bool CanSetItem(BaseItem newItem)
        {
            if (_interactable.hasCraftingTable)
            {
                foreach (var process in newItem.processes)
                {
                    if (process.craftingTable == _interactable.craftingTable.type && process.craftingTable != CraftingTableType.Table)
                    {
                        return true;
                    }
                }
                return false;
            }
            
            
            
            
            
            if (item == null)
            {
                return true;
            }
            else
            {
                //new item pode ser mesclado com o item atual ?
                BaseItem[] itemsInUse = {
                    newItem,
                    item
                };
                Process[] processes = newItem.processes.Concat(item.processes).ToArray();
                
                
                
                
                
                foreach (var process in processes)
                {
                    var canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));

                    if (canMerge)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void SetItem(BaseItem newItem, bool craftingTable = false)
        {
            if (craftingTable)
            {
                Debug.Log("Crafting table");
                SpawnItem(newItem,true);
                return;
            }
            
            if (item == null)
            {
                Debug.Log("Item null");
                SpawnItem(newItem,false);
            }
            else
            {
                Debug.Log("merge item");
                BaseItem[] itemsInUse = {
                    newItem,
                    item
                };
                Process[] processes = newItem.processes.Concat(item.processes).ToArray();
                
                foreach (var process in processes)
                {
                    var canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                    if (canMerge)
                    {
                        SpawnItem(process.itemGenerated, true);
                    }
                }
            }
        }

        private void SpawnItem(BaseItem newItem, bool hasToDestroy)
        {
            item = newItem;
            if (hasToDestroy)
            {
                Destroy(_itemTransform.gameObject);
            }
            _itemTransform = Instantiate(item.prefab, pointToSpawnItem.position, Quaternion.identity,
                pointToSpawnItem).transform;
            itemScript = _itemTransform.GetComponent<Item>();
        }
    }
}