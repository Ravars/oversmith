using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder),typeof(Table))]
    public class WorkingTable : CraftingTable
    {
        public override void ItemAddedToTable()
        {
            // _craftingInteractionHandler.Init(timeToPrepareItem,1);   
        }
    }
}