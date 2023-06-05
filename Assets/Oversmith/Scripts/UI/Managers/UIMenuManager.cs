using System;
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
        private void Start()
        {
            SetMenuScreen();
        }
        
        void SetMenuScreen()
        {
            _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            _mainMenuPanel.SetMenuScreen(_hasSaveData);
            // _mainMenuPanel.ContinueButtonAction += _continueGameEvent.RaiseEvent;
            // _mainMenuPanel.NewGameButtonAction += ButtonStartNewGameClicked;
            _mainMenuPanel.SettingsButtonAction += OpenSettingsScreen;
            _mainMenuPanel.CreditsButtonAction += OpenCreditsScreen;
            // _mainMenuPanel.ExitButtonAction += ShowExitConfirmationPopup;

        }
        
        public void OpenSettingsScreen()
        {
            _settingsPanel.gameObject.SetActive(true);
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
            _creditsPanel.OnCloseCredits += CloseCreditsScreen;
        }
        public void CloseCreditsScreen()
        {
            _creditsPanel.OnCloseCredits -= CloseCreditsScreen;
            _creditsPanel.gameObject.SetActive(false);
            _mainMenuPanel.SetMenuScreen(_hasSaveData);

        }
    }
}