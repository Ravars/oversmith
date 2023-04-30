using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using System.Linq;
using Oversmith.Scripts.Multiplayer.Managers;
using Oversmith.Scripts.Multiplayer.Player;
using Oversmith.Scripts.Multiplayer.UI;
using Oversmith.Scripts.Utils;
using TMPro;


namespace Oversmith.Scripts.Multiplayer
{
    public class SteamLobbyController : Singleton<SteamLobbyController>
    {
        //UI Element
        public TextMeshProUGUI LobbyNameText;
        
        //Player Data
        public GameObject PlayerListViewContent;
        public GameObject PlayerListItemPrefab;
        public GameObject LocalPlayerObject;
        
        //Other Data
        public ulong CurrentLobbyID;
        public bool PlayerItemCreated = false;
        private List<PlayerListItem> PlayerListItems = new();
        public PlayerObjectController LocalplayerController;
        
        //Manager
        private CustomNetworkManager _manager;

        private CustomNetworkManager Manager
        {
            get
            {
                if (_manager != null)
                {
                    return _manager;
                }
                return _manager = CustomNetworkManager.singleton as CustomNetworkManager;
            }
        }
        
        public void UpdateLobbyName()
        {
            CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
            LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
        }
        
        public void UpdatePlayerList()
        {
            if (!PlayerItemCreated) { CreateHostPlayerItem(); } // Host
            if (PlayerListItems.Count < Manager.GamePlayers.Count){ CreateClientPlayerItem();}
            if (PlayerListItems.Count > Manager.GamePlayers.Count) { RemovePlayerItem();}
            if (PlayerListItems.Count == Manager.GamePlayers.Count) { UpdatePlayerItem();}
        }
        public void FindLocalPlayer()
        {
            LocalPlayerObject = GameObject.Find("LocalGamePlayer");
            LocalplayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
        }

        public void CreateHostPlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();
                
                NewPlayerItemScript.PlayerName = player.PlayerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.SetPlayerValues();
                
                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;
                PlayerListItems.Add(NewPlayerItemScript);
            }
            
            PlayerItemCreated = true;
        }

        public void CreateClientPlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                if (!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID))
                {
                    GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab) as GameObject;
                    PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

                    NewPlayerItemScript.PlayerName = player.PlayerName;
                    NewPlayerItemScript.ConnectionID = player.ConnectionID;
                    NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                    // NewPlayerItemScript.Ready = player.Ready;
                    NewPlayerItemScript.SetPlayerValues();
            
                    NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                    NewPlayerItem.transform.localScale = Vector3.one;
            
                    PlayerListItems.Add(NewPlayerItemScript);
                }
            }
        }

        public void UpdatePlayerItem()
        {
            foreach (PlayerObjectController player in Manager.GamePlayers)
            {
                foreach (PlayerListItem playerListItemScript in PlayerListItems)
                {
                    if (playerListItemScript.ConnectionID == player.ConnectionID)
                    {
                        playerListItemScript.PlayerName = player.PlayerName;
                        playerListItemScript.SetPlayerValues();
                    }
                }
            }
        }

        public void RemovePlayerItem()
        {
            List<PlayerListItem> playerListItemsToRemove = new();

            foreach (PlayerListItem playerListItem in PlayerListItems)
            {
                if (Manager.GamePlayers.All(b => b.ConnectionID != playerListItem.ConnectionID))
                {
                    playerListItemsToRemove.Add(playerListItem);
                }
            }
            if (playerListItemsToRemove.Count > 0)
            {
                foreach (PlayerListItem playerListItemToRemove in playerListItemsToRemove)
                {
                    if (playerListItemToRemove != null)
                    {
                        GameObject ObjectToRemove = playerListItemToRemove.gameObject;
                        PlayerListItems.Remove(playerListItemToRemove);
                        Destroy(ObjectToRemove);
                        ObjectToRemove = null;
                    }
                }
            }
            
        }
    }
}