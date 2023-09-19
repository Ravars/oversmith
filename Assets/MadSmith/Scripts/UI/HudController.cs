using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.OLD;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class HudController : Singleton<HudController>
    {
        // public GameObject Card;
        public GameObject orderCardPrefab;
        public GameObject hudPanel;
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
    }
}