using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField] private UISettingsAudioComponent audioComponent;
        [SerializeField] private SettingsSO currentSetting;
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        
        public UnityAction Closed;
        private void OnEnable()
        {
            audioComponent._save += SaveAudioSettings;
        }
        private void SaveAudioSettings(float masterVolume, float musicVolume, float sfxVolume)
        {
            currentSetting.SaveAudioSettings(masterVolume,musicVolume,sfxVolume);
            saveSettingsEvent.RaiseEvent();
        }
        [ContextMenu("Close Menu")]
        public void CloseScreen()
        {
            Closed.Invoke();
        }


    }
}