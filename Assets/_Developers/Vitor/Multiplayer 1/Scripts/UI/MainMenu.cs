using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private HelloWorldNetworkManager networkManager;

        [Header("UI")] 
        [SerializeField] private GameObject landingPagePanel;

        public void HostLobby()
        {
            networkManager.StartHost();
            landingPagePanel.SetActive(false);
        }
    }
}