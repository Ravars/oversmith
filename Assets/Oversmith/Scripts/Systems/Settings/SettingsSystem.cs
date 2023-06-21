using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
            }
            currentSettings.LoadSavedSettings(saveSystem.saveData);
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
            masterVolumeChannel.RaiseEvent(currentSettings.MasterVolume);
            musicVolumeChannel.RaiseEvent(currentSettings.MusicVolume);
            sfxVolumeChannel.RaiseEvent(currentSettings.SfxVolume);
            Resolution currentResolution = Screen.currentResolution; // get a default resolution in case saved resolution doesn't exist in the resolution List
            if (currentSettings.ResolutionsIndex < Screen.resolutions.Length)
            {
                currentResolution = Screen.resolutions[currentSettings.ResolutionsIndex];
            }
            Screen.SetResolution(currentResolution.width, currentResolution.height, currentSettings.IsFullscreen);
            QualitySettings.SetQualityLevel(currentSettings.GraphicsQuality);
            LocalizationSettings.SelectedLocale = currentSettings.CurrentLocale;
            //TODO: Add other configs
        }

        private void SaveSettings()
        {
            saveSystem.SaveDataToDisk();
        }
        
    }
}