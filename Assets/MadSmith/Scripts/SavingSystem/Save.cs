using System;
using System.Collections.Generic;
using MadSmith.Scripts.Systems.Settings;
using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.SavingSystem
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
        public List<SerializedLevelScore> levelScores;


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

        public void SaveGame(GameDataSO gameDataSo)
        {
            levelScores = new List<SerializedLevelScore>();
            levelScores.AddRange(gameDataSo.LevelScores);
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

    public enum Levels
    {
        None,
        Level01,
        Level02,
        Level03,
        Level04,
        Level05,
        Level06,
        Level07,
        Level08,
        Level09,
    }

    [Serializable]
    public class SerializedLevelScore
    {
        public int score;
        public Levels Level;
    }
}