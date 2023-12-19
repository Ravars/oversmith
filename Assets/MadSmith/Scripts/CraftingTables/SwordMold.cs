using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder),typeof(Table))]
    public class SwordMold : CraftingTable
    {
        protected override void Awake()
        {
            base.Awake();
            CanAddPlayer = true;
        }
        
        public override void ItemAddedToTable()
        {
            _craftingInteractionHandler.Init(timeToPrepareItem,1);   
        }
    }
}