using Oversmith.Scripts.Systems.Settings;
using Oversmith.Scripts.UI.Canvas;
using UnityEngine;

namespace Oversmith.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Scene UI")] 
        [SerializeField] private UIPause pauseScreen;
        [SerializeField] private UISettingsController _settingScreen = default;
        
        
        [ContextMenu("Open Config")]
        private void OpenUIPause()
        {
            // _inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed


            pauseScreen.SettingsScreenOpened += OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            pauseScreen.Resumed += CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            pauseScreen.gameObject.SetActive(true);

            // _inputReader.EnableMenuInput();
            // _gameStateManager.UpdateGameState(GameState.Pause);
        }
        
        void OpenSettingScreen()
        {
            _settingScreen.Closed += CloseSettingScreen; // sub to close setting event with event 

            pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

            _settingScreen.gameObject.SetActive(true);// set Setting screen to active 

            // time is still set to 0 and Input is still set to menuInput 
        }
        void CloseSettingScreen()
        {
            //unsub from close setting events 
            _settingScreen.Closed -= CloseSettingScreen;

            // _selectionHandler.Unselect();
            pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

            _settingScreen.gameObject.SetActive(false);

            // time is still set to 0 and Input is still set to menuInput 
            //going out from setting screen gets us back to the pause screen
        }
        void ShowBackToMenuConfirmationPopup()
        {
            pauseScreen.gameObject.SetActive(false); // Set pause screen to inactive

            // _popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;
            //
            // _popupPanel.ConfirmationResponseAction += BackToMainMenu;

            // _inputReader.EnableMenuInput();
            pauseScreen.gameObject.SetActive(true);
            // pauseScreen.SetPopup(PopupType.BackToMenu);
        }
        void CloseUIPause()
        {
            // Time.timeScale = 1; // unpause time

            // _inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

            // once the popup is closed, you can't listen to the following events 
            pauseScreen.SettingsScreenOpened -= OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            pauseScreen.Resumed -= CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            pauseScreen.gameObject.SetActive(false);

            // _gameStateManager.ResetToPreviousGameState();
		
            // if (_gameStateManager.CurrentGameState == GameState.Gameplay
            //     || _gameStateManager.CurrentGameState == GameState.Combat)
            // {
            //     _inputReader.EnableGameplayInput();
            // }
            //
            // _selectionHandler.Unselect();
        }
    }
}