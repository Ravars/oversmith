using System;
using System.Collections;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.SceneManagement
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        [SerializeField] private GameSceneSO gameplayScene = default;
        [SerializeField] private InputReader _inputReader;
        [Header("Listening To")] 
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        [SerializeField] private LoadEventChannelSO loadMenu;

        [Header("Broadcasting on")] 
        [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
        [SerializeField] private VoidEventChannelSO _onSceneReady = default; //picked up by the SpawnSystem
        
        // private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        // private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;
        
        //Parameters coming from scene loading requests
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;
        
        // private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
        private float _fadeDuration = 1.5f;
        private bool _isLoading;
        private int _currentlyLoadedSceneIndex;

        private void OnEnable()
        {
            // Debug.Log("OnEnable scene loader");
            loadLocation.OnLoadingRequested += LoadLocation;
            loadMenu.OnLoadingRequested += LoadMenu;
        }
        
        private void OnDisable()
        {
            loadLocation.OnLoadingRequested -= LoadLocation;
            loadMenu.OnLoadingRequested -= LoadMenu;
        }

        private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            // Debug.Log("Load Menu");
            if (_isLoading) return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            // if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
            // {
            //     Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
            // }
            StartCoroutine(UnloadPreviousScene());
        }

        private IEnumerator UnloadPreviousScene()
        {
            // Debug.Log("UnloadPreviousScene");
            _inputReader.DisableAllInput();
		    // _fadeRequestChannel.FadeOut(_fadeDuration);

            #region Test
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(true);
            }
            #endregion
            
            yield return new WaitForSeconds(_fadeDuration);

            var a = SceneManager.GetSceneByBuildIndex(_currentlyLoadedSceneIndex);
            
            // if (_currentlyLoadedScene != null)
            // {
            //     if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
            //     {
            //         _currentlyLoadedScene.sceneReference.UnLoadScene();
            //     }
            // }
            if (a.isLoaded)
            {
                SceneManager.UnloadSceneAsync(a);
            }
            LoadNewScene();
        }
        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        private void LoadNewScene()
        {
            // if (_showLoadingScreen)
            // {
            //     _toggleLoadingScreen.RaiseEvent(true);
            // }
            Debug.Log("LoadNewScene");
            SceneManager.LoadSceneAsync(_sceneToLoad.sceneId, LoadSceneMode.Additive).completed += OnCompleted;
            _currentlyLoadedSceneIndex = _sceneToLoad.sceneId;
            _currentlyLoadedScene = _sceneToLoad;
            // _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            // _loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        private void OnCompleted(AsyncOperation obj)
        {
            LightProbes.TetrahedralizeAsync();
            _isLoading = false;

            if (_showLoadingScreen)
                _toggleLoadingScreen.RaiseEvent(false);
            
            StartGameplay();
        }

        // private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        // {
        //     //Save loaded scenes (to be unloaded at next load request)
        //     _currentlyLoadedScene = _sceneToLoad;
        //
        //     Scene s = obj.Result.Scene;
        //     SceneManager.SetActiveScene(s);
        //     LightProbes.TetrahedralizeAsync();
        //
        //     _isLoading = false;
        //
        //     if (_showLoadingScreen)
        //         _toggleLoadingScreen.RaiseEvent(false);
        //
        //     // _fadeRequestChannel.FadeIn(_fadeDuration);
        //
        //     StartGameplay();
        // }
        private void StartGameplay()
        {
            _onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef in a gameplay scene
        }
        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            // Debug.Log("Load Location");
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            var gameplaySceneIndex = SceneManager.GetSceneByBuildIndex(gameplayScene.sceneId);
            if (!gameplaySceneIndex.isLoaded)
            {
                SceneManager.LoadSceneAsync(gameplayScene.sceneId, LoadSceneMode.Additive).completed += OnCompletedGameplayLoad;
                // _gameplayManagerLoadingOpHandle = gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                // _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        private void OnCompletedGameplayLoad(AsyncOperation obj)
        {
            // Debug.Log("OnCompletedGameplayLoad");
            StartCoroutine(UnloadPreviousScene());
        }

        // private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
        // {
        //     _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;
        //
        //     StartCoroutine(UnloadPreviousScene());
        // }
        
        public GameSceneSO GetCurrentSceneLoaded()
        {
            return _currentlyLoadedScene;
        }

    }
}