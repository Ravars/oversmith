using System.Collections.Generic;
using System.Linq;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Multiplayer.Old.Managers;
using MadSmith.Scripts.Multiplayer.Old.UI;
using MadSmith.Scripts.Multiplayer.UI;
using MadSmith.Scripts.Utils;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class LobbyControllerCanvas : Singleton<LobbyControllerCanvas>
    {
        public UnityAction Closed;
        // public UnityAction NextPage;
        [SerializeField] private InputReader _inputReader;
        
        //UI Element
        public TextMeshProUGUI LobbyNameText;
        [SerializeField] private LocalizeStringEvent readyText;

        //Player Data
        public GameObject PlayerListViewContent;
        public GameObject PlayerListItemPrefab;
        // private MadSmithNetworkRoomPlayer _localMadSmithNetworkRoomPlayer;
        
        //Other Data
        public ulong CurrentLobbyID;
        public bool PlayerItemCreated = false;
        private List<PlayerListItem> PlayerListItems = new();

        //Ready
        public Button StartGameButton;
        public TextMeshProUGUI ReadyButtonText;
        [SerializeField] private VoidEventChannelSO onUpdatePlayerList = default;

        public GameObject canvasView;
        //Manager
        private MadSmithNetworkRoomManager _manager;
        public MadSmithNetworkRoomManager Manager
        { get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
        private MadSmithNetworkRoomPlayer _localMadSmithNetworkRoomPlayer;
        public MadSmithNetworkRoomPlayer LocalPlayer
        { get
            {
                if (!ReferenceEquals(_localMadSmithNetworkRoomPlayer, null)) return _localMadSmithNetworkRoomPlayer;
                return _localMadSmithNetworkRoomPlayer = UiRoomManager.Instance.LocalClient;
            }
        }

        public void Setup(MadSmithNetworkRoomPlayer localClient)
        {
            //Debug.Log("Setup");
            _localMadSmithNetworkRoomPlayer = localClient;
        }
        
        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseScreen;
            onUpdatePlayerList.OnEventRaised += UpdatePlayerList;
            canvasView.SetActive(true);
            
        }
        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
            onUpdatePlayerList.OnEventRaised -= UpdatePlayerList;
            // NullReference exception
            // if (!ReferenceEquals(canvasView, null))
            // {
            //     canvasView.SetActive(false);
            // }
        }
        
        public void UpdatePlayerList()
        {
            if (!PlayerItemCreated) { CreateHostPlayerItem(); } // Host
            if (PlayerListItems.Count < Manager.roomSlots.Count){ CreateClientPlayerItem();}
            if (PlayerListItems.Count > Manager.roomSlots.Count) { RemovePlayerItem();}
            if (PlayerListItems.Count == Manager.roomSlots.Count) { UpdatePlayerItem();}
        }
        
        
        private void CreateHostPlayerItem()
        {
            foreach (var networkRoomPlayer in Manager.roomSlots)
            {
                var player = (MadSmithNetworkRoomPlayer)networkRoomPlayer;
                GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, PlayerListViewContent.transform, true) as GameObject;
                PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();
                
                // newPlayerItemScript.PlayerName = player.PlayerName;
                newPlayerItemScript.ConnectionID = player.ConnectionID;
                // newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                newPlayerItemScript.Ready = player.readyToBegin;
                newPlayerItemScript.SetPlayerValues();

                newPlayerItem.transform.localScale = Vector3.one;
                PlayerListItems.Add(newPlayerItemScript);
            }
            
            PlayerItemCreated = true;
        }
        private void CreateClientPlayerItem()
        {
            foreach (var networkRoomPlayer in Manager.roomSlots)
            {
                var player = (MadSmithNetworkRoomPlayer)networkRoomPlayer;
                if (PlayerListItems.All(b => b.ConnectionID != player.ConnectionID))
                {
                    GameObject newPlayerItem = Instantiate(PlayerListItemPrefab, PlayerListViewContent.transform, true) as GameObject;
                    PlayerListItem newPlayerItemScript = newPlayerItem.GetComponent<PlayerListItem>();

                    // newPlayerItemScript.PlayerName = player.PlayerName;
                    newPlayerItemScript.ConnectionID = player.ConnectionID;
                    // newPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
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
                
                if (Manager.roomSlots.All(b => ((MadSmithNetworkRoomPlayer)b).ConnectionID != playerListItem.ConnectionID))
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
            foreach (var networkRoomPlayer in Manager.roomSlots)
            {
                var player = (MadSmithNetworkRoomPlayer)networkRoomPlayer;
                foreach (PlayerListItem playerListItemScript in PlayerListItems)
                {
                    if (playerListItemScript.ConnectionID == player.ConnectionID)
                    {
                        // playerListItemScript.PlayerName = player.PlayerName;
                        playerListItemScript.Ready = player.readyToBegin;
                        playerListItemScript.CharacterID = player.CharacterId;
                        playerListItemScript.SetPlayerValues();
                        if (player.ConnectionID == LocalPlayer.ConnectionID)
                        {
                            UpdateButton();
                        }
                    }
                }
            }   
            // CheckIfAllReady();
        }
        private void UpdateButton()
        {
            if (LocalPlayer.readyToBegin)
                readyText.StringReference.TableEntryReference = "Unready";
            else
                readyText.StringReference.TableEntryReference = "Ready";
        }
        
        // private void CheckIfAllReady()
        // {
        //     foreach (var networkRoomPlayer in Manager.roomSlots)
        //     {
        //         var player = (MadSmithNetworkRoomPlayer)networkRoomPlayer;
        //         foreach (PlayerListItem playerListItemScript in PlayerListItems)
        //         {
        //             if (playerListItemScript.ConnectionID == player.ConnectionID)
        //             {
        //                 if (player.ConnectionID == lobbyClient.ConnectionID && !lobbyClient.isLeader)
        //                 {
        //                     return;
        //                 }
        //             }
        //         }
        //     }
        //     bool allReady = false;
        //     
        //     foreach (var networkRoomPlayer in Manager.roomSlots)
        //     {
        //         var playerObjectController = (MadSmithNetworkRoomPlayer)networkRoomPlayer;
        //         if (playerObjectController.readyToBegin)
        //         {
        //             allReady = true;
        //         }
        //         else
        //         {
        //             allReady = false;
        //             break;
        //         }
        //     }
        //
        //     if (ReferenceEquals(lobbyClient, null)) return;
        //     
        //     if (allReady)
        //     {
        //         if (lobbyClient.ConnectionID == 0)
        //         {
        //             // StartGameButton.interactable = true;
        //         }
        //         else
        //         {
        //             // StartGameButton.interactable = false;
        //         }
        //     }
        //     else
        //     {
        //         // StartGameButton.interactable = false;
        //     }
        // }
        
        public void NextCharacter()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.NextCharacter();
            }
        }
        
        public void PreviousCharacter()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.PreviousCharacter();
            }
        }
        public void ReadyPlayerButton()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.ToggleReadyButton();
            }
        }
        public void NextPageButton()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.NextPageButton();
            }
        }
        
        // public void FinishCharacterSelectionButton()
        // {
        //     //Debug.Log("FinishCharacterSelectionButton" + ReferenceEquals(lobbyClient, null));
        //     if (!ReferenceEquals(lobbyClient, null))
        //     {
        //         lobbyClient.FinishCharacterSelection();
        //     }
        // }

        public void CloseScreen()
        {
            Closed?.Invoke();
        }
        
        // public void FindLocalPlayer()
        // {
        //     LocalPlayerObject = GameObject.Find("LocalGamePlayer");
        //     lobbyClient = LocalPlayerObject.GetComponent<LobbyClient>();
        // }



        public void UpdateLobbyName()
        {
            CurrentLobbyID = Manager.SteamLobby.currentLobbyID;
            LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
        }

        public void FinishCharacterSelectionPage()
        {
            //Debug.Log("FinishCharacterSelectionPage");
            foreach (var playerListItem in PlayerListItems)
            {
                Destroy(playerListItem.gameObject);
            }
            canvasView.SetActive(false);
            PlayerListItems.Clear();
            // NextPage?.Invoke();
        }

        // [ContextMenu("Update")]
        // public void UpdateLobby()
        // {
        //     UpdatePlayerList();
        // }
    }
}