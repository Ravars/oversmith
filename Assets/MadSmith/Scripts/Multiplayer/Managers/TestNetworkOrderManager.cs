    using System;
using System.Collections.Generic;
using _Developers.Nicole.ScriptableObjects.Data_Structures;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class TestNetworkOrderManager : NetworkBehaviour
    {
        [SerializeField] private float firstOrderDelay = 3;
        [SerializeField] private float orderDelay = 11;
        [SerializeField] private float timeToSingleItem = 60;
        [SerializeField] private int maxConcurrentOrders = 6;
        [SerializeField] private int timeToDeliver = 240;
        private float _timeToSpawn;
        private bool _firstOrderAlreadySpawned;
        private float _currentTime;
        private bool _hasBeenStarted;
        private LevelConfigItems _levelConfigItems;
        private int _lastOrderId;
		
        [SerializeField]  private List<OrderData> currentOrderList = new ();
        [SerializeField]  private List<OrderTimes> currentOrderListTimes = new ();
		
        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO onLevelStart;
        [SerializeField] private VoidEventChannelSO onSceneReady;

        [Header("Broadcasting to")] 
        [SerializeField] private OrderListUpdateEventChannelSO onOrderListUpdate;
        [SerializeField] private OrderUpdateEventChannelSO onCreateOrder;
        [SerializeField] private OrderUpdateEventChannelSO onMissedOrder;
        [SerializeField] private OrderUpdateEventChannelSO onDeliveryOrder;
        [SerializeField] private IntEventChannelSO onCountdownTimerUpdated;

        private void Start()
        {
            onSceneReady.OnEventRaised += Setup;
            _firstOrderAlreadySpawned = false;
            // if (!isServer) return;
            onLevelStart.OnEventRaised += StartGame;
        }
        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= Setup;
            // if (!isServer) return;
            onLevelStart.OnEventRaised -= StartGame;
        }
        private void Setup()
        {
            var currentSceneSo = GameManager.Instance.CurrentSceneSo;
            if (currentSceneSo.sceneType == GameSceneType.Location)
            {
                var location = (LocationSO)currentSceneSo;
                _levelConfigItems = location.levelConfigItems;
            }
            else
            {
                return;
            }
        }
        private void StartGame() 
        {
            // show hud
            _timeToSpawn = Time.fixedTime + firstOrderDelay;
            _hasBeenStarted = true;
            _currentTime = timeToDeliver;
        }

        private void FixedUpdate()
        {
            if (!isServer || !_hasBeenStarted) return;
            
            float percentToRemove = Time.fixedDeltaTime / timeToSingleItem;
            _currentTime -= Time.fixedDeltaTime;

            // List<OrderTimes> list = new List<OrderTimes>();
            // list

            UpdateTimers(currentOrderListTimes);
            // update times
            for (var i = 0; i < currentOrderList.Count; i++)
            {
                var orderListTime = currentOrderList[i];
                orderListTime.TimeRemaining01 -= percentToRemove;
                
                var orderData = currentOrderListTimes[i];
                orderData.TimeRemaining01 = orderListTime.TimeRemaining01;
            }

            //Remove by time expired
            for (int i = currentOrderList.Count-1; i >= 0; i--)
            {
                if (currentOrderList[i].TimeRemaining01 <= 0)
                {
                    MissedOrder(i);
                    currentOrderList.RemoveAt(i); 
                    currentOrderListTimes.RemoveAt(i); 
                }
            }

            if (Time.fixedTime >= _timeToSpawn && currentOrderList.Count < maxConcurrentOrders || (_firstOrderAlreadySpawned && currentOrderList.Count == 0))
            {
                int itemIndex = Random.Range(0, _levelConfigItems.itemsToDelivery.Length);
                SpawnOrder(itemIndex);
                
                // BaseItem newItem = _levelConfigItems.itemsToDelivery[itemIndex];
                // var newOrderData = new OrderData(_lastOrderId++, 1, newItem);
                // currentOrderList.Add(newOrderData);
                _firstOrderAlreadySpawned = true;
                
                _timeToSpawn = Time.fixedTime + (_firstOrderAlreadySpawned ? orderDelay : firstOrderDelay );
            }
        }

        [ClientRpc]
        private void UpdateTimers(List<OrderTimes> orderListTimes)
        {
            onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
            onOrderListUpdate.RaiseEvent(orderListTimes);
        }
        [ClientRpc]
        private void MissedOrder(int i)
        {
            onMissedOrder.RaiseEvent(currentOrderList[i]);
            //Verificar se fica aqui ou fora do RPC
        }

        [ClientRpc]
        private void SpawnOrder(int itemIndex)
        {
            BaseItem newItem = _levelConfigItems.itemsToDelivery[itemIndex];
            var value = _lastOrderId++;
            var newOrderData = new OrderData(value, 1, newItem);
            var newOrderTime = new OrderTimes(value, 1);
            currentOrderList.Add(newOrderData);
            currentOrderListTimes.Add(newOrderTime);
            onCreateOrder.RaiseEvent(newOrderData);
        }
    }
}