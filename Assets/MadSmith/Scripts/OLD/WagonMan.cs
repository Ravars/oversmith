using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.UI;
using UnityEngine;

namespace MadSmith.Scripts.OLD
{
    public class WagonMan : Interactable
    {
        public DeliveryBox deliveryBox;
        // public DeliveryUI deliveryUI;
        public bool alreadyGet;
        
        public override void Interact(PlayerInteractions playerInteractions)
        {
            // DeliveryUI.Instance.gameObject.SetActive(true);
            
            DeliveryUI.Instance.SetItems(deliveryBox.requiredItems);

            if (playerInteractions.ItemScript == null && !deliveryBox.isActive)
            {
                var pI = playerInteractions.GetComponent<PlayerInteractions>();
                deliveryBox.transform.SetParent(pI.itemHolder.transform, true);
                deliveryBox.transform.SetPositionAndRotation(pI.itemHolder.position, Quaternion.identity);
                playerInteractions.ItemScript = deliveryBox.GetComponent<Item>();
                pI._itemTransform = deliveryBox.transform;
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