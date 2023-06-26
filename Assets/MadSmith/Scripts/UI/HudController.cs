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
        public void AddOrder(ItemStruct[] itemStructs, int npcId, BoxColor boxColor) //TODO: add BoxColor
        {
            ItemCardHolder itemCardHolder = Instantiate(orderCardPrefab, orderCardHolder).GetComponent<ItemCardHolder>();
            itemCardHolder.SetItems(itemStructs,npcId, boxColor);
            ItemCardHolders.Add(itemCardHolder);
        }

        public void SetItemCollected(BaseItem item, int npcId )
        {
            
            var a =ItemCardHolders.Find(x => x.npcId == npcId);
            a.SetItemChecked(item);
        }

        public void RemoveOrder(int npcId)
        {
            var a = ItemCardHolders.Find(x => x.npcId == npcId);
            if (!ReferenceEquals(a, null))
            {
                Destroy(a.gameObject);
            }
        }
    }
}