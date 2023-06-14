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
        //TODO: adicinar dados das configurações de tela

        [SerializeField] private int widthValue;
        [SerializeField] private int heightValue;
        [SerializeField] private string displayTypeValue;
        [SerializeField] Locale _currentLocale = default;


        public int MasterVolume
        {
            get => masterVolume;
            set => masterVolume = value;
        }

        public int MusicVolume
        {
            get => musicVolume;
            set => musicVolume = value;
        }

        public int SfxVolume
        {
            get => sfxVolume;
            set => sfxVolume = value;
        }

        public int WidthValue
        {
            get => widthValue;
            set => widthValue = value;
        }

        public int HeightValue
        {
            get => heightValue;
            set => heightValue = value;
        }

        public string DisplayTypeValue
        {
            get => displayTypeValue;
            set => displayTypeValue = value;
        }
        public Locale CurrentLocale => _currentLocale;

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
            MasterVolume = 50;
            MusicVolume = 100;
            SfxVolume = 100;
            WidthValue = 1920;
            HeightValue = 1080;
            displayTypeValue = "";
        }

        public void SaveAudioSettings(int newMasterVolume, int newMusicVolume, int newSfxVolume)
        {
            MasterVolume = newMasterVolume;
            MusicVolume = newMusicVolume;
            SfxVolume = newSfxVolume;
        }

        public void SaveVideoSettings(string newDisplayType, int newWidthValue, int newHeightValue)
        {
            DisplayTypeValue = newDisplayType;
            WidthValue = newWidthValue;
            HeightValue = newHeightValue;
        }
        public void SaveLanguageSettings(Locale local)
        {
            _currentLocale = local;
        }
    }
}