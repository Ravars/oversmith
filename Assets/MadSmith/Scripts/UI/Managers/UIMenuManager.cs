using System;
using System.Collections;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.UI.Canvas;
using UnityEngine;

namespace MadSmith.Scripts.UI.Managers
{
    public class UIMenuManager : MonoBehaviour
    {
        private bool _hasSaveData;
        [SerializeField] private UIPopup _popupPanel = default;
        [SerializeField] private UISettingsController _settingsPanel = default;
        [SerializeField] private UIMainMenu _mainMenuPanel = default;
        [SerializeField] private UICredits _creditsPanel = default;
        [SerializeField] private SaveSystem _saveSystem = default;
        [SerializeField] private GameObject _mainMenuCamera;
        [SerializeField] private GameObject _characterSelectCamera;
        [SerializeField] private CharacterSelect _characterSelectUI;
        [SerializeField] private GameObject _creditsCamera;
        
        [SerializeField] private GameSceneSO _locationTutorial;

        //REMOVER
        [SerializeField] private Transform _characterToRotate;
        [SerializeField] private Transform _characterRotation;

        [SerializeField] private InputReader _inputReader = default;
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO _loadTutorialEvent = default;
        [SerializeField] private VoidEventChannelSO _startNewGameEvent = default;
        [SerializeField] private VoidEventChannelSO _continueGameEvent = default;
        private IEnumerator Start()
        {
            _inputReader.EnableMenuInput(); //TODO active this
            yield return new WaitForSeconds(0.4f); //waiting time for all scenes to be loaded
            SetMenuScreen();
        }
        
        void SetMenuScreen()
        {
            _hasSaveData = _saveSystem.Loaded;
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
            _mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
            _mainMenuPanel.NewGameButtonAction += OpenCharacterSelect;
            _mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
            _mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
            _mainMenuPanel.TutorialButtonAction += LoadTutorialLevel;
            _mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;

        }

        #region Settings
        public void OpenSettingsScreen()
        {
            _settingsPanel.gameObject.SetActive(true);
            _mainMenuPanel.gameObject.SetActive(false);
            _settingsPanel.Closed += CloseSettingsScreen;
        }
        public void CloseSettingsScreen()
        {
            _settingsPanel.Closed -= CloseSettingsScreen;
            _settingsPanel.gameObject.SetActive(false);
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }
        #endregion

        #region Credits
        public void OpenCreditsScreen()
        {
            _mainMenuCamera.SetActive(false);
            _creditsCamera.SetActive(true);
            _creditsPanel.gameObject.SetActive(true);
            _mainMenuPanel.gameObject.SetActive(false);
            _creditsPanel.OnCloseCredits += CloseCreditsScreen;
        }
        public void CloseCreditsScreen()
        {
            _creditsCamera.SetActive(false);
            _mainMenuCamera.SetActive(true);
            _creditsPanel.OnCloseCredits -= CloseCreditsScreen;
            _creditsPanel.gameObject.SetActive(false);
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }
        #endregion

        public void OpenCharacterSelect()
        {
            _characterSelectUI.Setup();
            _characterToRotate.rotation = _characterRotation.rotation;
            _mainMenuCamera.SetActive(false);
            _characterSelectCamera.SetActive(true);
            _characterSelectUI.gameObject.SetActive(true);
        }

        public void CloseCharacterSelect()
        {
            _characterSelectUI.gameObject.SetActive(false);
            _characterSelectCamera.SetActive(false);
            _mainMenuCamera.SetActive(true);
        }
        
        public void ButtonStartNewGameClicked()
        {
            Debug.Log(_hasSaveData);
            ConfirmStartNewGame();
        }

        void ConfirmStartNewGame()
        {
            _startNewGameEvent.RaiseEvent();
        }
        public void ShowExitConfirmationPopup()
        {
            _popupPanel.ConfirmationResponseAction += HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(true);
            _popupPanel.SetPopup(PopupType.Quit);
        }
        void HideExitConfirmationPopup(bool quitConfirmed)
        {
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            _popupPanel.gameObject.SetActive(false);
            if (quitConfirmed)
            {
                Application.Quit();
            }
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }
        private void OnDestroy()
        {
            _popupPanel.ConfirmationResponseAction -= HideExitConfirmationPopup;
            // _popupPanel.ConfirmationResponseAction -= StartNewGamePopupResponse;
        }

        private void LoadTutorialLevel()
        {
            _loadTutorialEvent.RaiseEvent(_locationTutorial, true, true);
        }
        
        
        
        
    }
}