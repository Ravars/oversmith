using System;
using MadSmith.Scripts.UI.Managers;
using MadSmith.Scripts.UI.SettingsScripts;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class TestSingleton : NetworkSingleton<TestSingleton>
    {
        [SerializeField] private UILevelSelection uiLevelSelection;
        private void Start()
        {
            
        }


        public void CallCmdFunction()
        {
            Debug.Log("CallCmdFunction");
            // CmdTestFunction();
            RpcTestFunction();
        }

        // [Command]
        // public void CmdTestFunction()
        // {
        //     Debug.Log("CmdTestFunction");
        //     RpcTestFunction();
        // }

        [ClientRpc]
        public void RpcTestFunction()
        {
            Debug.Log("RpcTestFunction");
            
        }

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
            Debug.Log("Rpc Left");
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
            Debug.Log("Rpc Right");
            uiLevelSelection.RightButton();
        }
    }
}