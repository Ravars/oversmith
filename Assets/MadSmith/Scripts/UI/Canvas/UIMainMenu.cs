using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.Canvas
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        
        public UnityAction HostGameButtonAction;
        public UnityAction JoinLobbyButtonAction;
        public UnityAction SettingsButtonAction;
        public UnityAction CreditsButtonAction;
        public UnityAction TutorialButtonAction;
        public UnityAction ExitButtonAction;

        public void SetMenuScreen(bool hasSaveData)
        {
            joinButton.Select();
        }
        
        public void HostGameButton()
        {
            HostGameButtonAction?.Invoke();
        }

        public void JoinButton()
        {
            JoinLobbyButtonAction?.Invoke();
        }

        public void SettingsButton()
        {
            SettingsButtonAction?.Invoke();
        }

        public void CreditsButton()
        {
            CreditsButtonAction?.Invoke();
        }

        public void TutorialButton()
        {
            TutorialButtonAction?.Invoke();
        }

        public void ExitButton()
        {
            ExitButtonAction?.Invoke();
        }
    }
}