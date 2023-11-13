
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
        [SerializeField] private LoadEventChannelSO _menuLoadChannel = default;
        private void Start()
        {
            SceneManager.LoadSceneAsync(managerScene.sceneId, LoadSceneMode.Additive).completed += LoadMainMenu;
            // managerScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadMainMenu;
        }

        private void LoadMainMenu(AsyncOperation obj)
        {
            _menuLoadChannel.RaiseEvent(menuScene, true);
            SceneManager.UnloadSceneAsync(0);
        }

        // private void LoadMainMenu(AsyncOperationHandle<SceneInstance> obj)
        // {
        //     _menuLoadChannel.RaiseEvent(menuScene, true);
        //     SceneManager.UnloadSceneAsync(0);
        // }

        // private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        // {
        //     LoadMainMenu
        // }

        // private void LoadMainMenu()
        // {
        //     obj.Result.RaiseEvent(menuScene, true);
        //
        //     SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
        // }
    }
}