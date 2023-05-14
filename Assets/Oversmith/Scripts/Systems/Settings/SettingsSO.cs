using Oversmith.Scripts.SaveSystem;
using UnityEngine;

namespace Oversmith.Scripts.Systems.Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Create new settings")]
    public class SettingsSO : ScriptableObject
    {
        [SerializeField] private float masterVolume;
        [SerializeField] private float musicVolume;
        [SerializeField] private float sfxVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set => masterVolume = value;
        }

        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = value;
        }

        public float SfxVolume
        {
            get => sfxVolume;
            set => sfxVolume = value;
        }
        public SettingsSO() { }

        public void LoadSavedSettings(Save savedFile)
        {
            MasterVolume = savedFile.masterVolume;
            MusicVolume = savedFile.musicVolume;
            SfxVolume = savedFile.sfxVolume;
        }

        public void SaveAudioSettings(float newMasterVolume, float newMusicVolume, float newSfxVolume)
        {
            MasterVolume = newMasterVolume;
            MusicVolume = newMusicVolume;
            SfxVolume = newSfxVolume;
        }
    }
}