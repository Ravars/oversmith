using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class LoadingInterfaceController : MonoBehaviour
    {
        [SerializeField] private GameObject loadingInterface;

        [Header("Listening to")]
        [SerializeField] private BoolEventChannelSO toggleLoadingScreen;

        private void OnEnable()
        {
            toggleLoadingScreen.OnEventRaised += ToggleLoadingScreen;
        }

        private void OnDisable()
        {
            toggleLoadingScreen.OnEventRaised -= ToggleLoadingScreen;
        }

        private void ToggleLoadingScreen(bool state)
        {
            loadingInterface.SetActive(state);
        }
    }
}