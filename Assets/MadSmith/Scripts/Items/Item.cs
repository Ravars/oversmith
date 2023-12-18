using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Items
{
    public enum SoundType
    {
        SoundIn, SoundOut, CraftSound
    }
    
    public class Item : NetworkBehaviour
    {
        public Slider slider;
        [SyncVar] public float currentProcessTimeNormalized;
        public BaseItem baseItem; 
        private CraftingTableType _lastCraftingTable;
        
        public AudioClip soundIn, soundOut, craftSound;
        private AudioSource sound;
        
        private void Awake()
        {
            sound = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundType soundType)
        {
            if (sound == null) return;
            
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
            get => currentProcessTimeNormalized;
            set
            {
                currentProcessTimeNormalized = value;
                if (slider != null)
                {
                    // slider.value = value;
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
        public CraftingTableType LastCraftingTable 
        {
            get => _lastCraftingTable;
            set
            {
                if (_lastCraftingTable != value)
                {
                    _lastCraftingTable = value;
                    CurrentProcessTimeNormalized = 0;
                }
            }
        }
    }
}