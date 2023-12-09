using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class MadSmithNetworkRoomPlayer : NetworkRoomPlayer
    {
        [SyncVar] public int ConnectionID;
        [SyncVar] public ulong PlayerSteamID;
        [SyncVar] public string PlayerName;
        [SyncVar] public bool ready;
        [SyncVar] public int CharacterId;
        // [SyncVar(hook = nameof(ChangeCharacter))] public int CharacterId;
        // [SyncVar(hook = nameof(PlayerReadyUpdate))] public bool ready;
        public override void OnStartClient()
        {
            Debug.Log($"OnStartClient {gameObject}");
        }

        public override void OnClientEnterRoom()
        {
            Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
            LobbyControllerCanvas.Instance.UpdatePlayerList();
        }

        public override void OnClientExitRoom()
        {
            Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}