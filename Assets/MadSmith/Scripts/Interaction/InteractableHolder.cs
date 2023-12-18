using MadSmith.Scripts.CraftingTables;
using MadSmith.Scripts.Gameplay;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class InteractableHolder : NetworkBehaviour
    {
        public bool hasTable;
        public bool hasDispenser;
        public bool hasCraftingTable;
        public bool hasInteractable;
        public bool hasDelivery;
        public bool hasTrashCan;

        public Table table;
        public Dispenser dispenser;
        public CraftingTable craftingTable;
        public Interactable interactable;
        public TrashCan trashCan;
        public DeliveryPlace deliveryPlace;

        public GameObject visual;
        public GameObject visualSelected;

        private void Awake()
        {
            table = GetComponent<Table>();
            hasTable = table != null;

            dispenser = GetComponent<Dispenser>();
            hasDispenser = dispenser != null;

            craftingTable = GetComponent<CraftingTable>();
            hasCraftingTable = craftingTable != null;

            interactable = GetComponent<Interactable>();
            hasInteractable = interactable != null;

            trashCan = GetComponent<TrashCan>();
            hasTrashCan = trashCan != null;

            deliveryPlace = GetComponent<DeliveryPlace>();
            hasDelivery = deliveryPlace != null;
            
            SetStatusInteract(false);
        }

        public void SetStatusInteract(bool state)
        {
            if (visual != null && visualSelected != null)
            {
                visualSelected.SetActive(state);
                visual.SetActive(!state);
            }
        }
    }
}