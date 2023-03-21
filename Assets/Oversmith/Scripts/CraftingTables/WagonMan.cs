using Oversmith.Scripts.Interaction;
using Oversmith.Scripts.Level;
using Oversmith.Scripts.UI;
using UnityEngine;

namespace _Developers.Vitor
{
    public class WagonMan : Interactable
    {
        public Delivery delivery;
        public DeliveryUI deliveryUI;
        
        public override void Interact()
        {
            Debug.Log("wagon man");
            deliveryUI.gameObject.SetActive(true);
            deliveryUI.SetItems(delivery.requiredItems);
        }
    }
}