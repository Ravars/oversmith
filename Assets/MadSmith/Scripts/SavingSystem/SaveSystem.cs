using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using UnityEngine;

namespace MadSmith.Scripts.SavingSystem

{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Save")]
    public class SaveSystem : ScriptableObject
    {
        
        [SerializeField] private VoidEventChannelSO saveSettingsEvent = default;
        [SerializeField] private VoidEventChannelSO saveGameEvent = default;
        [SerializeField] private LoadEventChannelSO _loadLocation = default;
        public Save saveData = new Save();
        [SerializeField] private SettingsSO _currentSettings;
        [SerializeField] private GameDataSO _currentGameData;
        public string saveFilename = "save.mad";
        public string backupSaveFilename = "save.mad.bak";
        public bool Loaded { get; private set; }
        private void OnEnable()
        {
            saveSettingsEvent.OnEventRaised += OnSaveSettings;
            saveGameEvent.OnEventRaised += OnSaveGame;
            // _loadLocation.OnLoadingRequested += CacheLoadLocations;
        }
        private void OnDisable()
        {
            saveSettingsEvent.OnEventRaised -= OnSaveSettings;
            saveGameEvent.OnEventRaised -= OnSaveGame;
            // _loadLocation.OnLoadingRequested -= CacheLoadLocations;
        }
        private void OnSaveSettings()
        {
            saveData.SaveSetting(_currentSettings);
        }

        private void OnSaveGame()
        {
            saveData.SaveGame(_currentGameData);
        }
        private void CacheLoadLocations(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            LocationSO locationSO = locationToLoad as LocationSO;
            if (locationSO)
            {
                saveData._locationID = locationSO.Guid;
            }

            SaveDataToDisk();
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
        public bool LoadSaveDataFromDisk()
        {
            if (FileManager.LoadFromFile(saveFilename, out var json))
            {
                if (string.IsNullOrEmpty(json))
                {
                    Loaded = false;
                    return false;
                }
                saveData.LoadFromJson(json);
                Loaded = true;
                return true;
            }
            Loaded = false;
            return false;
        }
        public void WriteEmptySaveFile()
        {
            FileManager.WriteToFile(saveFilename, "");
        }

        public void SetNewGameData()
        {
            FileManager.WriteToFile(saveFilename, "");
            //TODO: init save data
            SaveDataToDisk();

        }
    }
}