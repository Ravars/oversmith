using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.OLD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPlace : MonoBehaviour
{
    public OrdersManager OrdersManager;

    public bool DeliverItem(BaseItem item)
    {
        return OrdersManager.Instance.CheckOrder(item);
    }
}
