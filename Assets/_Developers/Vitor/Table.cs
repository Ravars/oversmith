using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(Interactable))]
    public class Table : MonoBehaviour
    {
        private BaseItem item;
        private Transform _itemTransform;
        [SerializeField] private Transform pointToSpawnItem;

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

        public void SetItem(BaseItem newItem)
        {
            if (item == null)
            {
                item = newItem;
                _itemTransform = Instantiate(item.prefab, pointToSpawnItem.position, Quaternion.identity,
                    pointToSpawnItem).transform;
            }
            
        }
    }
}