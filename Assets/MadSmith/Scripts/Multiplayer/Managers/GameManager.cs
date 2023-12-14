using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("Scenes control")] 
        public GameSceneSO[] sceneSos;
        public GameSceneSO CurrentSceneSo { get; private set; }
        
        [SerializeField] private SaveSystem saveSystem = default;
        
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
        }
        
        public void SetGameSceneSo(GameSceneSO gameSceneSo)
        {
            CurrentSceneSo = gameSceneSo;
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