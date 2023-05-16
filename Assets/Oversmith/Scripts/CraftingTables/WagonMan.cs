using Oversmith.Scripts.Interaction;
using Oversmith.Scripts.Level;
using Oversmith.Scripts.UI;
using Test1.Scripts.Prototype;
using UnityEngine;

namespace _Developers.Vitor
{
    public class WagonMan : Interactable
    {
        public DeliveryBox deliveryBox;
        public DeliveryUI deliveryUI;
        public bool alreadyGet;
        
        public override void Interact(GameObject player)
        {
            deliveryUI.gameObject.SetActive(true);
            deliveryUI.SetItems(deliveryBox.requiredItems);

            if (PlayerInteractions.ItemScript == null && !deliveryBox.isActive)
            {
                var pI = player.GetComponent<PlayerInteractions>();
                deliveryBox.transform.SetParent(pI.itemHolder.transform, true);
                deliveryBox.transform.SetPositionAndRotation(pI.itemHolder.position, Quaternion.identity);
                PlayerInteractions.ItemScript = deliveryBox.GetComponent<Item>();
                deliveryBox.isActive = true;
                deliveryBox.visual.SetActive(true);

                if (!alreadyGet)
                {
                    HudController.Instance.AddOrder(deliveryBox.requiredItems.Items.ToArray(), gameObject.name);
                }
                
                alreadyGet = true;
            }

            // Spawnar caixa nas m�os do player
        }
    }
}