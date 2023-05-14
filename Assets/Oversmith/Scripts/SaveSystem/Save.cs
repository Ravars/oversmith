using System;
using Oversmith.Scripts.Systems.Settings;
using UnityEngine;

namespace Oversmith.Scripts.SaveSystem
{
    [Serializable]
    public class Save
    {
        public string _locationID;
        public float masterVolume = default;
        public float musicVolume = default;
        public float sfxVolume = default;


        public void SaveSetting(SettingsSO settingsSo)
        {
            masterVolume = settingsSo.MasterVolume;
            musicVolume = settingsSo.MusicVolume;
            sfxVolume = settingsSo.SfxVolume;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }
}