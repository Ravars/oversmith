using Oversmith.Scripts.Interaction;
using UnityEngine;

namespace _Developers.Vitor
{
    public class InteractableHolder : MonoBehaviour
    {
        public bool hasTable;
        public bool hasDispenser;
        public bool hasCraftingTable;
        public bool hasInteractable;
        
        public Table table;
        public Dispenser dispenser;
        public CraftingTable craftingTable;
        public Interactable interactable;

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
        }

        public void SetStatusInteract(bool b)
        {
            if (visual != null && visualSelected != null)
            {
                visualSelected.SetActive(b);
                visual.SetActive(!b);
            }
        }
        
        



    }
}