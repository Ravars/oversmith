using System.Linq;
using Oversmith.Scripts.UI;
using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Table : MonoBehaviour
    {
        public BaseItem item { get; private set; }
        private Transform _itemTransform;
        [SerializeField] private Transform pointToSpawnItem;
        private InteractableHolder _interactableHolder;

        public Item itemScript { get; private set; }
        //ideia
        // public Base

        private void Awake()
        {
            _interactableHolder = GetComponent<InteractableHolder>();
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
            if (_interactableHolder.hasCraftingTable)
            {
                if (item != null)
                {
                    return false;
                }
                
                
                foreach (var process in newItem.processes)
                {
                    if (process.craftingTable == _interactableHolder.craftingTable.type && process.craftingTable != CraftingTableType.Table)
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
                BaseItem[] itemsInUse = {
                    newItem,
                    item
                };
                Process[] processes = newItem.processes.Concat(item.processes).ToArray();
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
        }

        public void SetItem(BaseItem newItem, bool craftingTable = false)
        {
            if (craftingTable)
            {
                AlertMessageManager.Instance.SpawnAlertMessage($"Item {newItem.itemName} construído com sucesso.", MessageType.Normal);
                SpawnItem(newItem,true);
                return;
            }
            
            if (item == null)
            {
                SpawnItem(newItem,false);
            }
            else
            {
                BaseItem[] itemsInUse = {
                    newItem,
                    item
                };
                Process[] processes = newItem.processes.Concat(item.processes).ToArray();
                
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
                        SpawnItem(process.itemGenerated, true);
                        break;
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
            _itemTransform = Instantiate(item.prefab, pointToSpawnItem.position, pointToSpawnItem.rotation,
                pointToSpawnItem).transform;
            itemScript = _itemTransform.GetComponent<Item>();
        }
    }
}