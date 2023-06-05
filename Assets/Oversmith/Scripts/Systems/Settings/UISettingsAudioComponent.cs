using System;
using System.Globalization;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Managers;
using TMPro;
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

        [Header("Texts")] 
        [SerializeField] private TextMeshProUGUI masterVolumeText;
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private TextMeshProUGUI sfxVolumeText;
        
        public event UnityAction<float, float, float> _save = delegate { };

        private void OnEnable()
        {
            _masterVolume = (int)settingsSo.MasterVolume;
            _musicVolume = (int)settingsSo.MusicVolume;
            _sfxVolume = (int)settingsSo.SfxVolume;
            masterVolumeText.text = settingsSo.MasterVolume.ToString(CultureInfo.InvariantCulture);
            musicVolumeText.text = settingsSo.MusicVolume.ToString(CultureInfo.InvariantCulture);
            sfxVolumeText.text = settingsSo.SfxVolume.ToString(CultureInfo.InvariantCulture);
        }

        #region Master Volume
        public void IncreaseMasterVolume()
        {
            SetValue(AudioGroups.MasterVolume, AudioManager.StepVolume);
        }
        public void DecreaseMasterVolume()
        {
            SetValue(AudioGroups.MasterVolume, -AudioManager.StepVolume);
        }
        #endregion

        #region Music Volume
        public void IncreaseMusicVolume()
        {
            SetValue(AudioGroups.MusicVolume, AudioManager.StepVolume);
        }
        public void DecreaseMusicVolume()
        {
            SetValue(AudioGroups.MusicVolume, -AudioManager.StepVolume);
        }
        #endregion

        #region SFX Volume
        public void IncreaseSfxVolume()
        {
            SetValue(AudioGroups.SFXVolume, AudioManager.StepVolume);
        }
        public void DecreaseSfxVolume()
        {
            SetValue(AudioGroups.SFXVolume, -AudioManager.StepVolume);
        }
        #endregion

        private void SetValue(AudioGroups audioGroup, int step)
        {
            switch (audioGroup)
            {
                case AudioGroups.MasterVolume:
                    _masterVolume = Mathf.Clamp(_masterVolume+step,0,AudioManager.MaxVolume);
                    masterVolumeChannel.RaiseEvent(_masterVolume);
                    masterVolumeText.text = _masterVolume.ToString();
                    break;
                case AudioGroups.MusicVolume:
                    _musicVolume = Mathf.Clamp(_musicVolume+step,0,AudioManager.MaxVolume);
                    musicVolumeChannel.RaiseEvent(_musicVolume);
                    musicVolumeText.text = _musicVolume.ToString();
                    break;
                case AudioGroups.SFXVolume:
                    _sfxVolume = Mathf.Clamp(_sfxVolume+step,0,AudioManager.MaxVolume);
                    sfxVolumeChannel.RaiseEvent(_sfxVolume);
                    sfxVolumeText.text = _sfxVolume.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioGroup), audioGroup, null);
            }
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