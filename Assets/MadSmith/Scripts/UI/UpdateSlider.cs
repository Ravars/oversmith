using System;
using MadSmith.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class UpdateSlider : MonoBehaviour
    {
        public Item item;
        public Slider slider;
        private void FixedUpdate()
        {
            if (item.currentProcessTimeNormalized > 0)
            {
                slider.value = item.currentProcessTimeNormalized;
            }
        }
    }
}