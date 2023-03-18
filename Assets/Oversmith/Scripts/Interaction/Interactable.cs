using UnityEngine;

namespace _Developers.Vitor
{
    public class Interactable : MonoBehaviour
    {
        public bool hasTable;
        public bool hasDispenser;
        public bool hasCraftingTable;
        public bool hasInteractable;
        
        public Table table;
        public Dispenser dispenser;
        public CraftingTable craftingTable;
        private void Awake()
        {
            table = GetComponent<Table>();
            hasTable = table != null;

            dispenser = GetComponent<Dispenser>();
            hasDispenser = dispenser != null;

            craftingTable = GetComponent<CraftingTable>();
            hasCraftingTable = craftingTable != null;

        }

        public void SetStatusInteract(bool b)
        {
        }
    }
}