using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(Interactable))]
    public class Table : MonoBehaviour
    {
        private GameObject item;

        public bool HasItem()
        {
            return item != null;
        }

        public GameObject GetItem()
        {
            var tempItem = item;
            Destroy(item);
            item = null;
            return tempItem;
        }
    }
}