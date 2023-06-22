using System;
using System.Collections;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MadSmith.Scripts.SceneManagement
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _locationToLevelSelect;
        [SerializeField] private GameSceneSO _locationToNewGame;
        [SerializeField] private SaveSystem saveSystem;
        [SerializeField] private bool _showLoadScreen = default;
        
        [SerializeField] private GameDataSO currentGameData;
        
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO _loadLocation = default;

        [SerializeField] private VoidEventChannelSO onNewGameButton;
        [SerializeField] private VoidEventChannelSO onContinueButton;

        private void Start()
        {
            onNewGameButton.OnEventRaised += StartNewGame;
            onContinueButton.OnEventRaised += ContinuePreviousGame;
        }

        private void OnDestroy()
        {
            onNewGameButton.OnEventRaised -= StartNewGame;
            onContinueButton.OnEventRaised -= ContinuePreviousGame;
        }
        private void ContinuePreviousGame()
        {
            StartCoroutine(LoadSaveGame());
        }

        private void StartNewGame()
        {
            if (currentGameData.LevelScores.Count > 0)
            {
                _loadLocation.RaiseEvent(_locationToLevelSelect, _showLoadScreen);
            }
            else
            {
                _loadLocation.RaiseEvent(_locationToNewGame, _showLoadScreen);
            }
        }
        private IEnumerator LoadSaveGame() //TODO: verificar se funciona
        {
            // yield return StartCoroutine(saveSystem.LoadSavedInventory());

            // _saveSystem.LoadSavedQuestlineStatus();
            var locationGuid = saveSystem.saveData._locationID;
            var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);

            yield return asyncOperationHandle;

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                LocationSO locationSO = asyncOperationHandle.Result;
                _loadLocation.RaiseEvent(locationSO, _showLoadScreen);
            }
        }
        
    }   
}