using Mirror;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts.UI
{
    public class MainMenuNetwork : NetworkBehaviour
    {
        [SerializeField] private UIMenuManager uiMenuManager;
        [SerializeField] private GameObject menuHolder;
        [SerializeField] private LobbyClient lobbyClient;
        public override void OnStartAuthority()
        {
            if (!hasAuthority) return;
            menuHolder.gameObject.SetActive(true);
        }

        public void NewGameButton()
        {
            // CmdNewGame();
            lobbyClient.CmdChangeMenu();
        }

        [Command]
        private void CmdNewGame()
        {
            // RpcNewGame();
            // Managet
            // RpcNewGame();
        }
        [ClientRpc]
        private void RpcNewGame()
        {
            // if (!hasAuthority) return;
            Debug.Log("RpcNewGame" + gameObject.name + " " + NetworkClient.connection.connectionId);
            uiMenuManager.SetState(MenuState.CharacterSelection);
        }
    }
}