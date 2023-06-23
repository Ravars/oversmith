using System;
using System.Collections;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.Utils;
using MadSmith.Scripts.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private GameDataSO _currentGameData = default; // MenuSO
        public int characterIndex = 0;
        public GameObject[] charactersPrefabs; 
        
        [SerializeField] private SaveSystem saveSystem = default;

        //Scenes
        [Header("Scenes control")] 
        public GameSceneSO[] sceneSos;
        public GameSceneSO CurrentSceneSo { get; private set; }
        
        
        
        [Header("Listening To")] 
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        [SerializeField] private IntEventChannelSO _onLevelCompleted = default;
        [SerializeField] private SettingsSO currentSettings;
        [SerializeField] private GameDataSO currentGameData;
        
        [Header("Broadcasting to")]
        [SerializeField] private VoidEventChannelSO _saveGameData = default;

        public bool Loaded { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            Loaded = saveSystem.LoadSaveDataFromDisk();
            if (Loaded)
            {
                currentSettings.LoadSavedSettings(saveSystem.saveData);
                currentGameData.LoadSavedSettings(saveSystem.saveData);
            }
            else
            {
                currentSettings.LoadDefaultSettings();
                currentGameData.LoadDefaultSettings();
            }
        }

        private void OnEnable()
        { 
            loadLocation.OnLoadingRequested += OnLoadingRequested;   
            _onLevelCompleted.OnEventRaised += OnLevelCompleted; 
        }

        private void OnLevelCompleted(int finalScore)
        {
            _currentGameData.SaveLevelScore(CurrentSceneSo.level,finalScore); // change to game manager
            _saveGameData.RaiseEvent();
        }

        private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        {
            CurrentSceneSo = arg0;
        }

        private void OnDisable()
        {
            loadLocation.OnLoadingRequested -= OnLoadingRequested;   
        }

        public static string CalculateScore(int finalScore)
        {
            Debug.Log(finalScore);
            return finalScore switch
            {
                >= 90 => "S",
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                >= 40 => "E",
                _ => "F"
            };
        } 
    }
}