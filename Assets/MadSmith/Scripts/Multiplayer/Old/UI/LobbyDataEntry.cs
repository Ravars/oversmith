using MadSmith.Scripts.Multiplayer.Old.Managers;
using Steamworks;
using TMPro;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Old.UI
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