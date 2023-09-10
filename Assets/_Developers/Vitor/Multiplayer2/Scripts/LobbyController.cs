using System.Collections.Generic;
using System.Linq;
using _Developers.Vitor.Multiplayer2.Scripts.UI;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Utils;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Developers.Vitor.Multiplayer2.Scripts
{
    public class LobbyController : Singleton<LobbyController>
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
        public LobbyClient lobbyClient;
        
        //Ready
        public Button StartGameButton;
        public TextMeshProUGUI ReadyButtonText;
            
        //Manager
        private MadSmithNetworkManager _manager;
        public MadSmithNetworkManager Manager
        {
            get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }
        
        public UnityAction Closed;
        [SerializeField] private InputReader _inputReader;
        
        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseScreen;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
        }
        
        
        public void UpdateLobbyName()
        {
            CurrentLobbyID = Manager.SteamLobby.currentLobbyID;
            LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
        }
        public void UpdatePlayerList()
        {
            if (!PlayerItemCreated) { CreateHostPlayerItem(); } // Host
            if (PlayerListItems.Count < Manager.lobbyPlayers.Count){ CreateClientPlayerItem();}
            if (PlayerListItems.Count > Manager.lobbyPlayers.Count) { RemovePlayerItem();}
            if (PlayerListItems.Count == Manager.lobbyPlayers.Count) { UpdatePlayerItem();}
        }
        public void FindLocalPlayer()
        {
            LocalPlayerObject = GameObject.Find("LocalGamePlayer");
            lobbyClient = LocalPlayerObject.GetComponent<LobbyClient>();
        }

        private void CreateHostPlayerItem()
        {
            foreach (LobbyClient player in Manager.lobbyPlayers)
            {
                GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, PlayerListViewContent.transform, true) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();
                
                newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionID = player.ConnectionID;
                newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.Ready = player.ready;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.localScale = Vector3.one;
                PlayerListItems.Add(newPlayerItemScript);
            }
            
            PlayerItemCreated = true;
        }

        private void CreateClientPlayerItem()
        {
            foreach (LobbyClient player in Manager.lobbyPlayers)
            {
                if (PlayerListItems.All(b => b.ConnectionID != player.ConnectionID))
                {
                    GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, PlayerListViewContent.transform, true) as GameObject;
                    PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                    newPlayerItemScript.PlayerName = player.PlayerName;
                    newPlayerItemScript.ConnectionID = player.ConnectionID;
                    newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                    newPlayerItemScript.Ready = player.ready;
                    newPlayerItemScript.SetPlayerValues();

                    newPlayerItem.transform.localScale = Vector3.one;
            
                    PlayerListItems.Add(newPlayerItemScript);
                }
            }
        }
        public void RemovePlayerItem()
        {
            List<PlayerListItem> playerListItemsToRemove = new();

            foreach (PlayerListItem playerListItem in PlayerListItems)
            {
                if (Manager.lobbyPlayers.All(b => b.ConnectionID != playerListItem.ConnectionID))
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
                        GameObject objectToRemove = playerListItemToRemove.gameObject;
                        PlayerListItems.Remove(playerListItemToRemove);
                        Destroy(objectToRemove);
                        objectToRemove = null;
                    }
                }
            }
        }
        public void UpdatePlayerItem()
        {
            foreach (LobbyClient player in Manager.lobbyPlayers)
            {
                foreach (PlayerListItem playerListItemScript in PlayerListItems)
                {
                    if (playerListItemScript.ConnectionID == player.ConnectionID)
                    {
                        playerListItemScript.PlayerName = player.PlayerName;
                        playerListItemScript.Ready = player.ready;
                        playerListItemScript.CharacterID = player.CharacterId;
                        Debug.Log("UpdatePlayerItem: " + player.ConnectionID);
                        playerListItemScript.SetPlayerValues();
                        if (player == lobbyClient)
                        {
                            UpdateButton();
                        }
                    }
                }
            }
            CheckIfAllReady();
        }

        private void UpdateButton()
        {
            ReadyButtonText.text = lobbyClient.ready? "Unready" : "Ready";
        }

        private void CheckIfAllReady()
        {
            bool allReady = false;
            
            foreach (LobbyClient playerObjectController in Manager.lobbyPlayers)
            {
                if (playerObjectController.ready)
                {
                    allReady = true;
                }
                else
                {
                    allReady = false;
                    break;
                }
            }
            if (allReady)
            {
                if (lobbyClient.ConnectionID == 0)
                {
                    StartGameButton.interactable = true;
                }
                else
                {
                    StartGameButton.interactable = false;
                }
            }
            else
            {
                StartGameButton.interactable = false;
            }
        }
        public void ReadyPlayer()
        {
            lobbyClient.ChangeReady();
        }
        public void NextCharacter()
        {
            lobbyClient.NextCharacter();   
        }

        public void PreviousCharacter()
        {
            lobbyClient.PreviousCharacter();
        }
        public void StartGame(string sceneName)
        {
            lobbyClient.CanStartGame(sceneName);
        }

        public void CloseScreen()
        {
            Closed?.Invoke();
        }
    }
}