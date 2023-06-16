using System;
using System.Globalization;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Managers;
using Oversmith.Scripts.UI;
using Oversmith.Scripts.UI.SettingsScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsAudioComponent : MonoBehaviour //Sera usado no caso de usarmos abas
    {
        [SerializeField] private SettingsSO settingsSo;
        [SerializeField] private UISettingItemFiller _masterVolumeField;
        [SerializeField] private UISettingItemFiller _musicVolumeField;
        [SerializeField] private UISettingItemFiller _sfxVolumeField;

        [SerializeField] private UIGenericButton _saveButton;
        [SerializeField] private UIGenericButton _resetButton;
        
        [Header("Broadcasting")]
        [SerializeField] private FloatEventChannelSO masterVolumeChannel;
        [SerializeField] private FloatEventChannelSO musicVolumeChannel;
        [SerializeField] private FloatEventChannelSO sfxVolumeChannel;
        private int _masterVolume;
        private int _masterVolumeSaved;
        private int _musicVolume;
        private int _musicVolumeSaved;
        private int _sfxVolume;
        private int _sfxVolumeSaved;

        // [Header("Texts")] 
        // [SerializeField] private TextMeshProUGUI masterVolumeText;
        // [SerializeField] private TextMeshProUGUI musicVolumeText;
        // [SerializeField] private TextMeshProUGUI sfxVolumeText;
        
        public event UnityAction<int, int, int> _save = delegate { };

        private void OnEnable()
        {
            // _masterVolume = _masterVolumeSaved = (int)settingsSo.MasterVolume;
            // _musicVolume = _musicVolumeSaved = (int)settingsSo.MusicVolume;
            // _sfxVolume = _sfxVolumeSaved = (int)settingsSo.SfxVolume;
            // masterVolumeText.text = settingsSo.MasterVolume.ToString(CultureInfo.InvariantCulture);
            // musicVolumeText.text = settingsSo.MusicVolume.ToString(CultureInfo.InvariantCulture);
            // sfxVolumeText.text = settingsSo.SfxVolume.ToString(CultureInfo.InvariantCulture);

            _musicVolumeField.OnNextOption += IncreaseMusicVolume;
            _musicVolumeField.OnPreviousOption += DecreaseMusicVolume;

            _masterVolumeField.OnNextOption += IncreaseMasterVolume;
            _masterVolumeField.OnPreviousOption += DecreaseMasterVolume;

            _sfxVolumeField.OnNextOption += IncreaseSfxVolume;
            _sfxVolumeField.OnPreviousOption += DecreaseSfxVolume;

            _saveButton.Clicked += Save;
            _resetButton.Clicked += Reset;
            
        }

        private void OnDisable()
        {
            Reset();
            _musicVolumeField.OnNextOption -= IncreaseMusicVolume;
            _musicVolumeField.OnPreviousOption -= DecreaseMusicVolume;

            _masterVolumeField.OnNextOption -= IncreaseMasterVolume;
            _masterVolumeField.OnPreviousOption -= DecreaseMasterVolume;

            _sfxVolumeField.OnNextOption -= IncreaseSfxVolume;
            _sfxVolumeField.OnPreviousOption -= DecreaseSfxVolume;

            _saveButton.Clicked -= Save;
            _resetButton.Clicked -= Reset;
        }
        public void Setup(int musicVolume, int sfxVolume, int masterVolume)
        {
            _masterVolume = masterVolume;
            _musicVolume = sfxVolume;
            _sfxVolume = musicVolume;

            _masterVolumeSaved = _masterVolume;
            _musicVolumeSaved = _musicVolume;
            _sfxVolumeSaved = _sfxVolume;

            SetMusicVolumeField();
            SetSfxVolumeField();
            SetMasterVolumeField();
        }
        public void Save()
        {
            _masterVolumeSaved = _masterVolume;
            _musicVolumeSaved = _musicVolume;
            _sfxVolumeSaved = _sfxVolume; 
            // settingsSo.SaveAudioSettings(_masterVolume, _musicVolume, _sfxVolume);
            _save?.Invoke(_masterVolume, _musicVolume, _sfxVolume);
        }

        public void Reset()
        {
            Setup(_musicVolumeSaved,_sfxVolumeSaved,_masterVolumeSaved);
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
        private void SetMasterVolumeField()
        {
            int paginationCount = AudioManager.MaxVolume + 1;// adding a page in the pagination since the count starts from 0
            int selectedPaginationIndex = Mathf.RoundToInt(_masterVolume);
            string selectedOption = Mathf.RoundToInt(_masterVolume).ToString();

            SetMasterVolume();

            _masterVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);
        }
        private void SetMasterVolume()
        {
            masterVolumeChannel.RaiseEvent(_masterVolume); //raise event for volume change
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
        private void SetMusicVolumeField()
        {
            int paginationCount = AudioManager.MaxVolume + 1; // adding a page in the pagination since the count starts from 0
            int selectedPaginationIndex = Mathf.RoundToInt(_musicVolume);
            string selectedOption = Mathf.RoundToInt(_musicVolume).ToString();

            SetMusicVolume();

            _musicVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);


        }
        private void SetMusicVolume()
        {
            musicVolumeChannel.RaiseEvent(_musicVolume);//raise event for volume change
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
        private void SetSfxVolumeField()
        {
            int paginationCount = AudioManager.MaxVolume + 1;// adding a page in the pagination since the count starts from 0
            int selectedPaginationIndex = Mathf.RoundToInt(_sfxVolume);
            string selectedOption = Mathf.RoundToInt(_sfxVolume).ToString();

            SetSfxVolume();

            _sfxVolumeField.FillSettingField(paginationCount, selectedPaginationIndex, selectedOption);

        }
        private void SetSfxVolume()
        {
            sfxVolumeChannel.RaiseEvent(_sfxVolume); //raise event for volume change

        }
        #endregion

        private void SetValue(AudioGroups audioGroup, int step)
        {
            switch (audioGroup)
            {
                case AudioGroups.MasterVolume:
                    _masterVolume = Mathf.Clamp(_masterVolume+step,0,AudioManager.MaxVolume);
                    SetMasterVolumeField();
                    // masterVolumeChannel.RaiseEvent(_masterVolume);
                    // masterVolumeText.text = _masterVolume.ToString();
                    break;
                case AudioGroups.MusicVolume:
                    _musicVolume = Mathf.Clamp(_musicVolume+step,0,AudioManager.MaxVolume);
                    SetMusicVolumeField();
                    // musicVolumeChannel.RaiseEvent(_musicVolume);
                    // musicVolumeText.text = _musicVolume.ToString();
                    break;
                case AudioGroups.SFXVolume:
                    _sfxVolume = Mathf.Clamp(_sfxVolume+step,0,AudioManager.MaxVolume);
                    SetSfxVolumeField();
                    // sfxVolumeChannel.RaiseEvent(_sfxVolume);
                    // sfxVolumeText.text = _sfxVolume.ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(audioGroup), audioGroup, null);
            }
        }
    }
}