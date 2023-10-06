using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Managers
{
	public class OrdersManager : MonoBehaviour
	{
		[SerializeField] private ItemDeliveryList listOfItems;

		[Header("Listening to")]
		[SerializeField] private VoidEventChannelSO onSceneReady;

		[Header("Broadcasting to")]
		[SerializeField] private FloatEventChannelSO _onLevelCompleted;
		[SerializeField] private IntEventChannelSO _onCountdownTimerUpdated;
		[SerializeField] private VoidEventChannelSO _onItemDelivering;
		[SerializeField] private VoidEventChannelSO _onItemMissed;

		[SerializeField] private float firstOrderDelay = 5;
		[SerializeField] private float nextOrderDelay = 7;
		[SerializeField] private int maxOrders = 7;
		[SerializeField] private float timeToDeliver = 60f;
		private float _currentTime;

		private bool _isLevelCompleted = false;
		private bool _isOnOrderDelay;
		private int _orderCount;

		private List<BaseItem> _availableItems = new List<BaseItem>();
		private List<ItemCardHolder> _activeOrders = new List<ItemCardHolder>();
		private List<ItemCardHolder> _ordersToRemove = new List<ItemCardHolder>();


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
			_currentTime = timeToDeliver;
			_onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
			Invoke(nameof(CreateOrder), firstOrderDelay);
			foreach (var item in listOfItems.Items)
			{
				_availableItems.Add(item.BaseItem);
			}
		}

		private void Update()
		{
			if (!HudController.InstanceExists) return; // Only to avoid errors
			_currentTime -= Time.deltaTime;
			_onCountdownTimerUpdated.RaiseEvent((int)_currentTime);
			if (!_isLevelCompleted && _currentTime <= 0)
			{
				// Level end by time
				
				HudController.Instance.ClearCardHolders();
				
				_onLevelCompleted.RaiseEvent((ScoreManager.Instance.PlayerScore / ScoreManager.Instance.TotalScore) * 100);

				_isLevelCompleted = true;
			}
			for(int i = _activeOrders.Count - 1; i >= 0; i--)
			{
				_activeOrders[i].slider.value -= Time.deltaTime;
				if (_activeOrders[i].slider.value <= 0)
				{
					HudController.Instance.RemoveOrder(_activeOrders[i].id);
					_activeOrders.RemoveAt(i);

					if (_availableItems.Count > 0 && !_isOnOrderDelay)
					{
						Invoke(nameof(CreateOrder), nextOrderDelay);
						_isOnOrderDelay = true;
					}

					_onItemMissed.RaiseEvent();
				}
			}
		}

		private void CreateOrder()
		{
			_isOnOrderDelay = false;

			int orderNumber = UnityEngine.Random.Range(0, _availableItems.Count);
			
			_activeOrders.Add(HudController.Instance.AddOrder(_availableItems[orderNumber], _orderCount));

			_availableItems.Remove(_availableItems[orderNumber]);

			_orderCount++;

			if (_activeOrders.Count < maxOrders && _availableItems.Count > 0)
			{
				Invoke(nameof(CreateOrder), nextOrderDelay);
				_isOnOrderDelay = true;
			}
		}

		public void CheckOrder(BaseItem item)
		{
			var a = _activeOrders.Find(x => x.baseItem.Equals(item));
			if (!ReferenceEquals(a, null))
			{
				HudController.Instance.RemoveOrder(a.id);
				_activeOrders.Remove(a);

				_onItemDelivering.RaiseEvent();

				if (_availableItems.Count > 0 && !_isOnOrderDelay)
				{
					Invoke(nameof(CreateOrder), nextOrderDelay);
					_isOnOrderDelay = true;
				}
			}
		}
	}
}