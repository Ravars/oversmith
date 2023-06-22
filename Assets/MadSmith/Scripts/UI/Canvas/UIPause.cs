using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.Canvas
{
    public class UIPause : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private UIGenericButton resumeButton;
        [SerializeField] private UIGenericButton settingsButton;
        [SerializeField] private UIGenericButton backToMenuButton;

        [Header("Listening to")]
        [SerializeField] private BoolEventChannelSO onPauseOpened;

        public event UnityAction Resumed = default;
        public event UnityAction SettingsScreenOpened = default;
        public event UnityAction BackToMainRequested = default;


        private void OnEnable()
        {
            onPauseOpened.RaiseEvent(true);
            
            resumeButton.SetButton(true);
            _inputReader.MenuCloseEvent += Resume;
            resumeButton.Clicked += Resume;
            settingsButton.Clicked += OpenSettingsScreen;
            backToMenuButton.Clicked += BackToMainMenuConfirmation;
        }
        private void OnDisable()
        {
            onPauseOpened.RaiseEvent(false);
            _inputReader.MenuCloseEvent -= Resume;
        }
        
        private void Resume()
        {
            Resumed?.Invoke();
        }
        private void OpenSettingsScreen()
        {
            SettingsScreenOpened?.Invoke();
        }

        private void BackToMainMenuConfirmation()
        {
            BackToMainRequested?.Invoke();
        }

        public void CloseScreen()
        {
            Resumed.Invoke();
        }


    }
}