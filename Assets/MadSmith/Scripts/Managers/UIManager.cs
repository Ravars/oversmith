using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Menu;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Systems.Settings;
using MadSmith.Scripts.UI;
using MadSmith.Scripts.UI.Canvas;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Gameplay")] 
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private GameStateSO _gameStateManager = default;
        [SerializeField] private GameSceneSO _mainMenu = default; // MenuSO

        [Header("Scene UI")] 
        [SerializeField] private MenuSelectionHandler _selectionHandler = default;
        [SerializeField] private UIPause pauseScreen;
        [SerializeField] private UISettingsController _settingScreen = default;
        [SerializeField] private UIPopup _popupPanel = default;
        [SerializeField] private UIEndGame _endGameComponent = default;
        [SerializeField] private HudController inGameComponent = default;
        [SerializeField] private UIInGameTutorial inGameTutorialComponent = default;

        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;
        [SerializeField] private FloatEventChannelSO _onLevelCompleted = default;

        [Header("Broadcasting on ")]
        [SerializeField] private LoadEventChannelSO _loadMenuEvent = default;
        [SerializeField] private LoadEventChannelSO _loadNextLevel = default;
        [SerializeField] private VoidEventChannelSO _onGameStart = default;
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        { get {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        private void Awake()
        {
            CloseAll();
            
        }

        private void OnEnable()
        {
            Debug.Log("UI Manager OnEnable");
            // _onSceneReady.OnEventRaised += StartUp;
            _onLevelCompleted.OnEventRaised += OpenEndGameScreen;
            _inputReader.MenuPauseEvent += OpenUIPause;
            StartUp();
            // MadSmithNetworkManager.OnServerReadied += MadSmithNetworkManagerOnOnServerReadied;
        }

        // private void MadSmithNetworkManagerOnOnServerReadied(NetworkConnection obj)
        // {
        //     StartUp();
        // }

        private void OpenEndGameScreen(float finalScore)
        {
            StartUp();
            inGameComponent.gameObject.SetActive(false);
            _endGameComponent.Setup((int)finalScore);
            _endGameComponent.Continued += EndGameComponentOnContinued;
            _endGameComponent.BackToMenuClicked += ShowBackToMenuConfirmationPopup;
            _endGameComponent.gameObject.SetActive(true);
        }
        

        private void EndGameComponentOnContinued()
        {
            _endGameComponent.Continued -= EndGameComponentOnContinued;
            _endGameComponent.BackToMenuClicked -= ShowBackToMenuConfirmationPopup;
            if (GameManager.Instance.CurrentSceneSo.sceneType == GameSceneType.Location && GameManager.Instance.CurrentSceneSo.nextScene != null)
            {
                _endGameComponent.gameObject.SetActive(false);
                _loadNextLevel.RaiseEvent(GameManager.Instance.CurrentSceneSo.nextScene,true);
            }
            // _onScreenEndGameClosed.RaiseEvent();
        }

        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= StartUp;
            _onLevelCompleted.OnEventRaised -= OpenEndGameScreen;
            _inputReader.MenuPauseEvent -= OpenUIPause;
            // MadSmithNetworkManager.OnServerReadied -= MadSmithNetworkManagerOnOnServerReadied;
        }

        private void CloseAll()
        {
            _inputReader.EnableDialogueInput();
            inGameComponent.gameObject.SetActive(false);
            inGameTutorialComponent.gameObject.SetActive(false);
            _endGameComponent.gameObject.SetActive(false);
            _popupPanel.gameObject.SetActive(false);
            pauseScreen.gameObject.SetActive(false);
            _settingScreen.gameObject.SetActive(false);
        }
        private void StartUp()
        {
            if (GameManager.Instance.CurrentSceneSo.sceneType == GameSceneType.Location)
            {
                OpenTutorial();
            }
        }


        [ContextMenu("Open Config")]
        private void OpenUIPause()
        {
            if (pauseScreen == null) return; 
            _inputReader.MenuPauseEvent -= OpenUIPause; // you can open UI pause menu again, if it's closed
            
            pauseScreen.SettingsScreenOpened += OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            pauseScreen.BackToMainRequested += ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            pauseScreen.Resumed += CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            inGameComponent.gameObject.SetActive(false);
            pauseScreen.gameObject.SetActive(true);

            _inputReader.EnableMenuInput();
            _gameStateManager.UpdateGameState(GameState.Pause);
        }
        void CloseUIPause()
        {
            // Time.timeScale = 1; // unpause time

            _inputReader.MenuPauseEvent += OpenUIPause; // you can open UI pause menu again, if it's closed

            // once the popup is closed, you can't listen to the following events 
            pauseScreen.SettingsScreenOpened -= OpenSettingScreen;//once the UI Pause popup is open, listen to open Settings 
            pauseScreen.BackToMainRequested -= ShowBackToMenuConfirmationPopup;//once the UI Pause popup is open, listen to back to menu button
            pauseScreen.Resumed -= CloseUIPause;//once the UI Pause popup is open, listen to unpause event

            pauseScreen.gameObject.SetActive(false);
            inGameComponent.gameObject.SetActive(true);

            _gameStateManager.ResetToPreviousGameState();
		
            if (_gameStateManager.CurrentGameState == GameState.Gameplay)
            {
                _inputReader.EnableGameplayInput();
            }
            _selectionHandler.Unselect();
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

            _popupPanel.ClosePopupAction += HideBackToMenuConfirmationPopup;
            //
            _popupPanel.ConfirmationResponseAction += BackToMainMenu;

            _inputReader.EnableMenuInput();
            _popupPanel.gameObject.SetActive(true);
            _popupPanel.SetPopup(PopupType.BackToMenu);
        }
        void HideBackToMenuConfirmationPopup()
        {
            _popupPanel.ClosePopupAction -= HideBackToMenuConfirmationPopup;
            _popupPanel.ConfirmationResponseAction -= BackToMainMenu;

            _popupPanel.gameObject.SetActive(false);
            _selectionHandler.Unselect();
            pauseScreen.gameObject.SetActive(true); // Set pause screen to inactive

            // time is still set to 0 and Input is still set to menuInput 
            //going out from confirmaiton popup screen gets us back to the pause screen
        }
        void BackToMainMenu(bool confirm)
        {
            HideBackToMenuConfirmationPopup();// hide confirmation screen, show close UI pause, 

            if (confirm)
            {
                CloseUIPause();//close ui pause to unsub from all events 
                _loadMenuEvent.RaiseEvent(_mainMenu, false); //load main menu
            }
        }

        private void OpenTutorial()
        {
            inGameTutorialComponent.gameObject.SetActive(true);
            inGameTutorialComponent.Closed += UiTutorialClosed;
            inGameTutorialComponent.Setup((LocationSO)GameManager.Instance.CurrentSceneSo);
        }

        private void UiTutorialClosed()
        {
            inGameTutorialComponent.gameObject.SetActive(false);
            inGameTutorialComponent.Closed -= UiTutorialClosed;
            _inputReader.EnableGameplayInput();
            inGameComponent.gameObject.SetActive(true);
            _onGameStart.RaiseEvent();
        }
    }
}