using System;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.OLD;
using UnityEngine;
using UnityEngine.UI;

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

        public Image[] imageColors;

        public Color blue;
        public Color brown;
        public Color orange;
        public Color pink;
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
                        foreach (var imageColor in imageColors)
                        {
                            imageColor.color = pink;
                        }
                        break;
                    case BoxColor.Orange:
                        a = orangeImage;
                        foreach (var imageColor in imageColors)
                        {
                            imageColor.color = orange;
                        }
                        break;
                    case BoxColor.Brown:
                        a = brownImage;
                        foreach (var imageColor in imageColors)
                        {
                            imageColor.color = brown;
                        }
                        break;
                    case BoxColor.Blue:
                        a = blueImage;
                        foreach (var imageColor in imageColors)
                        {
                            imageColor.color = blue;
                        }
                        break;
                }
                itemCards[i].SetItem(itemStructs[i], i,a);
            }

            itemsHolder.GetComponent<RawImage>().texture = itemStructs[0].BaseItem.image;
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