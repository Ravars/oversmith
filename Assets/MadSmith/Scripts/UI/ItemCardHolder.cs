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
        public Transform itemsHolder;

        public Texture blueImage;
        public Texture brownImage;
        public Texture orangeImage;
        public Texture pinkImage;

        public int id;

        public BaseItem baseItem;

        public void SetItem(BaseItem item, int id)
        {
            this.id = id;

            baseItem = item;

            itemsHolder.GetComponent<RawImage>().texture = item.image;
        }
    }
}