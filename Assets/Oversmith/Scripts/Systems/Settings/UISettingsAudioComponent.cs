using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsAudioComponent : MonoBehaviour
    {
        [SerializeField] private SettingsSO settingsSo;
        
        [Header("Broadcasting")]
        [SerializeField] private FloatEventChannelSO masterVolumeChannel;
        [SerializeField] private FloatEventChannelSO musicVolumeChannel;
        [SerializeField] private FloatEventChannelSO sfxVolumeChannel;
        private int _masterVolume;
        private int _musicVolume;
        private int _sfxVolume;
        
        public event UnityAction<float, float, float> _save = delegate { };

        private void OnEnable()
        {
            Debug.Log("Enable: " + settingsSo.MasterVolume);
            _masterVolume = (int)(settingsSo.MasterVolume * AudioManager.MaxVolume);
            _musicVolume = (int)(settingsSo.MusicVolume * AudioManager.MaxVolume);
            _sfxVolume = (int)(settingsSo.SfxVolume * AudioManager.MaxVolume);
        }

        public void IncreaseMasterVolume()
        {
            _masterVolume = Mathf.Clamp(_masterVolume+1,0,AudioManager.MaxVolume);
            SetMasterVolume();
        }
        public void DecreaseMasterVolume()
        {
            _masterVolume = Mathf.Clamp(_masterVolume-1,0,AudioManager.MaxVolume);
            SetMasterVolume();
        }

        private void SetMasterVolume()
        {
            //TODO: Update UI
           masterVolumeChannel.RaiseEvent(_masterVolume);
        }
        public void IncreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp(_musicVolume+1,0,AudioManager.MaxVolume);
            SetMusicVolume();
        }
        public void DecreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp(_musicVolume-1,0,AudioManager.MaxVolume);
            SetMusicVolume();
        }

        private void SetMusicVolume()
        {
            //TODO: Update UI
            musicVolumeChannel.RaiseEvent(_musicVolume);
        }
        public void IncreaseSfxVolume()
        {
            _sfxVolume = Mathf.Clamp(_sfxVolume+1,0,AudioManager.MaxVolume);
            SetSfxVolume();
        }
        public void DecreaseSfxVolume()
        {
            _sfxVolume = Mathf.Clamp(_sfxVolume-1,0,AudioManager.MaxVolume);
            SetSfxVolume();
        }

        private void SetSfxVolume()
        {
            //TODO: Update UI
            sfxVolumeChannel.RaiseEvent(_sfxVolume);
        }

        public void Save()
        {
            Debug.Log("Save settings: " + _masterVolume);
            settingsSo.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
            _save?.Invoke(_masterVolume, _musicVolume, _sfxVolume);
        }

        public void Reset()
        {
            
        }
        
    }
}