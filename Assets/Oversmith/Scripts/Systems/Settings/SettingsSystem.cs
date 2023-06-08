using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SavingSystem;
using UnityEngine;

namespace Oversmith.Scripts.Systems.Settings
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        [SerializeField] private SettingsSO currentSettings;
        [SerializeField] private SaveSystem saveSystem = default;
        
        [SerializeField] private FloatEventChannelSO masterVolumeChannel;
        [SerializeField] private FloatEventChannelSO musicVolumeChannel;
        [SerializeField] private FloatEventChannelSO sfxVolumeChannel;
        
        public bool Loaded { get; private set; }

        private void Awake()
        {
            Loaded = saveSystem.LoadSaveDataFromDisk();
            Debug.Log("Loaded: " + Loaded);
            if (!Loaded)
            {
                currentSettings.LoadDefaultSettings();
                // currentSettings.SaveAudioSettings();
            }
            currentSettings.LoadSavedSettings(saveSystem.saveData);
            Debug.Log("Awake Settings system: Master: " + currentSettings.MasterVolume);
            SetCurrentSettings();
        }

        private void OnEnable()
        {
            saveSettingsEvent.OnEventRaised += SaveSettings;
        }

        private void OnDisable()
        {
            saveSettingsEvent.OnEventRaised -= SaveSettings;
        }

        private void SetCurrentSettings()
        {
            Debug.Log("SetCurrentSettings: " + currentSettings.MasterVolume);
            masterVolumeChannel.RaiseEvent(currentSettings.MasterVolume);
            musicVolumeChannel.RaiseEvent(currentSettings.MusicVolume);
            sfxVolumeChannel.RaiseEvent(currentSettings.SfxVolume);
            //TODO: Add other configs
        }

        private void SaveSettings()
        {
            saveSystem.SaveDataToDisk();
        }
        
    }
}