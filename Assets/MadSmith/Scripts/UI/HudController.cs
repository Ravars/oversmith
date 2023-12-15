using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class HudController : Singleton<HudController>
    {
        // public GameObject Card;
        public ItemCardHolder orderCardPrefab;
        public GameObject hudPanel;
        public Transform orderCardHolder;
        public List<ItemCardHolder> ItemCardHolders;

        [SerializeField] private Slider playerScoreSlider;
        [SerializeField] private Slider enemyScoreSlider;
        
        
        [SerializeField] private TextMeshProUGUI timerText;
        [Header("Listening on")] 
        // [SerializeField] private VoidEventChannelSO _onSceneReady = default;
        [SerializeField] private IntEventChannelSO _onCountdownTimerUpdated = default;
        [SerializeField] private FloatEventChannelSO onPlayerScore;
        [SerializeField] private FloatEventChannelSO onEnemyScore;
        
        [SerializeField] private OrderListUpdateEventChannelSO onOrderListUpdate;
        [SerializeField] private OrderUpdateEventChannelSO onCreateOrder;
        [SerializeField] private OrderUpdateEventChannelSO onDeleteOrder;
        [SerializeField] private OrderUpdateEventChannelSO onDeliveryOrder;

        private void Start() // Era no OnEnable
        {
            _onCountdownTimerUpdated.OnEventRaised += UpdateTimer;
            onPlayerScore.OnEventRaised += UpdatePlayerScore;
            onEnemyScore.OnEventRaised += UpdateEnemyScore;
            onOrderListUpdate.OnEventRaised += OnOrderListUpdate;
            onCreateOrder.OnEventRaised += CreateOrder;
            onDeleteOrder.OnEventRaised += DeleteOrder;
            onDeliveryOrder.OnEventRaised += DeleteOrder; // TODO: mudar para uma função diferente para uma propria para a entrega (VFX)
        }

        private void DeleteOrder(OrderData orderData)
        {
            //Debug.Log("Delete order");
            var a = ItemCardHolders.Find(x => x.id == orderData.Id);
            
            if (ReferenceEquals(a, null)) return;
            
            ItemCardHolders.Remove(a);
            Destroy(a.gameObject); // TODO: Change to Pool of objects
        }

        private void CreateOrder(OrderData orderData)
        {
            ItemCardHolder itemCardHolder = Instantiate(orderCardPrefab, orderCardHolder); // TODO: Change to Pool of objects
            itemCardHolder.SetItem(orderData.BaseItem, orderData.Id);
            ItemCardHolders.Add(itemCardHolder);
        }

        private void OnOrderListUpdate(List<OrderTimes> arg0)
        {
            for (int i = 0; i < ItemCardHolders.Count; i++)
            {
                ItemCardHolders[i].slider.value = arg0.Find(x=> x.Id == ItemCardHolders[i].id).TimeRemaining01;
            }
        }

        private void UpdateEnemyScore(float value)
        {
            enemyScoreSlider.value = value;
        }
        private void UpdatePlayerScore(float value)
        {
            playerScoreSlider.value = value;
        }

        private void UpdateTimer(int newValue)
        {
            int minutes = newValue / 60;
            int seconds = newValue % 60;
            string secondsString = seconds.ToString().PadLeft(2,'0');
            timerText.text = $"{minutes}:{secondsString}";
        }

        private void OnDisable()
        {
            _onCountdownTimerUpdated.OnEventRaised -= UpdateTimer;
            onPlayerScore.OnEventRaised -= UpdatePlayerScore;
            onEnemyScore.OnEventRaised -= UpdateEnemyScore;
            onOrderListUpdate.OnEventRaised -= OnOrderListUpdate;
            onCreateOrder.OnEventRaised -= CreateOrder;
            onDeleteOrder.OnEventRaised -= DeleteOrder;
            onDeliveryOrder.OnEventRaised -= DeleteOrder;
        }

        // private void Clear()
        // {
        //     foreach (var itemCardHolder in ItemCardHolders)
        //     {
        //         if (!ReferenceEquals(itemCardHolder, null))
        //         {
        //             Destroy(itemCardHolder);
        //         }
        //     }
        //     hudPanel.SetActive(true);
        //     ItemCardHolders.Clear();
        // }

        public void ClearCardHolders()
        {
            for(int i = ItemCardHolders.Count - 1; i >= 0; i--)
            {
                if (ItemCardHolders[i] != null)
                    Destroy(ItemCardHolders[i].gameObject);
			}
            // StartCoroutine(ClearCardHoldersList());
        }

  //       IEnumerator ClearCardHoldersList()
  //       {
  //           yield return new WaitForSeconds(0.1f);
		// 	ItemCardHolders.RemoveAll(s => s == null);
		// }
    }
}