using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.SceneManagement
{
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO managerScene;
        [SerializeField] private GameSceneSO menuScene;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO menuLoadChannel = default;
        private void Start()
        {
            managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            menuLoadChannel.RaiseEvent(menuScene, true);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}