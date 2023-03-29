using UnityEngine;
using UnityEngine.UI;

namespace _Developers.Vitor
{
    public class Item : MonoBehaviour
    {
        public Slider slider;
        private float _currentProcessTimeNormalized;
        public BaseItem baseItem;
        public float CurrentProcessTimeNormalized
        {
            get => _currentProcessTimeNormalized;
            set
            {
                _currentProcessTimeNormalized = value;
                if (slider != null)
                {
                    slider.value = value;
                }
                else
                {
                    Slider newSlider = GetComponentInChildren<Slider>();
                    if (newSlider)
                    {
                        slider = newSlider;
                    }
                }
                
            } 
        }
    }
}