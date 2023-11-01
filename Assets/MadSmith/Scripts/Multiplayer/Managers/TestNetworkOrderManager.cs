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

        public float timeToWait = 5;
        private float _time;
        private LevelConfigItems _levelConfigItems;
        private int _lastOrderId;
        [SerializeField]  private List<OrderData> currentOrderList = new ();
        
        [SerializeField] private float timeToSingleItem = 60;
        private float _currentTime = 240;
        
        [SerializeField] private OrderUpdateEventChannelSO onCreateOrder;
        [SerializeField] private IntEventChannelSO onCountdownTimerUpdated;
        [SerializeField] private OrderListUpdateEventChannelSO onOrderListUpdate;

        private void Awake()
        {
            _time = Time.fixedTime + timeToWait;
        }

        private void Start()
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

        private void FixedUpdate()
        {
            if (!isServer)
            {
                return;
            }
            float percentToRemove = Time.fixedDeltaTime / timeToSingleItem;
            _currentTime -= Time.fixedDeltaTime;
            UpdateTimers();
            
            // update times
            foreach (var orderData in currentOrderList)
            {
                orderData.TimeRemaining01 -= percentToRemove;
            }

            if (Time.time > _time)
            {
                _time = Time.fixedTime + timeToWait;
                SpawnOrder();
            }
        }

        [ClientRpc]
        private void UpdateTimers()
        {
            onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
            onOrderListUpdate.RaiseEvent(currentOrderList);
        }

        [ClientRpc]
        private void SpawnOrder()
        {
            BaseItem newItem = _levelConfigItems.itemsToDelivery[Random.Range(0, _levelConfigItems.itemsToDelivery.Length)];
            var newOrderData = new OrderData(_lastOrderId++, 1, newItem);
            currentOrderList.Add(newOrderData);
            onCreateOrder.RaiseEvent(newOrderData);
        }
    }
}