using System;
using UnityEditor;
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
            if (!isRunning && _table.HasItem() && _table.item.processes.Length > 0 ) // has process
            {
                _timeToPrepareItem = timeToPrepareItem;
                CurrentTimeToPrepareItem = 0;
                _numberOfPlayer = numberOfPlayer;
                _speed = speed;
                isRunning = true;
            }

        }
        
        private void Update()
        {
            if (!isRunning) return;
            if (CurrentTimeToPrepareItem >= _timeToPrepareItem)
            {
                foreach (var process in _table.item.processes)
                {
                    if (process.craftingTable == _craftingTable.type)
                    {
                        _table.SetItem(process.itemGenerated,true);
                        CurrentTimeToPrepareItem = 0;
                        isRunning = false;
                        enabled = false;
                        return;
                    }
                }
            }
            CurrentTimeToPrepareItem += Time.deltaTime * _numberOfPlayer * _speed;
        }
    }
}