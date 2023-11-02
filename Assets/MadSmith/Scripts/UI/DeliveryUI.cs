using System;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace MadSmith.Scripts.UI
{
    public class DeliveryUI : Singleton<DeliveryUI>
    {
        public TextMeshProUGUI[] texts;
        [SerializeField] private InputReader inputReader;

        private void OnEnable()
        {
            // inputReader.Menu
            
        }

        private void OnDisable()
        {
            
            
        }


        // public void SetItems(ItemStruct[] itemsToShow)
        // {
        //     Debug.Log(itemsToShow.Length);
        //     
        //     
        //     foreach (var text in texts)
        //     {
        //         text.text = string.Empty;
        //     }
        //     for (int i = 0; i < itemsToShow.Length; i++)
        //     {
        //         texts[i].text = itemsToShow[i].BaseItem.itemName + " x" + itemsToShow[i].Amount;
        //     }
        // }

        // public void SetItems(ItemDeliveryList list)
        // {
        //     foreach (var text in texts)
        //     {
        //         text.text = string.Empty;
        //     }
        //
        //     int i = 0;
        //     foreach (var item in list.Items)
        //     {
        //         texts[i].text = item.BaseItem.itemName + " x" + item.Amount;
        //         i++;
        //     }
        // }

        public void Close()
        {
            gameObject.SetActive(false);
        }

    }
}