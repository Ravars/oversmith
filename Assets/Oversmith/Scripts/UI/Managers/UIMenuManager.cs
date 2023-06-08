using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SavingSystem;
using Oversmith.Scripts.Systems.Settings;
using Oversmith.Scripts.UI.Canvas;
using UnityEngine;

namespace Oversmith.Scripts.UI.Managers
{
    public class UIMenuManager : MonoBehaviour
    {
        private bool _hasSaveData;
        [SerializeField] private UISettingsController _settingsPanel = default;
        [SerializeField] private UIMainMenu _mainMenuPanel = default;
        [SerializeField] private UICredits _creditsPanel = default;
        [SerializeField] private SaveSystem _saveSystem = default;
        
        
        [Header("Broadcasting on")]
        [SerializeField] private VoidEventChannelSO _startNewGameEvent = default;
        [SerializeField] private VoidEventChannelSO _continueGameEvent = default;
        private void Start()
        {
            SetMenuScreen();
        }
        
        void SetMenuScreen()
        {
            _hasSaveData = _saveSystem.Loaded;
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
            _mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
            _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
            _mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
            _mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
            // _mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;

        }
        
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
        
        public void OpenCreditsScreen()
        {
            _creditsPanel.gameObject.SetActive(true);
            _mainMenuPanel.gameObject.SetActive(false);
            _creditsPanel.OnCloseCredits += CloseCreditsScreen;
        }
        public void CloseCreditsScreen()
        {
            _creditsPanel.OnCloseCredits -= CloseCreditsScreen;
            _creditsPanel.gameObject.SetActive(false);
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
        }
        void ButtonStartNewGameClicked()
        {
            if (!_hasSaveData)
            {
                ConfirmStartNewGame();

            }
            else
            {
                ShowStartNewGameConfirmationPopup();

            }
        }
        void ConfirmStartNewGame()
        {
            _startNewGameEvent.RaiseEvent();
        }
        void ShowStartNewGameConfirmationPopup()
        {
            Debug.Log("Confirm new game");
            _startNewGameEvent.RaiseEvent(); //TODO: remove
            // _popupPanel.ConfirmationResponseAction += StartNewGamePopupResponse;
            // _popupPanel.ClosePopupAction += HidePopup;
            //
            // _popupPanel.gameObject.SetActive(true);
            // _popupPanel.SetPopup(PopupType.NewGame);

        }
        
        
        
        
    }
}