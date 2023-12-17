using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Multiplayer.UI;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class MadSmithNetworkRoomPlayer : NetworkRoomPlayer
    {
        [SyncVar] public int ConnectionID;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar] public string PlayerName;
        // [SyncVar] public bool ready;
        // [SyncVar] public int CharacterId;
        [SerializeField] private Button startButton;
        // [SerializeField] private TextMeshProUGUI readyText;
        // [SerializeField] private TextMeshProUGUI startButtonText;
        [SerializeField] private GameObject localUI;
        public bool isLeader;
        private const int NumberOfCharacters = 4;
        private const int NumberOfLevels = 9;
        [SyncVar(hook = nameof(ChangeCharacter))] public int CharacterId;
        [SyncVar(hook = nameof(ChangeLevel))] public int LevelSelected;
        // [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
        
        [Header("Broadcasting ong")]
        [SerializeField] private VoidEventChannelSO onUpdatePlayerList = default;
        [SerializeField] private IntEventChannelSO onUpdateLevel = default;
        
        private MadSmithNetworkRoomManager _manager;
        public MadSmithNetworkRoomManager Manager
        { get
            {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
        public override void OnStartClient()
        {
            // Debug.Log($"OnStartClient {gameObject}");
            startButton.enabled = isServer;
            if (hasAuthority)
            {
                UiRoomManager.Instance.SetLocalPlayer(this);
            }
        }

        public override void OnClientEnterRoom()
        {
            // Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
            onUpdatePlayerList.RaiseEvent();
        }

        public override void OnClientExitRoom()
        {
            // Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
            onUpdatePlayerList.RaiseEvent();
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
            onUpdatePlayerList.RaiseEvent();
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
            // SetReadyText();
            onUpdatePlayerList.RaiseEvent();
        }

        public void NextPageButton()
        {
            if (Manager.allPlayersReady && isServer)
            {
                RpcMessageToClient();
                // Manager.ServerChangeScene(Manager.GameplayScene);
            }
        }

        [ClientRpc]
        public void RpcMessageToClient()
        {
            UiRoomManager.Instance.SetState(MenuRoomState.LevelSelection);
        }

        public void ToggleReadyButton()
        {
            CmdChangeReadyState(!readyToBegin);
        }
        
        #region Character
        public void NextCharacter()
        {
            if (hasAuthority)
            {
                int id = (this.CharacterId + 1) % NumberOfCharacters;
                CmdChangeCharacter(this.CharacterId, id);
            }
        }

        public void PreviousCharacter()
        {
            if (hasAuthority)
            {
                int id = this.CharacterId - 1 < 0 ? NumberOfCharacters - 1 : this.CharacterId - 1;
                CmdChangeCharacter(this.CharacterId,id);
            }
        }
        [Command]
        private void CmdChangeCharacter(int oldId, int newId)
        {
            this.ChangeCharacter(oldId, newId);
        }
        private void ChangeCharacter(int oldId, int newId)
        {
            if (isServer)
            {
                this.CharacterId = newId;
            }
            
            if (isClient)
            {
                onUpdatePlayerList.RaiseEvent();
            }
        }
        #endregion


        #region Level
        public void NextLevel()
        {
            if (hasAuthority && isLeader)
            {
                int id = (this.LevelSelected + 1) % NumberOfLevels;
                CmdChangeLevel(this.LevelSelected, id);
            }
        }

        public void PreviousLevel()
        {
            if (hasAuthority && isLeader)
            {
                int id = this.LevelSelected - 1 < 0 ? NumberOfLevels - 1 : this.LevelSelected - 1;
                CmdChangeLevel(this.LevelSelected,id);
            }
        }

        [Command]
        private void CmdChangeLevel(int oldLevel, int newLevel)
        {
            this.ChangeLevel(oldLevel, newLevel);
        }
        
        public void ChangeLevel(int oldLevel, int newLevel)
        {
            if (isServer)
            {
                this.LevelSelected = newLevel;
            }

            if (isClient)
            {
                onUpdateLevel.RaiseEvent(newLevel);
            }
        }
        #endregion

        #region PlayGame

        public void PlayGameButton()
        {
            if (!hasAuthority || !isLeader) return;
            
            LocationSO locationSo = GameManager.Instance.sceneSos[LevelSelected] as LocationSO;
            if (locationSo != null)
            {
                string sceneName = locationSo.sceneName;
                if (isClient)
                {
                    CmdSetGameSceneSo(LevelSelected);
                }
                Manager.ServerChangeScene(sceneName);
            }
            else
            {
                Debug.LogError("Location not found!!");
            }
        }

        [Command]
        public void CmdSetGameSceneSo(int locationIndex)
        {
            GameManager.Instance.SetGameSceneSo(locationIndex);
        }

        #endregion

        [ContextMenu("Test")]
        public void Test()
        {
            //Debug.Log(GameManager.Instance.CurrentSceneSo.name + " - " + GameManager.Instance.currentSceneIndex);
        }
        
    }
}