using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField] private UISettingsAudioComponent audioComponent;
        [SerializeField] private SettingsSO currentSetting;
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        private void OnEnable()
        {
            audioComponent._save += SaveAudioSettings;
        }

        private void SaveAudioSettings(float masterVolume, float musicVolume, float sfxVolume)
        {
            currentSetting.SaveAudioSettings(masterVolume,musicVolume,sfxVolume);
            saveSettingsEvent.RaiseEvent();
        }
    }
}