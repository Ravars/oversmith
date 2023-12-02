using System;
using MadSmith.Scripts.UI.Managers;
using MadSmith.Scripts.UI.SettingsScripts;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class UiNetworkHandler : NetworkSingleton<UiNetworkHandler>
    {
        [SerializeField] private UILevelSelection uiLevelSelection;
        public void Left()
        {
            if (!isServer)
            {
                return;
            }
            RpcLeftButton();
        }
        [ClientRpc]
        private void RpcLeftButton()
        {
            uiLevelSelection.LeftButton();
        }
        public void Right()
        {
            if (!isServer)
            {
                return;
            }
            RpcRightButton();
        }
        [ClientRpc]
        private void RpcRightButton()
        {
            uiLevelSelection.RightButton();
        }

        public void Play()
        {
            RpcPlay();
        }

        [ClientRpc]
        private void RpcPlay()
        {
            uiLevelSelection.Play();
        }
    }
}