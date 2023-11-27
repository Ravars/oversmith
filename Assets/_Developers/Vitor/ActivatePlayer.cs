using System;
using System.Collections.Generic;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Multiplayer.Player;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor
{
    public class ActivatePlayer : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private void Start()
        {
            inputReader.DashEvent += InputReaderOnDashEvent;
        }

        private void OnDestroy()
        {
            inputReader.DashEvent -= InputReaderOnDashEvent;
        }

        private void InputReaderOnDashEvent()
        {
            Debug.Log("InputReaderOnDashEvent");
            var manager = NetworkManager.singleton as TestNetworkManager;
            if (manager != null) manager.LoadScenes();
        }
    }
}