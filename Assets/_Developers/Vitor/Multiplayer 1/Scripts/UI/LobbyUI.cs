using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts.UI
{
    public class LobbyUI : NetworkBehaviour
    {
        [SerializeField] private GameSceneSO level1; //TODO: mudar para algum manager
        [SerializeField] private InputReader inputReader;
        
        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        
        public void ButtonPlay()
        {
            if (!hasAuthority) return;
            CmdPlayButton();
        }

        [Command]
        private void CmdPlayButton()
        {
            RpcLoadLevel();
        }

        /// <summary>
        /// Manda todos os clients dar load no level
        /// </summary>
        [ClientRpc]
        private void RpcLoadLevel()
        {
            inputReader.DisableAllInput();
            loadLocation.RaiseEvent(level1);
        }
    }
}