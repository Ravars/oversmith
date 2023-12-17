using System;
using MadSmith.Scripts.CraftingTables;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Systems;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Interaction
{
    public class CraftingInteractionHandler : NetworkBehaviour
    {
        private Table _table;
        private CraftingTable _craftingTable;

        [SyncVar] private int _numberOfPlayer;
        public int NumberOfPlayers
        {
            get => _numberOfPlayer;
            set => _numberOfPlayer = Math.Max(0, value);
        }
        private float _timeToPrepareItem;
        // [field: SerializeField] public float CurrentTimeToPrepareItem { get; private set; }
        [field: SerializeField] private float _speed;
        [SyncVar] public bool isRunning;
        [SyncVar] private float currentTime;
        public Slider Slider;

        [SyncVar] private GameObject _itemObject;
        private Item _itemScript;
        
        
        private void Awake()
        {
            _table = GetComponent<Table>();
            _craftingTable = GetComponent<CraftingTable>();
        }

        public void SetObject(GameObject itemObject)
        {
            _itemObject = itemObject;
            _itemScript = _itemObject.GetComponent<Item>();
        }

        public void Init(float timeToPrepareItem, float speed, int numberOfPlayer = 1)
        {
            Debug.Log("Init");
            if (!isRunning && _table.HasItem()) // has process
            {
                Debug.Log("1");
                    
                var itemScript = _table._itemTransform.GetComponent<Item>();
                if (itemScript.baseItem.processes.Length <= 0) return;
                Debug.Log("2");
                
                Process? a = null;
                foreach (var process in itemScript.baseItem.processes)
                {
                    if (process.craftingTable == _craftingTable.type)
                    {
                        a = process;
                        break;
                    }
                }
                Debug.Log("3");
                if (a == null)
                {
                    // error VFX
                    return;
                }
                Debug.Log("4");
                //Debug.Log($"Init {a.Value.itemGenerated.itemName}");
                
                _timeToPrepareItem = timeToPrepareItem;
                
                // Item itemScript = _table.item.
                // CurrentTimeToPrepareItem = _table.itemScript.currentProcessTimeNormalized;
                _numberOfPlayer = numberOfPlayer;
                Debug.Log("numberOfPlayer " + numberOfPlayer);
                _speed = speed;
                isRunning = true;
                enabled = true;
                _craftingTable.SetParticlesState(true);
                _craftingTable._audioSource.Play();
                _itemScript = itemScript;
                currentTime = 0;
                if(_itemScript.slider != null)
                {
                    _itemScript.slider.gameObject.SetActive(true);
                }
            }
        }
        
        
        private void Update()
        {
            // if (!isServer) return;
            if (!isRunning) return;

            if (isServer)
            {
                if (_table._itemTransform == null)
                {
                    isRunning = false;
                    enabled = false;
                    _craftingTable.SetParticlesState(false);
                    _craftingTable._audioSource.Stop();
                    return;
                };
                if (_itemScript.CurrentProcessTimeNormalized >= 1)
                {
                    
                    Debug.Log("Finished");
                    foreach (var process in _itemScript.baseItem.processes)
                    {
                        if (process.craftingTable == _craftingTable.type)
                        {
                            // _table.CraftItem(process.itemGenerated); // Nao sei se consigo enviar o Item aqui
                            isRunning = false;
                            enabled = false;
                            Debug.Log("Finished 2");
                            ServerSpawnItem(process.itemGenerated.id);
                            // _craftingTable.SetParticlesState(false);
                            // _craftingTable._audioSource.Stop();
                            // _craftingTable.ItemAddedToTable();
                            return;
                        }
                    }
                }
                _itemScript.CurrentProcessTimeNormalized += (Time.deltaTime * _numberOfPlayer * _speed) / _timeToPrepareItem;
            }

            if (isClient)
            {
                _itemScript.slider.value = _itemScript.CurrentProcessTimeNormalized;
            }
        }

        [Server]
        public void ServerSpawnItem(int id)
        {
            var baseItem = BaseItemsManager.Instance.GetBaseItemById(id);
            var item = Instantiate(baseItem.prefab, _table.PointToSpawnItem.position, Quaternion.identity, _table.PointToSpawnItem);
            Quaternion quaternion = item.transform.rotation;
            NetworkServer.Spawn(item);
            Debug.Log("Spawn new item");
            NetworkServer.Destroy(_table._itemTransform.gameObject);
            // _table._itemTransform = item.transform;
            ClientRpcSyncItem(item, quaternion);
        }

        [ClientRpc]
        public void ClientRpcSyncItem(GameObject item,Quaternion quaternion)
        {
            item.transform.SetParent(_table.PointToSpawnItem);
            item.transform.SetLocalPositionAndRotation(Vector3.zero, quaternion);
            _table._itemTransform = item.transform;
            Debug.Log("ClientRpcSyncItem");
        }
    }
}