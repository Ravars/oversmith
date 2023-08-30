using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Developers.Vitor.Multiplayer_1.Scripts.UI
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private HelloWorldNetworkManager networkManager;

        [Header("UI")] [SerializeField] private GameObject landingPagePanel;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private Button joinButton;

        private void OnEnable()
        {
            HelloWorldNetworkManager.OnClientConnected += HandleClientConnected;
            HelloWorldNetworkManager.OnClientDisconnected += HandleClientDisconnected;
        }
        private void OnDisable()
        {
            HelloWorldNetworkManager.OnClientConnected -= HandleClientConnected;
            HelloWorldNetworkManager.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = ipAddressInputField.text;
            networkManager.networkAddress = ipAddress;
            networkManager.StartClient();
            joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            joinButton.interactable = true;
            gameObject.SetActive(false);
            landingPagePanel.SetActive(false);
        }
        private void HandleClientDisconnected()
        {
            joinButton.interactable = true;
        }
    }
}