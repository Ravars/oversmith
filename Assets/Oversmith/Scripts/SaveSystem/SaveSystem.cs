using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Systems.Settings;
using UnityEngine;

namespace Oversmith.Scripts.SaveSystem

{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Save")]
    public class SaveSystem : ScriptableObject
    {
        
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        public Save saveData = new Save();
        [SerializeField] private SettingsSO _currentSettings;
        public string saveFilename = "save.mad";
        public string backupSaveFilename = "save.mad.bak";
        private void OnEnable()
        {
            saveSettingsEvent.OnEventRaised += OnSaveSettings;
        }
        private void OnDisable()
        {
            saveSettingsEvent.OnEventRaised -= OnSaveSettings;
        }
        private void OnSaveSettings()
        {
            saveData.SaveSetting(_currentSettings);
        }

        public void SaveDataToDisk()
        {


            if (FileManager.MoveFile(saveFilename, backupSaveFilename))
            {
                if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
                {
                    Debug.Log("Success save");
                }
            }
        }
    }
}