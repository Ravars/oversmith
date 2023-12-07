using System;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace MadSmith.Scripts.Systems.Settings
{
    public class UIInGameTutorial : MonoBehaviour
    {
        public UnityAction Closed = default;
        [SerializeField] private InputReader _inputReader = default;
        [SerializeField] private Image tutorialImage;
        [SerializeField] private LocalizeStringEvent tutorialTitle;
        [SerializeField] private LocalizeStringEvent tutorialSecondaryText;
        private void OnEnable()
        {
            _inputReader.EnableDialogueInput();
            _inputReader.OnAdvanceDialogueEvent += CloseTutorialButton;
            _inputReader.MenuCloseEvent += CloseTutorialButton;
        }

        private void OnDisable()
        {
            _inputReader.OnAdvanceDialogueEvent -= CloseTutorialButton;
            _inputReader.MenuCloseEvent -= CloseTutorialButton;
        }


        public void Setup(LocationSO level)
        {
            //Debug.Log("UI Setup." + level.tutorialDataSo == null + " " + ReferenceEquals(level.tutorialDataSo, null));
            //TODO verificacao com null nao funfa
            if (level.tutorialDataSo == null)
            {
                CloseTutorialButton();
                return;
            }
            
            tutorialImage.sprite = level.tutorialDataSo.tutorialImage;
            tutorialTitle.StringReference = level.tutorialDataSo.locationTitle;
            if (level.tutorialDataSo.locationSecondaryText != null)
            {
                tutorialSecondaryText.StringReference = level.tutorialDataSo.locationSecondaryText;
            }
        }

        public void CloseTutorialButton()
        {
            //Debug.Log("Closed");
            Closed?.Invoke();
        }
    }
}