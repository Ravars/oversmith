using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UIHost : MonoBehaviour
    {
        // [SerializeField] private Button steamHost;
        // [SerializeField] private Button localHost;
        
        public UnityAction SteamHostButtonAction;
        public UnityAction LocalHostButtonAction;
        public UnityAction Closed;
        [SerializeField] private InputReader _inputReader;
        
        public void SetUIHost(bool steamOpen)
        {
            //Debug.Log("Steam open: " + steamOpen);
            // steamHost.interactable = steamOpen;
        }

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseScreen;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
        }

        public void SteamHostButton()
        {
            SteamHostButtonAction?.Invoke();
        }
        public void LocalHostButton()
        {
            LocalHostButtonAction?.Invoke();
        }
        public void CloseScreen()
        {
            Closed.Invoke();
        }
    }
}