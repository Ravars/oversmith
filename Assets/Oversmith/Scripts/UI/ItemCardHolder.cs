using _Developers.Vitor;
using Oversmith.Scripts.Level;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class ItemCardHolder : MonoBehaviour
    {
        public ItemCard[] itemCards;
        public GameObject prefabCard;
        public Transform itemsHolder;
        public string wagonName;
        public void SetItems(ItemStruct[] itemStructs, string wagonName)
        {
            this.wagonName = wagonName;
            itemCards = new ItemCard[itemStructs.Length];
            for (int i = 0; i < itemStructs.Length; i++)
            {
                itemCards[i] = Instantiate(prefabCard, itemsHolder).GetComponent<ItemCard>();
                itemCards[i].SetItem(itemStructs[i]);
            }
        }

        public void SetItemChecked(BaseItem baseItem)
        {
            for (int i = 0; i < itemCards.Length; i++)
            {
                if (itemCards[i].ItemStruct.BaseItem == baseItem)
                {
                    itemCards[i].CheckItem();
                }
            }
        }
    }
}