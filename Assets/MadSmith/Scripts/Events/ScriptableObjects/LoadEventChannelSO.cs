using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Load Event Channel")]
    public class LoadEventChannelSO : DescriptionBaseSO
    {
        public UnityAction<GameSceneSO, bool, bool> OnLoadingRequested;
        // ReSharper disable Unity.PerformanceAnalysis
        public void RaiseEvent(GameSceneSO locationToLoad, bool showLoadingScreen = false, bool fadeScreen = false)
        {
            Debug.Log("locationToLoad" + locationToLoad.sceneReference);
            if (OnLoadingRequested != null)
            {
                OnLoadingRequested.Invoke(locationToLoad, showLoadingScreen, fadeScreen);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("A Scene loading was requested, but nobody picked it up. " +
                                 "Check why there is no SceneLoader already present, " +
                                 "and make sure it's listening on this Load Event channel.");
#endif
            }
        }
    }
}