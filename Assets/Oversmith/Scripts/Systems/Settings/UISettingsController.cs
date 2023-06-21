using System;
using System.Collections.Generic;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Input;
using Oversmith.Scripts.UI.SettingsScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Oversmith.Scripts.Systems.Settings
{
    [System.Serializable]
    public enum SettingFieldType
    {
        Language,
        Volume_SFx,
        Volume_Music,
        Resolution,
        FullScreen,
        ShadowDistance,
        AntiAliasing,
        ShadowQuality,
        Volume_Master,
        GraphicsQuality
    }
    [System.Serializable]
    public class SettingTab
    {
        public SettingsType settingTabsType;
        public LocalizedString title;
    }
    [System.Serializable]
    public class SettingField
    {
        public SettingsType settingTabsType;
        public SettingFieldType settingFieldType;
        public LocalizedString title;
    }
    public enum SettingsType
    {
        Language,
        Video,
        Audio,
    }
    public class UISettingsController : MonoBehaviour
    {
        [SerializeField] private UISettingsAudioComponent audioComponent;
        [SerializeField] private UISettingsLanguageComponent _languageComponent;
        [SerializeField] private UISettingsGraphicsComponent _graphicsComponent;
        [SerializeField] private UISettingsTabsFiller _settingTabFiller = default;
        [SerializeField] private SettingsSO currentSetting;
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        [SerializeField] private List<SettingsType> _settingTabsList = new ();
        private SettingsType _selectedTab = SettingsType.Audio;
        [SerializeField] private InputReader _inputReader;
        
        public UnityAction Closed;
        private void OnEnable()
        {
            // centralizedComponent._save += CentralizedComponentOn_save;
            audioComponent._save += SaveAudioSettings;
            _languageComponent._save += SaveLanguageSettings;
            // videoComponent._save += VideoComponentOn_save;
            _graphicsComponent._save += SaveGraphicsSettings;
            _inputReader.MenuCloseEvent += CloseScreen;
            _inputReader.TabSwitched += SwitchTab;
            _settingTabFiller.FillTabs(_settingTabsList);
            _settingTabFiller.ChooseTab += OpenSetting;

            OpenSetting(SettingsType.Audio);
        }

        private void OnDisable()
        {
            audioComponent._save -= SaveAudioSettings;
            _languageComponent._save -= SaveLanguageSettings;
            _graphicsComponent._save -= SaveGraphicsSettings;
            _inputReader.MenuCloseEvent -= CloseScreen;
            _inputReader.TabSwitched -= SwitchTab;
            // _settingTabFiller.ChooseTab -= OpenSetting; //TODO: Verificar
        }
        public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, int newGraphicsQuality, bool fullscreenState)
        {
            currentSetting.SaveGraphicsSettings(newResolutionsIndex, newAntiAliasingIndex, newGraphicsQuality, fullscreenState);
            saveSettingsEvent.RaiseEvent();
        }
        public void SaveLanguageSettings(Locale local)
        {
            currentSetting.SaveLanguageSettings(local);
            saveSettingsEvent.RaiseEvent();
        }

        private void SaveAudioSettings(int masterVolume, int musicVolume, int sfxVolume)
        {
            currentSetting.SaveAudioSettings(masterVolume,musicVolume,sfxVolume);
            saveSettingsEvent.RaiseEvent();
        }
        [ContextMenu("Close Menu")]
        public void CloseScreen()
        {
            Closed.Invoke();
        }
        void SwitchTab(float orientation)
        {

            if (orientation != 0)
            {
                bool isLeft = orientation < 0;
                int initialIndex = _settingTabsList.FindIndex(o => o == _selectedTab);
                if (initialIndex != -1)
                {
                    if (isLeft)
                    {
                        initialIndex--;
                    }
                    else
                    {
                        initialIndex++;
                    }

                    initialIndex = Mathf.Clamp(initialIndex, 0, _settingTabsList.Count - 1);
                }

                OpenSetting(_settingTabsList[initialIndex]);
            }
        }
        void OpenSetting(SettingsType settingType)
        {
            _selectedTab = settingType;
            switch (settingType)
            {
                case SettingsType.Language:
                    currentSetting.SaveLanguageSettings(currentSetting.CurrentLocale);
                    break;  
                case SettingsType.Video:
                    _graphicsComponent.Setup();
                    break;
                case SettingsType.Audio:
                    audioComponent.Setup(currentSetting.MusicVolume, currentSetting.SfxVolume, currentSetting.MasterVolume);
                    break;
                default:
                    break;
            }

            _languageComponent.gameObject.SetActive(settingType == SettingsType.Language);
            _graphicsComponent.gameObject.SetActive((settingType == SettingsType.Video));
            audioComponent.gameObject.SetActive(settingType == SettingsType.Audio);
            _settingTabFiller.SelectTab(settingType);
        }


    }
}