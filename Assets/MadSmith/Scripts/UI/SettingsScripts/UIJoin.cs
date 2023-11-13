using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UIJoin : MonoBehaviour
    {
        public UnityAction Closed;
        public UnityAction SteamJoinButtonAction;
        public UnityAction LocalhostJoinButtonAction;
        [SerializeField] private InputReader _inputReader;
        public void SetJoinHost()
        {
            Debug.Log("Set join host");
        }
        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseScreen;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
        }
        public void CloseScreen()
        {
            Closed?.Invoke();
        }
        public void JoinSteam()
        {
            SteamJoinButtonAction?.Invoke();
        }
        public void JoinLocalhost()
        {
            Debug.LogWarning("Funcao desabilitada");
            LocalhostJoinButtonAction?.Invoke();
        }
    }
}