using MadSmith.Scripts.CraftingTables;
using MadSmith.Scripts.Gameplay;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    public class InteractableHolder : MonoBehaviour
    {
        public bool hasTable;
        public bool hasDispenser;
        public bool hasCraftingTable;
        public bool hasInteractable;
        public bool hasDelivery;
        public bool hasPallet;

        public Table table;
        public Dispenser dispenser;
        public CraftingTable craftingTable;
        public Interactable interactable;
        public DeliveryBox delivery;
        public Pallet pallet;

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

            delivery = GetComponent<DeliveryBox>();
            hasDelivery = delivery != null;

            pallet = GetComponent<Pallet>();
            hasPallet = pallet != null;
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