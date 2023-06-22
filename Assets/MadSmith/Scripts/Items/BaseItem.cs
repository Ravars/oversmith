using System;
using UnityEngine;

namespace MadSmith.Scripts.Items
{
    [CreateAssetMenu(fileName = "newItem", menuName = "Items/Item")]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public GameObject prefab;
        public Process[] processes;
        public Texture image;
    }

    [Serializable]
    public struct Process
    {
        public CraftingTableType craftingTable;
        public BaseItem itemGenerated;
        public BaseItem[] itemsNeeded;
        public Process(CraftingTableType craftingTable, BaseItem itemGenerated, BaseItem[] itemsNeeded)
        {
            this.craftingTable = craftingTable;
            this.itemGenerated = itemGenerated;
            this.itemsNeeded = itemsNeeded;
        }
        
    }

    // public enum ItemType
    // {
    //     RawMaterial,
    //     WorkedRawMaterial,
    //     Material
    // }

    public enum CraftingTableType
    {
        Furnace,
        Carpentry,
        Sharpener,
        AxeMold,
        SwordMold,
        Anvil,
        Table,
        Enchantment,
    }
}