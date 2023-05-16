using System.Collections.Generic;
using _Developers.Vitor;
using Oversmith.Scripts.Level;
using Oversmith.Scripts.Utils;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public class HudController : Singleton<HudController>
    {
        // public GameObject Card;
        public GameObject orderCardPrefab;
        public Transform orderCardHolder;
        public List<ItemCardHolder> ItemCardHolders;

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