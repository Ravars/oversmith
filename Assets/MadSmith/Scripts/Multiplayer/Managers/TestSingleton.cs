using System;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class TestSingleton : NetworkSingleton<TestSingleton>
    {
        
        private void Start()
        {
            
        }


        public void CallCmdFunction()
        {
            Debug.Log("CallCmdFunction");
            CmdTestFunction();
            RpcTestFunction();
        }

        [Command]
        public void CmdTestFunction()
        {
            Debug.Log("CmdTestFunction");
        }

        [ClientRpc]
        public void RpcTestFunction()
        {
            Debug.Log("RpcTestFunction");
        }
    }
}