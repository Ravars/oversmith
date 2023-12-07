using System;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.UI.Managers;
using MadSmith.Scripts.UI.SettingsScripts;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class UiNetworkHandler : NetworkSingleton<UiNetworkHandler>
    {
        [SyncVar] public int currentLevelSelected;
        [SerializeField] private UILevelSelection uiLevelSelection;
        public void Left()
        {
            if (!isServer)
            {
                return;
            }
            if (currentLevelSelected == 0)
            {
                currentLevelSelected = GameManager.Instance.sceneSos.Length - 1;
            }
            else
            {
                currentLevelSelected--;
            }
            RpcUpdateLevelSelected();
        }
        public void Right()
        {
            if (!isServer)
            {
                return;
            }
            if (currentLevelSelected == GameManager.Instance.sceneSos.Length - 1)
            {
                currentLevelSelected = 0;
            }
            else
            {
                currentLevelSelected++;
            }
            RpcUpdateLevelSelected();
        }
        [ClientRpc]
        private void RpcUpdateLevelSelected()
        {
            uiLevelSelection.UpdateLevelSelected(currentLevelSelected);
        }
    }
}