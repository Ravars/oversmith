using System.Collections.Generic;
using MadSmith.Scripts.OLD;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemDeliveryList", menuName = "Items/ItemDeliveryList")]
public class ItemDeliveryList : ScriptableObject
{
    public List<ItemStruct> Items;
}
