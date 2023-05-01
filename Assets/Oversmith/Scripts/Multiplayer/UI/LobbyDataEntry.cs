using Oversmith.Scripts.Multiplayer.Managers;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

namespace Oversmith.Scripts.Multiplayer.UI
{
    public class LobbyDataEntry : MonoBehaviour
    {
        //Data
        public CSteamID lobbyId;
        public string lobbyName;
        public TextMeshProUGUI lobbyNameText;
        
        public void SetLobbyData()
        {
            lobbyNameText.text = lobbyName != string.Empty ? lobbyName : "Empty";
        }
        
        public void JoinLobby()
        {
            Debug.Log("Join clicked");
            SteamLobby.Instance.JoinLobby(lobbyId);
        }
    }
}