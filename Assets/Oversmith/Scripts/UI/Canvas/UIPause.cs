using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Oversmith.Scripts.UI.Canvas
{
    public class UIPause : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button backToMenuButton;

        [Header("Listening to")]
        [SerializeField] private BoolEventChannelSO onPauseOpened;

        public event UnityAction Resumed = default;
        public event UnityAction SettingsScreenOpened = default;
        public event UnityAction BackToMainRequested = default;


        private void OnEnable()
        {
            onPauseOpened.RaiseEvent(true);
            // _inputReader.MenuCloseEvent += Resume;
            resumeButton.onClick.AddListener(Resume);
            settingsButton.onClick.AddListener(OpenSettingsScreen);

        }

        private void Resume()
        {
            Resumed.Invoke();
        }
        private void OpenSettingsScreen()
        {
            SettingsScreenOpened.Invoke();
        }

        private void OnDisable()
        {
            onPauseOpened.RaiseEvent(false);
        }
    }
}