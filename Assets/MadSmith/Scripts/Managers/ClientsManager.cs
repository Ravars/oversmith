using System;
using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Managers
{
    public class ClientsManager : MonoBehaviour
    {
        [SerializeField] private Transform[] pointsToSpawnNpc;
        [SerializeField] private Transform[] pointsToMoveNpc;
        [SerializeField] private Client[] clients;
        [SerializeField] private ItemDeliveryList[] listOfDesiredItems;
        private List<Client> _clientsSpawned;
        private List<DeliveryBox> _deliveryBoxes;
        [SerializeField] private Pallet pallet;

        private int _amountNpcActive = 0;
        private int _maxNpcActive = 3;
        [SerializeField] private int maxAmountNpc = 3;
        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onSceneReady;

        private bool _levelActive;
        private void OnEnable()
        {
            onSceneReady.OnEventRaised += Startup;
        }
        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= Startup;
        }

        private void Startup()
        {
            _clientsSpawned = new();
            pallet.clientsManager = this;
            if (CanSpawnNpc())
            {
                SpawnNpc();
            }
        }

        private void Update()
        {
            if (_amountNpcActive < _maxNpcActive)
            {
                SpawnNpc();
            }
        }

        private bool CanSpawnNpc()
        {
            return pallet.HasEmptyBox();
        }

        private void SpawnNpc()
        {
            int clientIndex = Random.Range(0, clients.Length);
            int pointToSpawnIndex = Random.Range(0, pointsToSpawnNpc.Length);
            int pointToMoveIndex = Random.Range(0, pointsToMoveNpc.Length);
            
            
            Client newClient = Instantiate(clients[clientIndex], pointsToSpawnNpc[pointToSpawnIndex].position,
                Quaternion.identity);
            _clientsSpawned.Add(newClient);

            int npcId = _clientsSpawned.Count;
            _amountNpcActive++;
            newClient.Init(pointsToMoveNpc[pointToMoveIndex].position, npcId, this);
        }

        public void ClientArrived(int clientIndex)
        {
            SpawnBoxInPallet(clientIndex);   
        }

        public void ClientFinish(int clientIndex)
        {
            pallet.DeSpawnBox(clientIndex);
            HudController.Instance.RemoveOrder(clientIndex);
        }

        private void SpawnBoxInPallet(int npcId)
        {
            var boxColor = pallet.GetUnusedBoxColor();
            int desiredItemsIndex = Random.Range(0, listOfDesiredItems.Length);
            if (boxColor != null)
            { 
                pallet.SpawnBox(listOfDesiredItems[desiredItemsIndex],(BoxColor)boxColor,npcId);   
                HudController.Instance.AddOrder(listOfDesiredItems[desiredItemsIndex].Items.ToArray(),npcId, (BoxColor)boxColor);
            }
        }
        

    }
}