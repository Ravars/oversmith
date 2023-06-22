using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using UnityEngine;

namespace MadSmith.Scripts.UI
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