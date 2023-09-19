using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.OLD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPlace : MonoBehaviour
{
    public OrdersManager OrdersManager;

    public void DeliverItem(BaseItem item)
    {
        OrdersManager.CheckOrder(item);
    }
}