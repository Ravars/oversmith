using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField] private UISettingsAudioComponent audioComponent;
        [SerializeField] private UISettingsVideoComponent videoComponent;
        // [SerializeField] private UISettingsCentralizedComponent centralizedComponent;
        [SerializeField] private SettingsSO currentSetting;
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        
        public UnityAction Closed;
        private void OnEnable()
        {
            // centralizedComponent._save += CentralizedComponentOn_save;
            audioComponent._save += SaveAudioSettings;
            videoComponent._save += VideoComponentOn_save;
        }

        private void VideoComponentOn_save(string arg0, int arg1, int arg2)
        {
            throw new NotImplementedException();
        }

        private void CentralizedComponentOn_save(float arg0, float arg1, float arg2)
        {
            throw new NotImplementedException();
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