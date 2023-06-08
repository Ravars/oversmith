using System;
using System.Collections;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SavingSystem;
using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Oversmith.Scripts.SceneManagement
{
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _locationsToLoad;
        [SerializeField] private SaveSystem saveSystem;
        [SerializeField] private bool _showLoadScreen = default;
        
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
            saveSystem.WriteEmptySaveFile();
            saveSystem.SetNewGameData();
            _loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
        }
        private IEnumerator LoadSaveGame()
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