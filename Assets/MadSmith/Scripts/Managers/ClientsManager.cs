using System;
using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
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
        
        
        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onSceneReady;
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
            Debug.Log("Startup");
            _clientsSpawned = new();
            Debug.Log(CanSpawnNpc());
            if (CanSpawnNpc())
            {
                Debug.Log("Spawned");
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
            int desiredItemsIndex = Random.Range(0, listOfDesiredItems.Length);
            
            Client newClient = Instantiate(clients[clientIndex], pointsToSpawnNpc[pointToSpawnIndex].position,
                Quaternion.identity);
            _clientsSpawned.Add(newClient);

            var boxColor = pallet.GetUnusedBoxColor();
            int npcId = _clientsSpawned.Count;
            Debug.Log("npc" + npcId);
            if (boxColor != null)
            { 
                pallet.SpawnBox(listOfDesiredItems[desiredItemsIndex],(BoxColor)boxColor,npcId);   
            }
            newClient.Init(pointsToMoveNpc[pointToMoveIndex].position, npcId, this);
        }

        public void ClientArrived(int clientIndex)
        {
            Debug.Log("Client Arrived: " + clientIndex);
            SpawnBoxInPallet();
            // Spawn Box in unique color
            // Set items required
            
        }

        public void SpawnBoxInPallet()
        {
            
        }
        

    }
}