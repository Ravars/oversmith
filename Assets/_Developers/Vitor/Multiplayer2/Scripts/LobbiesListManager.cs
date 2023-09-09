using System.Collections.Generic;
using MadSmith.Scripts.Utils;
using UnityEngine;
using Steamworks;

namespace _Developers.Vitor.Multiplayer2.Scripts
{
    public class LobbiesListManager : Singleton<LobbiesListManager>
    {
        public GameObject lobbiesMenu;
        public GameObject lobbyDataItemPrefab;
        public GameObject lobbyListContent;
        
        public GameObject lobbiesButton, hostButton;

        public List<GameObject> listOfLobbies = new();

        public void DestroyLobbies()
        {
            foreach (var lobbyItem in listOfLobbies)
            {
                Destroy(lobbyItem);
            }
            listOfLobbies.Clear();
            lobbiesMenu.SetActive(false);
        }

        public void DisplayLobbies(List<CSteamID> lobbyIDs, LobbyDataUpdate_t result)
        {
            for (int i = 0; i < lobbyIDs.Count; i++)
            {
                if (lobbyIDs[i].m_SteamID == result.m_ulSteamIDLobby)
                {
                    GameObject createdItem = Instantiate(lobbyDataItemPrefab, lobbyListContent.transform, true);
                    createdItem.GetComponent<LobbyDataEntry>().lobbyId = (CSteamID)lobbyIDs[i].m_SteamID;
                    createdItem.GetComponent<LobbyDataEntry>().lobbyName =
                        SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "name");
                    createdItem.GetComponent<LobbyDataEntry>().SetLobbyData();
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
    }
}