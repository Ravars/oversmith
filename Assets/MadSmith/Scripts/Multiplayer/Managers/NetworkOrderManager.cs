using System.Collections.Generic;
using _Developers.Nicole.ScriptableObjects.Data_Structures;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class NetworkOrderManager : NetworkSingleton<NetworkOrderManager>
    {
        [SerializeField] private float startingDelay = 5;
        [SerializeField] private float firstOrderDelay = 3;
        [SerializeField] private float orderDelay = 11;
        [SerializeField] private float timeToSingleItem = 60; // 60
        [SerializeField] private int maxConcurrentOrders = 6;
        [SerializeField] private int timeToDeliver = 20;
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

        [Header("Broadcasting to")] 
        [SerializeField] private VoidEventChannelSO onSceneReady;
        [SerializeField] private OrderListUpdateEventChannelSO onOrderListUpdate;
        [SerializeField] private OrderUpdateEventChannelSO onCreateOrder;
        [SerializeField] private IntEventChannelSO onMissedOrder;
        [SerializeField] private OrderUpdateEventChannelSO onDeliveryOrder;
        [SerializeField] private IntEventChannelSO onCountdownTimerUpdated;

        private void Start()
        {
            //Debug.Log("Listening onSceneReady");
            onSceneReady.RaiseEvent();
            Setup();
            Invoke(nameof(StartGame), startingDelay);
            _firstOrderAlreadySpawned = false;
            
            // onSceneReady.OnEventRaised += Setup;
            // Setup();
            // StartGame();
            // if (!isServer) return;
            // onLevelStart.OnEventRaised += StartGame;
        }
        private void OnDisable()
        {
            // onSceneReady.OnEventRaised -= Setup;
            // if (!isServer) return;
            // onLevelStart.OnEventRaised -= StartGame;
        }
        private void Setup()
        {
            Debug.Log("Start order manager");
            
        }
        private void StartGame() 
        {
            // show hud
            if (!isServer) return;
            _timeToSpawn = Time.fixedTime + firstOrderDelay;
            _hasBeenStarted = true;
            _currentTime = timeToDeliver;
            var currentSceneSo = GameManager.Instance.GetCurrentScene();
            Debug.Log("CurrentSceneSo" + currentSceneSo.name);
            if (currentSceneSo.sceneType == GameSceneType.Location)
            {
                var location = (LocationSO)currentSceneSo;
                _levelConfigItems = location.levelConfigItems;
            }
            else
            {
                return;
            }
            UpdateTimers(currentOrderListTimes, (int)_currentTime);
        }
        
        

        private void FixedUpdate()
        {
            if (!isServer || !_hasBeenStarted) return;
            
            float percentToRemove = Time.fixedDeltaTime / timeToSingleItem;
            _currentTime -= Time.fixedDeltaTime;

            // List<OrderTimes> list = new List<OrderTimes>();
            // list

            UpdateTimers(currentOrderListTimes, (int)_currentTime);
            // update times
            for (int i = 0; i < currentOrderList.Count; i++)
            {
                var orderListTime = currentOrderList[i];
                orderListTime.TimeRemaining01 -= percentToRemove;
                
                var orderData = currentOrderListTimes[i];
                orderData.TimeRemaining01 = orderListTime.TimeRemaining01;
            }

            //Remove by time expired
            // Debug.Log("currentOrderListTimes.Count:" + (currentOrderListTimes.Count - 1));
            if (currentOrderListTimes.Count > 0)
            {
                for (int j = currentOrderListTimes.Count-1; j > -1; j--)
                {
                    Debug.Log(j);
                    if (currentOrderListTimes[j].TimeRemaining01 <= 0)
                    {
                        MissedOrder(currentOrderListTimes[j].Id);
                        currentOrderListTimes.RemoveAt(j); 
                        currentOrderList.RemoveAt(j); 
                    }
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
        private void UpdateTimers(List<OrderTimes> orderListTimes, int currentTime)
        {
            onCountdownTimerUpdated.RaiseEvent(currentTime);
            onOrderListUpdate.RaiseEvent(orderListTimes);
        }
        [ClientRpc]
        private void MissedOrder(int i)
        {
            Debug.Log("ID Missed: " + i);
            onMissedOrder.RaiseEvent(i);
            //Verificar se fica aqui ou fora do RPC
        }

        [ClientRpc]
        private void SpawnOrder(int itemIndex)
        {
            BaseItem newItem = _levelConfigItems.itemsToDelivery[itemIndex];
            var value = _lastOrderId++;
            Debug.Log("Value: " + value);
            var newOrderData = new OrderData(value, 1, newItem);
            var newOrderTime = new OrderTimes(value, 1);
            currentOrderList.Add(newOrderData);
            currentOrderListTimes.Add(newOrderTime);
            //Debug.Log("Spawn order");
            onCreateOrder.RaiseEvent(newOrderData);
        }
        public bool CheckOrder(BaseItem item)
        {
            var orderData = currentOrderList.Find(x => x.BaseItem.id == item.id);
            //Debug.Log("oderData" + orderData);
            if (ReferenceEquals(orderData, null)) return false;
            //Debug.Log("onDeliveryOrder.RaiseEvent");
            onDeliveryOrder.RaiseEvent(orderData);
            currentOrderList.Remove(orderData);
            return true;
        }
    }
}