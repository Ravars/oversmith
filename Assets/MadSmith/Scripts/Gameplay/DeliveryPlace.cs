using System;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{ 
    [RequireComponent(typeof(AudioSource))]
    public class DeliveryPlace : NetworkBehaviour
    {
        public AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public bool DeliverItem(BaseItem item)
        {
            bool delivered = OrdersManager.Instance.CheckOrder(item);
            if (delivered)
            {
                audioSource.time = 0;
                audioSource.Play();    
            }
            return delivered;

        }
    }
}
