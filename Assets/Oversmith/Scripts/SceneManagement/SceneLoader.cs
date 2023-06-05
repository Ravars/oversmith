using System;
using System.Collections;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO gameplayScene = default;

        [Header("Listening To")] 
        [SerializeField] private LoadEventChannelSO loadMenu;

        [Header("Broadcasting on")] 
        [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
        [SerializeField] private VoidEventChannelSO _onSceneReady = default; //picked up by the SpawnSystem
        
        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;
        
        //Parameters coming from scene loading requests
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;
        
        private SceneInstance _gameplayManagerSceneInstance = new SceneInstance();
        private float _fadeDuration = .5f;
        private bool _isLoading;

        private void OnEnable()
        {
            loadMenu.OnLoadingRequested += LoadMenu;
        }

        private void OnDisable()
        {
            loadMenu.OnLoadingRequested -= LoadMenu;
        }

        private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            if (_isLoading) return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            if (_gameplayManagerSceneInstance.Scene != null && _gameplayManagerSceneInstance.Scene.isLoaded)
            {
                Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);
            }
            StartCoroutine(UnloadPreviousScene());
        }

        private IEnumerator UnloadPreviousScene()
        {
            // _inputReader.DisableAllInput();
		    // _fadeRequestChannel.FadeOut(_fadeDuration);
            yield return new WaitForSeconds(_fadeDuration);
            if (_currentlyLoadedScene != null)
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    _currentlyLoadedScene.sceneReference.UnLoadScene();
                }
            }
            LoadNewScene();
        }
        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        private void LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.RaiseEvent(true);
            }

            _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _loadingOperationHandle.Completed += OnNewSceneLoaded;
        }
        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            //Save loaded scenes (to be unloaded at next load request)
            _currentlyLoadedScene = _sceneToLoad;

            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);
            LightProbes.TetrahedralizeAsync();

            _isLoading = false;

            if (_showLoadingScreen)
                _toggleLoadingScreen.RaiseEvent(false);

            // _fadeRequestChannel.FadeIn(_fadeDuration);

            StartGameplay();
        }
        private void StartGameplay()
        {
            _onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef in a gameplay scene
        }

    }
}