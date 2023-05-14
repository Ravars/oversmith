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
        private int _maxVolume = 10;
        public event UnityAction<float, float, float> _save = delegate { };

        public void IncreaseMasterVolume()
        {
            _masterVolume = Mathf.Clamp(_masterVolume+1,0,10);
            SetMasterVolume();
        }
        public void DecreaseMasterVolume()
        {
            _masterVolume = Mathf.Clamp(_masterVolume-1,0,10);
            SetMasterVolume();
        }

        private void SetMasterVolume()
        {
            //TODO: Update UI
           masterVolumeChannel.RaiseEvent(_masterVolume/(float)10);
        }
        public void IncreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp(_musicVolume+1,0,10);
            SetMusicVolume();
        }
        public void DecreaseMusicVolume()
        {
            _musicVolume = Mathf.Clamp(_musicVolume-1,0,10);
            SetMusicVolume();
        }

        private void SetMusicVolume()
        {
            //TODO: Update UI
            musicVolumeChannel.RaiseEvent((float)_musicVolume/(float)10);
        }
        public void IncreaseSfxVolume()
        {
            _sfxVolume = Mathf.Clamp(_sfxVolume+1,0,10);
            SetSfxVolume();
        }
        public void DecreaseSfxVolume()
        {
            _sfxVolume = Mathf.Clamp(_sfxVolume-1,0,10);
            SetSfxVolume();
        }

        private void SetSfxVolume()
        {
            //TODO: Update UI
            sfxVolumeChannel.RaiseEvent(_sfxVolume/(float)10);
        }

        public void Save()
        {
            settingsSo.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
            _save?.Invoke(_masterVolume, _musicVolume, _sfxVolume);
        }

        public void Reset()
        {
            
        }
        
    }
}