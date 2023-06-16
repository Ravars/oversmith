using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Oversmith.Scripts.UI.Canvas
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        
        public UnityAction NewGameButtonAction;
        public UnityAction ContinueButtonAction;
        public UnityAction SettingsButtonAction;
        public UnityAction CreditsButtonAction;
        public UnityAction ExitButtonAction;

        public void SetMenuScreen(bool hasSaveData)
        {
            gameObject.SetActive(true);
            continueButton.interactable = hasSaveData;
            if (hasSaveData)
            {
                continueButton.Select();
            }
            else
            {
                newGameButton.Select();
            }
        }
        
        public void NewGameButton()
        {
            NewGameButtonAction.Invoke();
        }

        public void ContinueButton()
        {
            ContinueButtonAction.Invoke();
        }

        public void SettingsButton()
        {
            SettingsButtonAction.Invoke();
        }

        public void CreditsButton()
        {
            CreditsButtonAction.Invoke();
        }

        public void ExitButton()
        {
            ExitButtonAction.Invoke();
        }
    }
}