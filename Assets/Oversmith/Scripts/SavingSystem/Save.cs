using System;
using Oversmith.Scripts.Systems.Settings;
using UnityEngine;

namespace Oversmith.Scripts.SavingSystem
{
    [Serializable]
    public class Save
    {
        public string _locationID;
        //Audio Settings
        public float masterVolume = default;
        public float musicVolume = default;
        public float sfxVolume = default;
        //Video Settings
        public int widthValue = default;
        public int heightValue = default;
        public string displayTypeValue = default;


        public void SaveSetting(SettingsSO settingsSo)
        {
            masterVolume = settingsSo.MasterVolume;
            musicVolume = settingsSo.MusicVolume;
            sfxVolume = settingsSo.SfxVolume;
            widthValue = settingsSo.WidthValue;
            heightValue = settingsSo.HeightValue;
            displayTypeValue = settingsSo.DisplayTypeValue;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json)
        {
            Debug.Log("Json: " + json);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}