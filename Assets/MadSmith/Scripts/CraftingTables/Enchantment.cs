using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder),typeof(Table))]
    public class Enchantment : CraftingTable
    {
        protected override void Awake()
        {
            base.Awake();
            CanAddPlayer = false;
        }
    }
}