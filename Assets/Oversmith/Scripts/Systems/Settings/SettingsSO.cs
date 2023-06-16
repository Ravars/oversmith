using Oversmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace Oversmith.Scripts.Systems.Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create new settings")]
    public class SettingsSO : ScriptableObject
    {
        [SerializeField] private int masterVolume;
        [SerializeField] private int musicVolume;
        [SerializeField] private int sfxVolume;

        [SerializeField] private int widthValue;
        [SerializeField] private int heightValue;
        [SerializeField] private string displayTypeValue;
        [SerializeField] Locale _currentLocale = default;
        
        //Graphics
        [SerializeField] int _resolutionsIndex = default;
        [SerializeField] bool _isFullscreen = default;
        
        public int MasterVolume => masterVolume;
        public int MusicVolume => musicVolume;
        public int SfxVolume => sfxVolume;
        public int WidthValue => widthValue;
        public int HeightValue => heightValue;
        public string DisplayTypeValue => displayTypeValue;
        public Locale CurrentLocale => _currentLocale;
        public int ResolutionsIndex => _resolutionsIndex;
        public bool IsFullscreen => _isFullscreen;

        public SettingsSO() { }

        public void LoadSavedSettings(Save savedFile)
        {
            Debug.Log(savedFile.masterVolume);
            masterVolume = savedFile.masterVolume;
            musicVolume = savedFile.musicVolume;
            sfxVolume = savedFile.sfxVolume;
            widthValue = savedFile.widthValue;
            heightValue = savedFile.heightValue;
            displayTypeValue = savedFile.displayTypeValue;
        }

        public void LoadDefaultSettings()
        {
            Debug.Log("Load default");
            masterVolume = 50;
            musicVolume = 100;
            sfxVolume = 100;
            widthValue = 1920;
            heightValue = 1080;
            displayTypeValue = "";
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
        public void SaveGraphicsSettings(int newResolutionsIndex, int newAntiAliasingIndex, float newShadowDistance, bool fullscreenState)
        {
            _resolutionsIndex = newResolutionsIndex;
            // _antiAliasingIndex = newAntiAliasingIndex;
            // _shadowDistance = newShadowDistance;
            _isFullscreen = fullscreenState;
        }
        public void SaveLanguageSettings(Locale local)
        {
            _currentLocale = local;
        }
    }
}