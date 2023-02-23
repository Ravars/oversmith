using System;
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
            return tempItem;
        }

        public bool CanSetItem(BaseItem newItem)
        {
            if (_interactable.hasCraftingTable)
            {
                foreach (var process in newItem.Processes)
                {
                    if (process.craftingTable == _interactable.craftingTable.type)
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
                bool canMerge = false;
                if (canMerge)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SetItem(BaseItem newItem, bool craftingTable = false)
        {
            if (item == null)
            {
                item = newItem;
                _itemTransform = Instantiate(item.prefab, pointToSpawnItem.position, Quaternion.identity,
                    pointToSpawnItem).transform;
            }

            if (craftingTable)
            {
                item = newItem;
                Destroy(_itemTransform.gameObject);
                _itemTransform = Instantiate(item.prefab, pointToSpawnItem.position, Quaternion.identity,
                    pointToSpawnItem).transform;
            }
            
        }
    }
}