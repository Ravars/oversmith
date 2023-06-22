using MadSmith.Scripts.Gameplay;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Level;
using MadSmith.Scripts.UI;
using Test1.Scripts.Prototype;
using UnityEngine;

namespace _Developers.Vitor
{
    public class WagonMan : Interactable
    {
        public DeliveryBox deliveryBox;
        // public DeliveryUI deliveryUI;
        public bool alreadyGet;
        
        public override void Interact(GameObject player)
        {
            // DeliveryUI.Instance.gameObject.SetActive(true);
            
            DeliveryUI.Instance.SetItems(deliveryBox.requiredItems);

            if (PlayerInteractions.ItemScript == null && !deliveryBox.isActive)
            {
                var pI = player.GetComponent<PlayerInteractions>();
                deliveryBox.transform.SetParent(pI.itemHolder.transform, true);
                deliveryBox.transform.SetPositionAndRotation(pI.itemHolder.position, Quaternion.identity);
                PlayerInteractions.ItemScript = deliveryBox.GetComponent<Item>();
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