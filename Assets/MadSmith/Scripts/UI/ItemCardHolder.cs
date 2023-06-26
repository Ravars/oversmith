using System;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.OLD;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class ItemCardHolder : MonoBehaviour
    {
        public ItemCard[] itemCards;
        public GameObject prefabCard;
        public Transform itemsHolder;

        public Texture blueImage;
        public Texture brownImage;
        public Texture orangeImage;
        public Texture pinkImage;
        // public string wagonName;
        public int npcId;
        public void SetItems(ItemStruct[] itemStructs, int npcId, BoxColor boxColor)
        {
            this.npcId = npcId;
            itemCards = new ItemCard[itemStructs.Length];
            for (int i = 0; i < itemStructs.Length; i++)
            {
                itemCards[i] = Instantiate(prefabCard, itemsHolder).GetComponent<ItemCard>();

                Texture a = null;
                switch (boxColor)
                {
                    case BoxColor.Pink:
                        a = pinkImage;
                        break;
                    case BoxColor.Orange:
                        a = orangeImage;
                        break;
                    case BoxColor.Brown:
                        a = brownImage;
                        break;
                    case BoxColor.Blue:
                        a = blueImage;
                        break;
                }
                
                
                
                itemCards[i].SetItem(itemStructs[i], i,a);
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