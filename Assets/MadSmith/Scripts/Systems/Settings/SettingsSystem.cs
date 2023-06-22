using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace MadSmith.Scripts.Systems.Settings
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private SettingsSO currentSettings;
        [SerializeField] private SaveSystem saveSystem = default;
        
        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        [SerializeField] private VoidEventChannelSO saveGameDataEvent = default; // Mudar para Game manager system
        [SerializeField] private FloatEventChannelSO masterVolumeChannel;
        [SerializeField] private FloatEventChannelSO musicVolumeChannel;
        [SerializeField] private FloatEventChannelSO sfxVolumeChannel;


        private void Start()
        {
            SetCurrentSettings();
        }


        private void OnEnable()
        {
            saveSettingsEvent.OnEventRaised += SaveSettings;
            saveGameDataEvent.OnEventRaised += SaveGameData;
        }

        private void OnGameManagerAwake()
        {
            Debug.Log("Game Manager awaken settings system");
            SetCurrentSettings();
        }

        private void OnDisable()
        {
            saveSettingsEvent.OnEventRaised -= SaveSettings;
            saveGameDataEvent.OnEventRaised -= SaveGameData;
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

        private void SaveGameData()
        {
            saveSystem.SaveDataToDisk();
        }
        
    }
}