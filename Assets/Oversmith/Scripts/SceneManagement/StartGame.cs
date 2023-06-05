using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SavingSystem;
using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;

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

        private bool _hasSaveData;

        private void Start()
        {
            _hasSaveData = saveSystem.LoadSaveDataFromDisk();
            onNewGameButton.OnEventRaised += StartNewGame;
        }

        private void StartNewGame()
        {
            _hasSaveData = false;
            saveSystem.WriteEmptySaveFile();
            saveSystem.SetNewGameData();
            _loadLocation.RaiseEvent(_locationsToLoad, _showLoadScreen);
        }
        
    }   
}