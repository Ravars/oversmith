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
        // [SerializeField] private voi
        
        
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
            _clientsSpawned = new();
            SpawnNpc();
        }

        private void SpawnNpc()
        {
            int clientIndex = Random.Range(0, clients.Length);
            int pointToSpawnIndex = Random.Range(0, pointsToSpawnNpc.Length);
            int pointToMoveIndex = Random.Range(0, pointsToMoveNpc.Length);
            int desiredItemsIndex = Random.Range(0, listOfDesiredItems.Length);
            
            Client newClient = Instantiate(clients[clientIndex], pointsToSpawnNpc[pointToSpawnIndex].position,
                quaternion.identity);
            _clientsSpawned.Add(newClient);
            newClient.Init(pointsToMoveNpc[pointToMoveIndex].position,_clientsSpawned.Count, this);
        }

        public void ClientArrived(int clientIndex)
        {
            Debug.Log("Client Arrived: " + clientIndex);
            // Spawn Box in unique color
            // Set items required
            
        }
        

    }
}