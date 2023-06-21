using System;
using Oversmith.Scripts.Systems.Settings;
using UnityEngine;
using UnityEngine.Localization;

namespace Oversmith.Scripts.SavingSystem
{
    [Serializable]
    public class Save
    {
        public string _locationID;
        //Audio Settings
        public int masterVolume = default;
        public int musicVolume = default;
        public int sfxVolume = default;
        public int resolutionsIndex = default;
        public int antiAliasingIndex = default;
        public int graphicsQuality = default;
        public bool isFullscreen = default;
        public Locale currentLocale = default;


        public void SaveSetting(SettingsSO settingsSo)
        {
            masterVolume = settingsSo.MasterVolume;
            musicVolume = settingsSo.MusicVolume;
            sfxVolume = settingsSo.SfxVolume;
            resolutionsIndex = settingsSo.ResolutionsIndex;
            antiAliasingIndex = settingsSo.AntiAliasingIndex;
            graphicsQuality = settingsSo.GraphicsQuality;
            isFullscreen = settingsSo.IsFullscreen;
            currentLocale = settingsSo.CurrentLocale;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}