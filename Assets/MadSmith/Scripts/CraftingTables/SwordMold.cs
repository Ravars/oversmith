using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(InteractableHolder),typeof(Table))]
    public class SwordMold : CraftingTable
    {
        protected override void Awake()
        {
            base.Awake();
            CanAddPlayer = false;
        }
        
        public override void ItemAddedToTable()
        {
            _craftingInteractionHandler.Init(timeToPrepareItem,1);   
        }
    }
}