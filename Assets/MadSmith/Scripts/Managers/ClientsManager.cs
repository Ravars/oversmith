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
        //[SerializeField] private Transform[] pointsToSpawnNpc;
        //[SerializeField] private Transform[] pointsToMoveNpc;
        //[SerializeField] private Client[] clients;
        //[SerializeField] private ItemDeliveryList[] listOfDesiredItems;
        //private List<Client> _clientsSpawned;
        //private List<DeliveryBox> _deliveryBoxes;
        //[SerializeField] private Pallet pallet;
        
        
        //private const int MaxNpcActive = 3;
        //[SerializeField] private int maxNpcPerLevel = 5;
        //private int _amountNpcActive = 0;
        //private int _amountNpcFinished = 0;

        //private float _timeToNextNpcSpawn;
        //private float _timeBetweenSpawns = 5;
        
        
        //[Header("Listening to")]
        //[SerializeField] private VoidEventChannelSO onSceneReady;
        
        //[Header("Broadcasting to")]
        //[SerializeField] private IntEventChannelSO _onLevelCompleted;

        //private bool _levelActive;
        //private void OnEnable()
        //{
        //    onSceneReady.OnEventRaised += Startup;
        //}
        //private void OnDisable()
        //{
        //    onSceneReady.OnEventRaised -= Startup;
        //}

        //private void Startup()
        //{
        //    _clientsSpawned = new();
        //    pallet.clientsManager = this;
        //    if (CanSpawnNpc())
        //    {
        //        _timeToNextNpcSpawn = Time.time + _timeBetweenSpawns;
        //        _amountNpcActive++;
        //        SpawnNpc();
        //    }
        //}

        //private void Update()
        //{
        //    if (_amountNpcActive < MaxNpcActive && _timeToNextNpcSpawn < Time.time)
        //    {
        //        _timeToNextNpcSpawn = Time.time + _timeBetweenSpawns;
        //        _amountNpcActive++;
        //        SpawnNpc();
        //    }
        //}

        //private bool CanSpawnNpc()
        //{
        //    return pallet.HasEmptyBox();
        //}

        //private void SpawnNpc()
        //{
        //    int clientIndex = Random.Range(0, clients.Length);
        //    int pointToSpawnIndex = Random.Range(0, pointsToSpawnNpc.Length);
        //    int pointToMoveIndex = Random.Range(0, pointsToMoveNpc.Length);
        //    Client newClient = Instantiate(clients[clientIndex], pointsToSpawnNpc[pointToSpawnIndex].position,
        //        Quaternion.identity);
        //    _clientsSpawned.Add(newClient);

        //    int npcId = _clientsSpawned.Count;
            
        //    newClient.Init(pointsToMoveNpc[pointToMoveIndex].position, npcId, this);
        //}

        //public void ClientArrived(int clientIndex)
        //{
        //    SpawnBoxInPallet(clientIndex);   
        //}

        //public void ClientFinish(int clientIndex)
        //{
        //    _amountNpcActive--;
        //    _timeToNextNpcSpawn = Time.time + _timeBetweenSpawns;
        //    pallet.DeSpawnBox(clientIndex);
        //    HudController.Instance.RemoveOrder(clientIndex);
        //    int pointToMoveAwayIndex = Random.Range(0, pointsToSpawnNpc.Length);
        //    _clientsSpawned[clientIndex].MoveAway(pointsToSpawnNpc[pointToMoveAwayIndex].position);
        //    _amountNpcFinished++;
        //    if (_amountNpcFinished >= maxNpcPerLevel)
        //    {
        //        int grade = GetFinalGrade();
        //        _onLevelCompleted.RaiseEvent(grade);
        //    }
            
        //}

        //private void SpawnBoxInPallet(int npcId)
        //{
        //    var boxColor = pallet.GetUnusedBoxColor();
        //    int desiredItemsIndex = Random.Range(0, listOfDesiredItems.Length);
        //    if (boxColor != null)
        //    { 
        //        pallet.SpawnBox(listOfDesiredItems[desiredItemsIndex],(BoxColor)boxColor,npcId);   
        //        HudController.Instance.AddOrder(listOfDesiredItems[desiredItemsIndex].Items.ToArray(),npcId, (BoxColor)boxColor);
        //    }
        //}
        //private int GetFinalGrade()
        //{
        //    // int baseScore = _totalScore > 0 ? Mathf.RoundToInt((float)_totalScore / customers.Count) : 0;
        //    // baseScore -= 10;
        //    //
        //    // int finalRatio = Mathf.RoundToInt((_timeRatiosSum / customers.Count) * 10); //in percentage
        //    // int finalScore = finalRatio + baseScore;
        //    //
        //    // return finalScore;
        //    return 100;
        //}
        

    }
}