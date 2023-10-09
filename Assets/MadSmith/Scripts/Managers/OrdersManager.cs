using System;
using System.Collections.Generic;
using _Developers.Nicole.ScriptableObjects.Data_Structures;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Managers
{
	public class OrdersManager : MonoBehaviour
	{
		[SerializeField] private float firstOrderDelay = 3;
		[SerializeField] private float orderDelay = 11;
		[SerializeField] private float timeToSingleItem = 60;
		[SerializeField] private int maxConcurrentOrders = 6;
		private float _timeToSpawn;
		private bool _firstOrderAlreadySpawned;

		[SerializeField] private LevelConfigItems levelConfigItems;
		private int _lastOrderId;
		
		[SerializeField]  private List<OrderData> currentOrderList = new ();
		
		[Header("Listening to")]
		[SerializeField] private VoidEventChannelSO onLevelStart;

		[Header("Broadcasting to")] 
		[SerializeField] private OrderListUpdateEventChannelSO onOrderListUpdate;
		[SerializeField] private OrderUpdateEventChannelSO onCreateOrder;
		[SerializeField] private OrderUpdateEventChannelSO onDeleteOrder;

		private bool _hasBeenStarted;

		private void Start()
		{
			_firstOrderAlreadySpawned = false;
			onLevelStart.OnEventRaised += StartGame;
		}

		private void OnDisable()
		{
			onLevelStart.OnEventRaised -= StartGame;
		}
		private void FixedUpdate()
		{
			if (!_hasBeenStarted) return;
			float percentToRemove = Time.fixedDeltaTime / timeToSingleItem;
			
			// update times
			foreach (var orderData in currentOrderList)
			{
				orderData.TimeRemaining01 -= percentToRemove;
			}
			
			//Remove by time expired
			for (int i = currentOrderList.Count-1; i >= 0; i--)
			{
				if (currentOrderList[i].TimeRemaining01 <= 0)
				{
					onDeleteOrder.RaiseEvent(currentOrderList[i]);
					currentOrderList.RemoveAt(i);
				}
			}
			onOrderListUpdate.RaiseEvent(currentOrderList);
			
			if (Time.fixedTime >= _timeToSpawn && currentOrderList.Count < maxConcurrentOrders || (_firstOrderAlreadySpawned && currentOrderList.Count == 0))
			{
				BaseItem newItem = levelConfigItems.itemsToDelivery[Random.Range(0, levelConfigItems.itemsToDelivery.Length)];
				var newOrderData = new OrderData(_lastOrderId++, 1, newItem);
				currentOrderList.Add(newOrderData);
				onCreateOrder.RaiseEvent(newOrderData);
				_timeToSpawn = Time.fixedTime + (_firstOrderAlreadySpawned ? orderDelay : firstOrderDelay );
				_firstOrderAlreadySpawned = true;
			}
		}

		private void StartGame() 
		{
			// show hud
			_timeToSpawn = Time.fixedTime + firstOrderDelay;
			_hasBeenStarted = true;
		}


		// [SerializeField] private ItemDeliveryList listOfItems;
		//
		// [Header("Listening to")]
		// [SerializeField] private VoidEventChannelSO onSceneReady;
		//
		// [Header("Broadcasting to")]
		// [SerializeField] private FloatEventChannelSO _onLevelCompleted;
		// [SerializeField] private IntEventChannelSO _onCountdownTimerUpdated;
		// [SerializeField] private VoidEventChannelSO _onItemDelivering;
		// [SerializeField] private VoidEventChannelSO _onItemMissed;
		//
		// [SerializeField] private float firstOrderDelay = 5;
		// [SerializeField] private float nextOrderDelay = 7;
		// [SerializeField] private int maxOrders = 7;
		// [SerializeField] private float timeToDeliver = 60f;
		// private float _currentTime;
		//
		// private bool _isLevelCompleted = false;
		// private bool _isOnOrderDelay;
		// private int _orderCount;
		//
		// private List<BaseItem> _availableItems = new List<BaseItem>();
		// private List<ItemCardHolder> _activeOrders = new List<ItemCardHolder>();
		// private List<ItemCardHolder> _ordersToRemove = new List<ItemCardHolder>();
		//
		//
		// private void OnEnable()
		// {
		// 	onSceneReady.OnEventRaised += Startup;
		// }
		// private void OnDisable()
		// {
		// 	onSceneReady.OnEventRaised -= Startup;
		// }
		// private void Startup()
		// {
		// 	_currentTime = timeToDeliver;
		// 	_onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
		// 	Invoke(nameof(CreateOrder), firstOrderDelay);
		// 	foreach (var item in listOfItems.Items)
		// 	{
		// 		_availableItems.Add(item.BaseItem);
		// 	}
		// }
		//
		// private void Update()
		// {
		// 	if (!HudController.InstanceExists) return; // Only to avoid errors
		// 	_currentTime -= Time.deltaTime;
		// 	_onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
		// 	if (!_isLevelCompleted && _currentTime <= 0)
		// 	{
		// 		// Level end by time
		// 		
		// 		HudController.Instance.ClearCardHolders();
		// 		
		// 		_onLevelCompleted.RaiseEvent((ScoreManager.Instance.PlayerScore / ScoreManager.Instance.TotalScore) * 100);
		//
		// 		_isLevelCompleted = true;
		// 	}
		// 	for(int i = _activeOrders.Count - 1; i >= 0; i--)
		// 	{
		// 		_activeOrders[i].slider.value -= Time.deltaTime;
		// 		if (_activeOrders[i].slider.value <= 0)
		// 		{
		// 			HudController.Instance.RemoveOrder(_activeOrders[i].id);
		// 			_activeOrders.RemoveAt(i);
		//
		// 			if (_availableItems.Count > 0 && !_isOnOrderDelay)
		// 			{
		// 				Invoke(nameof(CreateOrder), nextOrderDelay);
		// 				_isOnOrderDelay = true;
		// 			}
		//
		// 			_onItemMissed.RaiseEvent();
		// 		}
		// 	}
		// }
		//
		// private void CreateOrder()
		// {
		// 	_isOnOrderDelay = false;
		//
		// 	int orderNumber = UnityEngine.Random.Range(0, _availableItems.Count);
		// 	
		// 	_activeOrders.Add(HudController.Instance.AddOrder(_availableItems[orderNumber], _orderCount));
		//
		// 	_availableItems.Remove(_availableItems[orderNumber]);
		//
		// 	_orderCount++;
		//
		// 	if (_activeOrders.Count < maxOrders && _availableItems.Count > 0)
		// 	{
		// 		Invoke(nameof(CreateOrder), nextOrderDelay);
		// 		_isOnOrderDelay = true;
		// 	}
		// }
		//
		// public void CheckOrder(BaseItem item)
		// {
		// 	var a = _activeOrders.Find(x => x.baseItem.Equals(item));
		// 	if (!ReferenceEquals(a, null))
		// 	{
		// 		HudController.Instance.RemoveOrder(a.id);
		// 		_activeOrders.Remove(a);
		//
		// 		_onItemDelivering.RaiseEvent();
		//
		// 		if (_availableItems.Count > 0 && !_isOnOrderDelay)
		// 		{
		// 			Invoke(nameof(CreateOrder), nextOrderDelay);
		// 			_isOnOrderDelay = true;
		// 		}
		// 	}
		// }
	}
}