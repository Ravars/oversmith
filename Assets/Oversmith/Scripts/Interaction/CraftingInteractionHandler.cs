using System;
using UnityEngine;

namespace _Developers.Vitor
{
    public class CraftingInteractionHandler : MonoBehaviour
    {
        private Table _table;
        private CraftingTable _craftingTable;

        private int _numberOfPlayer;
        public int NumberOfPlayers
        {
            get => _numberOfPlayer;
            set => _numberOfPlayer = Math.Max(0, value);
        }
        private float _timeToPrepareItem;
        [field: SerializeField] public float CurrentTimeToPrepareItem { get; private set; }
        [field: SerializeField] private float _speed;
        [field: SerializeField]  public bool isRunning { get; private set; }

        private void Awake()
        {
            _table = GetComponent<Table>();
            _craftingTable = GetComponent<CraftingTable>();
        }

        public void Init(float timeToPrepareItem, float speed, int numberOfPlayer = 1)
        {
            if (!isRunning && _table.HasItem() && _table.item.processes.Length > 0) // has process
            {
                Process? a = null;
                foreach (var process in _table.item.processes)
                {
                    if (process.craftingTable == _craftingTable.type)
                    {
                        a = process;
                        break;
                    }
                }
                
                if (a == null)
                {
                    // error VFX
                    return;
                }
                
                _timeToPrepareItem = timeToPrepareItem;
                
                // Item itemScript = _table.item.
                CurrentTimeToPrepareItem = _table.itemScript.currentProcessTimeNormalized;
                _numberOfPlayer = numberOfPlayer;
                _speed = speed;
                isRunning = true;
                enabled = true;
            }
        }
        
        
        private void Update()
        {
            if (!isRunning) return;
            if (_table.item == null)
            {
                isRunning = false;
                enabled = false;
                return;
            }; // Change to event
            if (_table.itemScript.currentProcessTimeNormalized >= _timeToPrepareItem)
            {
                foreach (var process in _table.item.processes)
                {
                    if (process.craftingTable == _craftingTable.type)
                    {
                        _table.SetItem(process.itemGenerated,true);
                        // _table.item.currentProcessTimeNormalized = 0;
                        isRunning = false;
                        enabled = false;
                        return;
                    }
                }
            }
            _table.itemScript.currentProcessTimeNormalized += Time.deltaTime * _numberOfPlayer * _speed;
        }
    }
}