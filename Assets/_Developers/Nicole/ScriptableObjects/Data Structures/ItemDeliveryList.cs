using MadSmith.Scripts.Level;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemDeliveryList", menuName = "Items/ItemDeliveryList")]
public class ItemDeliveryList : ScriptableObject
{
    public List<ItemStruct> Items;
}
