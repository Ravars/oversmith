using System;
using UnityEngine;

namespace _Developers.Vitor
{
    [CreateAssetMenu(fileName = "newBaseItem", menuName = "Items/BaseItem")]
    public class BaseItem : ScriptableObject
    {
        public string itemName;
        public ItemType itemType;
        public GameObject prefab;
        public Process[] Processes;
    }

    [Serializable]
    public struct Process
    {
        public CraftingTableType craftingTable;
        public BaseItem itemGenerated;

        public Process(CraftingTableType craftingTable, BaseItem itemGenerated)
        {
            this.craftingTable = craftingTable;
            this.itemGenerated = itemGenerated;
        }
    }

    public enum ItemType
    {
        RawMaterial,
        WorkedRawMaterial,
        Material
    }

    public enum CraftingTableType
    {
        Furnace,
        Carpentry,
        SwordMold,
        Anvil,
    }
}