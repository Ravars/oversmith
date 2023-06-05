using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.SceneManagement
{
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO managerScene;
        [SerializeField] private GameSceneSO menuScene;

        [Header("Broadcasting on")]
        [SerializeField] private AssetReference menuLoadChannel = default;
        private void Start()
        {
            managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
        }

        private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            obj.Result.RaiseEvent(menuScene, true);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}