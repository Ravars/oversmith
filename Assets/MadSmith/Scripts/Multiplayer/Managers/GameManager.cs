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

            // _uiGameplayManagerInstance = Instantiate(uiGameplayManagerPrefab);
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