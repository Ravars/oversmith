using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class GameManager : PersistentNetworkSingleton<GameManager>
    {
        [Header("Scenes control")] 
        public GameSceneSO[] sceneSos;
        [SyncVar] public int currentSceneIndex;
        [SerializeField] private SaveSystem saveSystem = default;
        // public UIManager uiGameplayManagerPrefab;
        // private UIManager _uiGameplayManagerInstance;
        
        [Header("Listening To")] 
        [SerializeField] private SettingsSO currentSettings;
        [SerializeField] private GameDataSO currentGameData; 
        [SerializeField] private FloatEventChannelSO onLevelCompleted;
        [SerializeField] private LoadEventChannelSO _loadNextLevel = default;
        [Header("Broadcasting to")]
        [SerializeField] private VoidEventChannelSO _saveGameData = default;
        public bool Loaded { get; private set; }
        private MadSmithNetworkRoomManager _manager;
        public MadSmithNetworkRoomManager Manager
        { get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
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
            onLevelCompleted.OnEventRaised += OnLevelCompleted;
            _loadNextLevel.OnLoadingRequested += OnLoadingRequested;

            // _uiGameplayManagerInstance = Instantiate(uiGameplayManagerPrefab);
        }

        private void OnDisable()
        {
            onLevelCompleted.OnEventRaised -= OnLevelCompleted;
            _loadNextLevel.OnLoadingRequested -= OnLoadingRequested;
        }

        private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        {
            string newSceneName = ((LocationSO)arg0).sceneName;
            Debug.Log("newSceneName; " + newSceneName);
            Manager.GameplayScene = newSceneName;
            Manager.ServerChangeScene(newSceneName);
        }

        private void OnLevelCompleted(float finalScore)
        {
            currentGameData.SaveLevelScore((Levels)currentSceneIndex,(int)finalScore); // change to game manager
            _saveGameData.RaiseEvent();
        }

        // [Command]
        public void SetGameSceneSo(int gameSceneIndex)
        {
            currentSceneIndex = gameSceneIndex;
            // CurrentSceneSo = sceneSos[gameSceneIndex];
        }

        public GameSceneSO GetCurrentScene()
        {
            return sceneSos[currentSceneIndex];
        }
        
        public static string CalculateScore(int finalScore)
        {
             return finalScore switch
             {
                 >= 90 => "S",
                 >= 70 => "A",
                 >= 50 => "B",
                 >= 30 => "C",
                 _ => "F"
             };
         }
    }
}