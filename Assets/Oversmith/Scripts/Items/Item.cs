using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Developers.Vitor
{
    public enum SoundType
    {
        SoundIn, SoundOut, CraftSound
    }
    
    public class Item : MonoBehaviour
    {
        public Slider slider;
        private float _currentProcessTimeNormalized;
        public BaseItem baseItem; 
        
        public AudioClip soundIn, soundOut, craftSound;
        private AudioSource sound;
        
        private void Awake()
        {
            sound = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.SoundIn:
                    sound.clip = soundIn;
                    break;
                case SoundType.SoundOut:
                    sound.clip = soundOut;
                    break;
                case SoundType.CraftSound:
                    sound.clip = craftSound;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(soundType), soundType, null);
            }
            sound.Play();
        }
        
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