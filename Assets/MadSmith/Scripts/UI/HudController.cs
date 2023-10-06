using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.OLD;
using MadSmith.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class HudController : Singleton<HudController>
    {
        // public GameObject Card;
        public GameObject orderCardPrefab;
        public GameObject hudPanel;
        public Transform orderCardHolder;
        public List<ItemCardHolder> ItemCardHolders;

        [SerializeField] private Slider playerScoreSlider;
        [SerializeField] private Slider enemyScoreSlider;
        
        
        [SerializeField] private TextMeshProUGUI timerText;
        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;
        [SerializeField] private IntEventChannelSO _onCountdownTimerUpdated = default;
        [SerializeField] private FloatEventChannelSO onPlayerScore;
        [SerializeField] private FloatEventChannelSO onEnemyScore;

        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += Clear;
            _onCountdownTimerUpdated.OnEventRaised += UpdateTimer;
            onPlayerScore.OnEventRaised += UpdatePlayerScore;
            onEnemyScore.OnEventRaised += UpdateEnemyScore;
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
            _onSceneReady.OnEventRaised -= Clear;
        }

        private void Clear()
        {
            foreach (var itemCardHolder in ItemCardHolders)
            {
                if (!ReferenceEquals(itemCardHolder, null))
                {
                    Destroy(itemCardHolder);
                }
            }
            hudPanel.SetActive(true);
            ItemCardHolders.Clear();
        }
        public ItemCardHolder AddOrder(BaseItem itemStruct, int id)
        {
            ItemCardHolder itemCardHolder = Instantiate(orderCardPrefab, orderCardHolder).GetComponent<ItemCardHolder>();
            itemCardHolder.SetItem(itemStruct, id);
            ItemCardHolders.Add(itemCardHolder);

            return itemCardHolder;
        }

        public void RemoveOrder(int id)
        {
            var a = ItemCardHolders.Find(x => x.id == id);
            if (!ReferenceEquals(a, null))
            {
                Destroy(a.gameObject);
            }
        }

        public void ClearCardHolders()
        {
            for(int i = ItemCardHolders.Count - 1; i >= 0; i--)
            {
                if (ItemCardHolders[i] != null)
                    Destroy(ItemCardHolders[i].gameObject);
			}
            StartCoroutine(ClearCardHoldersList());
        }

        IEnumerator ClearCardHoldersList()
        {
            yield return new WaitForSeconds(0.1f);
			ItemCardHolders.RemoveAll(s => s == null);
		}
    }
}