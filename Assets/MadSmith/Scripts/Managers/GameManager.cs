// using System;
// using System.Collections;
// using MadSmith.Scripts.Events.ScriptableObjects;
// using MadSmith.Scripts.SavingSystem;
// using MadSmith.Scripts.SceneManagement.ScriptableObjects;
// using MadSmith.Scripts.Systems.Settings;
// using MadSmith.Scripts.Utils;
// using MadSmith.Scripts.Menu;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace MadSmith.Scripts.Managers
// {
//     public class GameManager : PersistentSingleton<GameManager>
//     {
//         [SerializeField] private GameDataSO _currentGameData = default; // MenuSO
//         public int characterIndex = 0;
//         public GameObject[] charactersPrefabs; 
//         
//         [SerializeField] private SaveSystem saveSystem = default;
//
//         //Scenes
//         [Header("Scenes control")] 
//         public GameSceneSO[] sceneSos;
//         public GameSceneSO CurrentSceneSo { get; private set; }
//         
//         
//         
//         [Header("Listening To")] 
//         [SerializeField] private LoadEventChannelSO loadLocation = default;
//         [SerializeField] private FloatEventChannelSO _onLevelCompleted = default;
//         [SerializeField] private SettingsSO currentSettings;
//         [SerializeField] private GameDataSO currentGameData;
//         
//         [Header("Broadcasting to")]
//         [SerializeField] private VoidEventChannelSO _saveGameData = default;
//
//         public bool Loaded { get; private set; }
//         protected override void Awake()
//         {
//             base.Awake();
//             Loaded = saveSystem.LoadSaveDataFromDisk();
//             if (Loaded)
//             {
//                 currentSettings.LoadSavedSettings(saveSystem.saveData);
//                 currentGameData.LoadSavedSettings(saveSystem.saveData);
//             }
//             else
//             {
//                 currentSettings.LoadDefaultSettings();
//                 currentGameData.LoadDefaultSettings();
//             }
//         }
//
//         private void OnEnable()
//         { 
//             // loadLocation.OnLoadingRequested += OnLoadingRequested;   
//             _onLevelCompleted.OnEventRaised += OnLevelCompleted; 
//         }
//
//         private void OnLevelCompleted(float finalScore)
//         {
//             _currentGameData.SaveLevelScore(CurrentSceneSo.level,(int)finalScore); // change to game manager
//             _saveGameData.RaiseEvent();
//         }
//
//         // private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
//         // {
//         //     CurrentSceneSo = arg0;
//         // }
//
//         // private void OnDisable()
//         // {
//         //     loadLocation.OnLoadingRequested -= OnLoadingRequested;   
//         // }
//
//         // public GameSceneSO GetSceneSo()
//         // {
//         //     GameSceneSO gameSceneSo;
//         //     if (CurrentSceneSo == null) return sceneSos[0];
//         //     switch (CurrentSceneSo.name)
//         //     {
//         //         case "Level01-1":
//         //             gameSceneSo = sceneSos[1];
//         //             break;
//         //         case "Level01-2":
//         //             gameSceneSo = sceneSos[2];
//         //             break;
//         //         case "MenuPrincipal":
//         //             gameSceneSo = sceneSos[0];
//         //             break;
//         //         default:
//         //             gameSceneSo = sceneSos[0];
//         //             break;
//         //     }
//         //
//         //     return gameSceneSo;
//         // }
//
//         // public void SetGameSceneSo(string sceneName)
//         // {
//         //     GameSceneSO gameSceneSo;
//         //     switch (sceneName)
//         //     {
//         //         case "Level01-1":
//         //             gameSceneSo = sceneSos[1];
//         //             break;
//         //         case "Level01-2":
//         //             gameSceneSo = sceneSos[2];
//         //             break;
//         //         case "MenuPrincipal":
//         //             gameSceneSo = sceneSos[0];
//         //             break;
//         //         default:
//         //             gameSceneSo = sceneSos[0];
//         //             break;
//         //     }
//         //
//         //     CurrentSceneSo = gameSceneSo;
//         // }
//
//         public static string CalculateScore(int finalScore)
//         {
//             return finalScore switch
//             {
//                 >= 90 => "S",
//                 >= 70 => "A",
//                 >= 50 => "B",
//                 >= 30 => "C",
//                 _ => "F"
//             };
//         } 
//     }
// }