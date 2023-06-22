using System.Collections.Generic;
using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.Systems.Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create new settings")]
    public class SettingsSO : ScriptableObject
    {
        //Sound
        [SerializeField] private int masterVolume;
        [SerializeField] private int musicVolume;
        [SerializeField] private int sfxVolume;
        
        //Localization
        [SerializeField] private Locale currentLocale = default;
        
        //Graphics
        [SerializeField] private int resolutionsIndex = default;
        [SerializeField] private int antiAliasingIndex = default;
        [SerializeField] private int graphicsQuality = default;
        [SerializeField] private bool isFullscreen = default;

        public int MasterVolume => masterVolume;
        public int MusicVolume => musicVolume;
        public int SfxVolume => sfxVolume;
        public Locale CurrentLocale => currentLocale;
        public int ResolutionsIndex => resolutionsIndex;
        public int AntiAliasingIndex => antiAliasingIndex;
        public int GraphicsQuality => graphicsQuality;
        public bool IsFullscreen => isFullscreen;

        public SettingsSO() { }

        public void LoadSavedSettings(Save savedFile)
        {
            masterVolume = savedFile.masterVolume;
            musicVolume = savedFile.musicVolume;
            sfxVolume = savedFile.sfxVolume;
            resolutionsIndex = savedFile.resolutionsIndex;
            antiAliasingIndex = savedFile.antiAliasingIndex;
            graphicsQuality = savedFile.graphicsQuality;
            isFullscreen = savedFile.isFullscreen;
            currentLocale = savedFile.currentLocale;
        }

        public void LoadDefaultSettings()
        {
            Debug.Log("Load default");
            masterVolume = 8;
            musicVolume = 8;
            sfxVolume = 10;
            graphicsQuality = 1;

            if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                // currentLocale = 
            }
            // widthValue = 1920;
            // heightValue = 1080;
            // displayTypeValue = "";
        }

        public void SaveAudioSettings(int newMasterVolume, int newMusicVolume, int newSfxVolume)
        {
            masterVolume = newMasterVolume;
            musicVolume = newMusicVolume;
            sfxVolume = newSfxVolume;
        }

        // public void SaveVideoSettings(string newDisplayType, int newWidthValue, int newHeightValue)
        // {
        //     DisplayTypeValue = newDisplayType;
        //     WidthValue = newWidthValue;
        //     HeightValue = newHeightValue;
        // }
        public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, int newGraphicsQuality, bool fullscreenState)
        {
            resolutionsIndex = newResolutionsIndex;
            antiAliasingIndex = newAntiAliasingIndex;
            graphicsQuality = newGraphicsQuality;
            isFullscreen = fullscreenState;
        }
        public void SaveLanguageSettings(Locale local)
        {
            currentLocale = local;
        }
    }
}