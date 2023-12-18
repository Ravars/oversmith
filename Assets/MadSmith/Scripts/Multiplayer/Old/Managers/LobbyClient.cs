// using MadSmith.Scripts.Events.ScriptableObjects;
// using MadSmith.Scripts.Managers;
// using MadSmith.Scripts.Multiplayer.Managers;
// using MadSmith.Scripts.Multiplayer.Old.UI;
// using MadSmith.Scripts.SceneManagement.ScriptableObjects;
// using Mirror;
// using Steamworks;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace MadSmith.Scripts.Multiplayer.Old.Managers
// {
//     public class LobbyClient : NetworkBehaviour
//     {
//         // Variables
//         private const int NumberOfCharacters = 4; 
//         private const int NumberOfLevels = 6; 
//         [SyncVar] public int ConnectionID;
//         [SyncVar] public ulong PlayerSteamID;
//         [SyncVar] public string PlayerName;
//         [SyncVar(hook = nameof(ChangeCharacter))] public int CharacterId;
//         [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
//         [SyncVar(hook = nameof(ChangeLevelSelected))] public int levelSelected;
//         public bool isLeader;
//         
//         
//         
//         
//         private MadSmithNetworkManager _manager;
//         private MadSmithNetworkManager Manager
//         { get {
//                 if (!ReferenceEquals(_manager, null)) return _manager;
//                 return _manager = NetworkManager.singleton as MadSmithNetworkManager;
//             }
//         }
//         [Header("Listening on")]
//         [SerializeField] private VoidEventChannelSO sceneReady;
//         [SerializeField] private LoadEventChannelSO loadLocation = default;
//         [SerializeField] private GameSceneSO gameplayScene = default;
//
//         private void Start()
//         {
//             DontDestroyOnLoad(gameObject); 
//             // loadLocation.OnLoadingRequested += OnLoadingRequested;
//         }
//         
//
//         // private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
//         // {
//         //     Debug.Log("OnLoadingRequested");
//         // }
//
//
//         // [Header("Broadcasting on")]
//         private void OnDestroy()
//         {
//             // sceneReady.OnEventRaised -= OnSceneReady;
//             // inputReader.MenuPauseEvent -= InputReaderOnMenuPauseEvent;
//         }
//         
//         public override void OnStartAuthority()
//         {
//             //Debug.Log("LobbyClient - OnStartAuthority " + hasAuthority);
//             // var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
//             // if (currentSceneLoaded.sceneType == GameSceneType.Location) return;
//             gameObject.name = "LocalGamePlayer";
//             // sceneReady.OnEventRaised += OnSceneReady;
//             // var currentSceneLoaded = SceneLoader.Instance.GetCurrentSceneLoaded();
//             // Debug.Log("Scene " + currentSceneLoaded.sceneType);
//             // if (currentSceneLoaded.sceneType == GameSceneType.Location) return;
//             // var currentSceneSo = GameManager.Instance.GetSceneSo();
//             // //Debug.Log("Name: " + currentSceneSo.name);
//             // if (currentSceneSo.sceneType != GameSceneType.Menu) return;
//             CmdSetPlayerName(Manager.TransportLayer == TransportLayer.Steam
//                 ? SteamFriends.GetPersonaName().ToString()
//                 : PlayerNameInput.DisplayName); 
//             LobbiesListManager.Instance.DestroyLobbies();
//             // LobbyControllerCanvas.Instance.SetLocalPlayer(this);
//             // LobbyController.Instance.FindLocalPlayer();
//             LobbyControllerCanvas.Instance.UpdateLobbyName();
//             
//         }
//
//         public override void OnStopClient()
//         {
//             Manager.lobbyPlayers.Remove(this);
//             if (LobbyControllerCanvas.InstanceExists)
//             {
//                 LobbyControllerCanvas.Instance.UpdatePlayerList();
//             }
//         }
//         public override void OnStartClient()
//         {
//             //Debug.Log("LobbyClient - OnStartClient" + hasAuthority);   
//             // if (SceneManager.GetActiveScene().name.StartsWith("Level")) return;
//             Manager.lobbyPlayers.Add(this);
//             LobbyControllerCanvas.Instance.UpdateLobbyName();
//             LobbyControllerCanvas.Instance.UpdatePlayerList();
//             
//         }
//
//         [Command]
//         public void CmdSetPlayerName(string playerName)
//         {
//             this.PlayerNameUpdate(this.PlayerName, playerName);
//         }
//         public void PlayerNameUpdate(string oldValue, string newValue)
//         {
//             if (!LobbyControllerCanvas.InstanceExists) return;
//             if (isServer)
//             {
//                 this.PlayerName = newValue;
//             }
//
//             if (isClient)
//             {
//                 LobbyControllerCanvas.Instance.UpdatePlayerList(); // Verificar pq esse atualiza a lista e o ready usa  Item
//             }
//         }
//
//         #region Ready State
//         public void ChangeReady()
//         {
//             if (hasAuthority)
//             {
//                 CmdSetPlayerReady();
//             }
//         }
//         [Command]
//         private void CmdSetPlayerReady()
//         {
//             this.PlayerReadyUpdate(this.ready, !this.ready);
//         }
//         private void PlayerReadyUpdate(bool oldValue, bool newValue)
//         {
//             if (isServer)
//             {
//                 this.ready = newValue;
//             }
//             
//             if (isClient)
//             {
//                 LobbyControllerCanvas.Instance.UpdatePlayerList();
//             }
//         }
//         #endregion
//         
//         #region Character
//         public void NextCharacter()
//         {
//             if (hasAuthority)
//             {
//                 int id = (this.CharacterId + 1) % NumberOfCharacters;
//                 CmdChangeCharacter(this.CharacterId, id);
//             }
//         }
//
//         public void PreviousCharacter()
//         {
//             if (hasAuthority)
//             {
//                 int id = this.CharacterId - 1 < 0 ? NumberOfCharacters - 1 : this.CharacterId - 1;
//                 CmdChangeCharacter(this.CharacterId,id);
//             }
//         }
//         [Command]
//         private void CmdChangeCharacter(int oldId, int newId)
//         {
//             this.ChangeCharacter(oldId, newId);
//         }
//         private void ChangeCharacter(int oldId, int newId)
//         {
//             if (isServer)
//             {
//                 this.CharacterId = newId;
//             }
//             
//             if (isClient)
//             {
//                 LobbyControllerCanvas.Instance.UpdatePlayerList();
//             }
//         }
//         #endregion
//
//         #region StartGame
//
//         [Command]
//         public void CmdStartGame()
//         {
//             if (!isLeader) return;
//             RpcLoadGameplayScene();
//             _manager.StartGame();
//         }
//
//         [ClientRpc]
//         private void RpcLoadGameplayScene()
//         {
//             Debug.Log("RpcLoadGameplayScene");
//             // loadLocation.RaiseEvent(gameplayScene);
//             SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
//         }
//
//         #endregion
//         
//         
//         #region Level
//         public void NextLevel()
//         {
//             
//             if (hasAuthority)
//             {
//                 Debug.Log("NextLevel");
//                 int id = (this.levelSelected + 1) % NumberOfLevels;
//                 CmdChangeLevelSelected(this.levelSelected, id);
//             }
//         }
//
//         public void PreviousLevel()
//         {
//             if (hasAuthority)
//             {
//                 Debug.Log("PreviousLevel");
//                 int id = this.levelSelected - 1 < 0 ? NumberOfLevels - 1 : this.levelSelected - 1;
//                 CmdChangeLevelSelected(this.levelSelected,id);
//             }
//         }
//         [Command]
//         private void CmdChangeLevelSelected(int oldLevel, int newLevel)
//         {
//             this.ChangeLevelSelected(oldLevel, newLevel);
//         }
//         private void ChangeLevelSelected(int oldLevel, int newId)
//         {
//             if (isServer)
//             {
//                 this.levelSelected = newId;
//             }
//             
//             if (isClient)
//             {
//                 // LobbyController.Instance.UpdatePlayerList();
//             }
//         }
//         #endregion
//         
//         
//
//         public void FinishCharacterSelection()
//         {
//             // if (hasAuthority)
//             // {
//             // }
//             //Debug.Log("Can Start Game");
//             CmdFinishCharacterSelection();
//         }
//         [Command]
//         public void CmdFinishCharacterSelection()
//         {
//             // _manager.StartGame(sceneName);
//             //Debug.Log("CmdFinishCharacterSelection");
//             RpcFinishCharacterSelection();
//         }
//         [ClientRpc]
//         private void RpcFinishCharacterSelection()
//         {
//             //Debug.Log("RpcFinishCharacterSelection");
//             // NetworkClient.PrepareToSpawnSceneObjects();
//             LobbyControllerCanvas.Instance.FinishCharacterSelectionPage();
//         }
//     }
// }