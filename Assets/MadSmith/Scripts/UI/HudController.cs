using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Level;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class HudController : Singleton<HudController>
    {
        // public GameObject Card;
        public GameObject orderCardPrefab;
        public Transform orderCardHolder;
        public List<ItemCardHolder> ItemCardHolders;
        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;

        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += Clear;
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
            ItemCardHolders.Clear();
        }
        public void AddOrder(ItemStruct[] itemStructs, string wagonName)
        {
            ItemCardHolder itemCardHolder = Instantiate(orderCardPrefab, orderCardHolder).GetComponent<ItemCardHolder>();
            itemCardHolder.SetItems(itemStructs,wagonName);
            ItemCardHolders.Add(itemCardHolder);
        }

        public void SetItemCollected(BaseItem item, string wagonName )
        {
            var a =ItemCardHolders.Find(x => x.wagonName == wagonName);
            a.SetItemChecked(item);
        }

        public void RemoveOrder(string wagonName)
        {
            var a = ItemCardHolders.Find(x => x.wagonName == wagonName);
            Destroy(a.gameObject);
        }
    }
}