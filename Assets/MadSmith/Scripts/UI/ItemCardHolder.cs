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
        public int id;
        public BaseItem baseItem;
        [SerializeField] private RawImage rawImage;
        public Slider slider;

        public void SetItem(BaseItem item, int newId)
        {
            id = newId;
            baseItem = item;
            rawImage.texture = item.image;
        }
    }
}