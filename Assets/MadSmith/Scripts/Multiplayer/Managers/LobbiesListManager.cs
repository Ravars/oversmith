using System.Collections.Generic;
using MadSmith.Scripts.Multiplayer.UI;
using MadSmith.Scripts.Utils;
using UnityEngine;
using Steamworks;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class LobbiesListManager : PersistentSingleton<LobbiesListManager>
    {
        public GameObject lobbiesMenu;
        public GameObject lobbyDataItemPrefab;
        public GameObject lobbyListContent;
        
        public GameObject lobbiesButton, hostButton;
        public List<GameObject> listOfLobbies = new();

        public void DestroyLobbies()
        {
            foreach (GameObject lobbyItem in listOfLobbies)
            {
                Destroy(lobbyItem);
            }
            listOfLobbies.Clear();
        }

        public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
        {
            for (int i = 0; i < lobbyIDs.Count; i++)
            {
                if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
                {
                    // GameObject createdItem = Instantiate(lobbyDataItemPrefab);
                    // LobbyDataEntry lobbyDataEntry = createdItem.GetComponent<LobbyDataEntry>();
                    // lobbyDataEntry.lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                    // lobbyDataEntry.SetLobbyData();
                    GameObject createdItem = Instantiate(lobbyDataItemPrefab, lobbyListContent.transform, true);
                    LobbyDataEntry lobbyDataEntry = createdItem.GetComponent<LobbyDataEntry>();
                    lobbyDataEntry.lobbyName = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                    lobbyDataEntry.lobbyId = (CSteamID)lobbyIDs[i].m_SteamID;
                    lobbyDataEntry.SetLobbyData();
                    createdItem.transform.localScale = Vector3.one;
                    listOfLobbies.Add(createdItem);
                }
            }
        }
        public void GetListOfLobbies()
        {
            lobbiesButton.SetActive(false);
            hostButton.SetActive(false);
            lobbiesMenu.SetActive(true);
        
            SteamLobby.Instance.GetLobbiesList();
        }
        public void CloseListOfLobbies()
        {
            lobbiesButton.SetActive(true);
            hostButton.SetActive(true);
            lobbiesMenu.SetActive(false);
            DestroyLobbies();
        }
        
    }
}