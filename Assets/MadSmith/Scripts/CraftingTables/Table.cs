    using System;
using System.Linq;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Systems;
using MadSmith.Scripts.UI;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder))]
    public class Table : NetworkBehaviour
    {
        public Item ItemScript { get; private set; }
        // public bool isWorkTable = false;
        // public BaseItem BaseItem { get; private set; }
        [SyncVar] public Transform _itemTransform;
        // [SyncVar] private int _baseItemHoldingId;
        [SyncVar] public int num;
        
        private InteractableHolder _interactableHolder;

        [field: SerializeField] public Transform PointToSpawnItem { get; private set; }
        //ideia

        //[SerializeField] private AudioSource catchSound;

        // public Base

        private void Awake()
        {
            _interactableHolder = GetComponent<InteractableHolder>();
        }

        public bool HasItem()
        {
            return _itemTransform != null;
        }

        // public BaseItem GetItem()
        // {
        //     var tempItem = BaseItem;
        //     Destroy(_itemTransform.gameObject);
        //     BaseItem = null;
        //     _itemTransform = null;
        //     ItemScript = null;
        //     return tempItem;
        // }

        // public Tuple<Transform,Item> RemoveFromTable(Transform newParent)
        // {
        //     Transform tempTransform = _itemTransform;
        //     Item tempItem = ItemScript;
        //     _itemTransform.SetParent(newParent);
        //     _itemTransform = null;
        //     ItemScript = null;
        //     return new Tuple<Transform,Item>(tempTransform,tempItem);
        // }
        //
        // [Command]
        // public void CmdReceiveItem(NetworkIdentity itemToReceive)
        // {
        //     Debug.Log("CmdReceiveItem");
        //     if (itemToReceive != null)
        //     {
        //         Debug.Log("not null");
        //         itemToReceive.transform.SetParent(transform); // Define o objeto recebido como filho deste objeto no cenário
        //         RpcSyncItem(itemToReceive.gameObject);
        //     }
        // }
        // [ClientRpc]
        // void RpcSyncItem(GameObject item)
        // {
        //     // Realize qualquer outra ação necessária ao receber o item no cliente
        //     item.transform.SetParent(transform);
        // }
        // [ClientCallback]
        // public void PutOnTable(int id)
        // {
        //     Debug.Log("PutOnTable");
        //     SetBaseItem(id);
        //     this._baseItemHoldingId = id;
        //     Debug.Log("PutOnTable" + _baseItemHoldingId);
        //     // if (isServer) return;
        //     CmdSpawn();
        //     Debug.Log("After spawn");
        //     // //catchSound.Play();
        //     // ItemScript = itemScript;
        //     // _itemTransform = itemTransform;
        //     // _itemTransform.SetParent(pointToSpawnItem);
        //     // _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //     //
        //     // if (_interactableHolder.hasCraftingTable)
        //     // {
        //     //     _interactableHolder.craftingTable.ItemAddedToTable();
        //     // }
        // }
        // [Command]
        // private void SetBaseItem(int id)
        // {
        //     this._baseItemHoldingId = id;
        // }
        // [Command]
        // public void CmdSpawn()
        // {
        //     Debug.Log("Cmd");
        //     if (!isClient) return;
        //     Debug.Log("client");
        //     // Verifica se quem chamou o comando é o servidor
        //     if (isServer)
        //     {
        //         Debug.Log("server");
        //         var baseItem = BaseItemsManager.Instance.GetBaseItemById(_baseItemHoldingId);
        //         GameObject itemTransform = Instantiate(baseItem.prefab, pointToSpawnItem.position, Quaternion.identity,this.pointToSpawnItem);
        //         _itemTransform = itemTransform.transform;
        //         Quaternion quaternion = itemTransform.transform.rotation;
        //         NetworkServer.Spawn(itemTransform);
        //         Debug.Log("after spawn");
        //         
        //         ItemScript = _itemTransform.GetComponent<Item>();
        //         RpcSyncItem(itemTransform,quaternion);
        //     }
        // }
        // [ClientRpc]
        // void RpcSyncItem(GameObject item,Quaternion quaternion)
        // {
        //     Debug.Log("Item" + item.name);
        //     // Define o item como filho do jogador em cada cliente
        //     item.transform.SetParent(this.pointToSpawnItem);
        //     item.transform.SetLocalPositionAndRotation(Vector3.zero,quaternion);
        //     item.transform.localScale = Vector3.one;
        // }
        // public void PutOnTableCallCmd(Transform itemTransform)
        // {
        //     Debug.Log("call CMD");
        //     Debug.Log(isClient);
        //     Debug.Log(isServer);
        //     Debug.Log(isLocalPlayer);
        //     Debug.Log("server");
        //     // _itemTransform = itemTransform;
        //     // _itemTransform.SetParent(this.pointToSpawnItem);
        //     // _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //     if (_interactableHolder.hasCraftingTable)
        //     {
        //         _interactableHolder.craftingTable.ItemAddedToTable();
        //     }
        //
        //     CmdPutOnTable(itemTransform);
        // }
        // [Command]
        // public void CmdPutOnTable(Transform itemTransform)
        // {
        //     Debug.Log("CMD");
        //     if (!isClient) return;
        //     Debug.Log("client");
        //     if (isServer)
        //     {
        //         _itemTransform = itemTransform;
        //         _itemTransform.SetParent(this.pointToSpawnItem);
        //         _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //         RpcPutOnTable(itemTransform);
        //         
        //     }
        //     // Verifica se quem chamou o comando é o servidor
        //     // if (isServer)
        //     // {
        //     //     Debug.Log("server");
        //     //     _itemTransform = itemTransform;
        //     //     _itemTransform.SetParent(this.pointToSpawnItem);
        //     //     _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //     //     if (_interactableHolder.hasCraftingTable)
        //     //     {
        //     //         _interactableHolder.craftingTable.ItemAddedToTable();
        //     //     }
        //     //     RpcPutOnTable(itemTransform);
        //     // }
        //
        // }
        
        // [ClientRpc]
        // public void RpcPutOnTable(Transform itemTransform)
        // {
        //     Debug.Log("Rpc");
        //     _itemTransform = itemTransform;
        //     _itemTransform.SetParent(this.pointToSpawnItem);
        //     _itemTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        //     if (_interactableHolder.hasCraftingTable)
        //     {
        //         _interactableHolder.craftingTable.ItemAddedToTable();
        //     }
        // }

        public bool CanSetItem(Item newItem)
        {
            Debug.Log("CanSetItem");
            if (_itemTransform == null && !_interactableHolder.hasCraftingTable)
            {
                return true;
            }

            // _itemTransform.TryGetComponent(out Item itemScript);
            // var itemScript = _itemTransform.GetComponent<Item>();
            if (_interactableHolder.hasCraftingTable)
            {
                // Debug.Log("CanSetItem 1");
                // if (_itemTransform != null || _itemTransform.TryGetComponent(out Item itemScript))
                // {
                //     return false;
                // }
                // Debug.Log("CanSetItem 2");
                
                
                foreach (var process in newItem.baseItem.processes)
                {
                    Debug.Log("process: "+process.craftingTable);
                    if (process.craftingTable == _interactableHolder.craftingTable.type && process.craftingTable != CraftingTableType.Table)
                    {
                        return true;
                    }
                }
                return false;
            }
            Debug.Log("CanSetItem 3");

            return false;
        }

        public bool CanMergeItem(Item newItem)
        {
            if (_itemTransform == null) return false;
            
            var itemScript = _itemTransform.GetComponent<Item>();
            if (itemScript == null) return false;

            BaseItem[] itemsInUse = {
                newItem.baseItem,
                itemScript.baseItem
            };
            Process[] processes = newItem.baseItem.processes.Concat(itemScript.baseItem.processes).ToArray();
            foreach (var process in processes)
            {
                var canMerge = false;
                if (process.itemsNeeded.Length > 0)
                {
                    canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                }
                if (canMerge)
                {
                    return true;
                }
            }

            return false;
        }

        public void CraftItem(BaseItem newBaseItem)
        {
            DestroyItem();
            SpawnNewItem(newBaseItem);
            
        }

        public void MergeItem(Item newItem)
        {
            BaseItem[] itemsInUse = {
                newItem.baseItem,
                ItemScript.baseItem
            };
            Process[] processes = newItem.baseItem.processes.Concat(ItemScript.baseItem.processes).ToArray();
            
            foreach (var process in processes)
            {
                var canMerge = false;
                if (process.itemsNeeded.Length > 0)
                {
                    canMerge = process.itemsNeeded.All(itemNeeded => itemsInUse.Contains(itemNeeded));
                }
                if (canMerge)
                {
                    
                    AlertMessageManager.Instance.SpawnAlertMessage($"Item {process.itemGenerated.itemName} construído com sucesso.", MessageType.Normal);
                    DestroyItem();
                    // Destroy(newItem.transform);
                    SpawnNewItem(process.itemGenerated);
                    break;
                }
            }
        }

        private void DestroyItem()
        {
            if (_itemTransform != null)
            {
                Destroy(_itemTransform.gameObject);
            }
            _itemTransform = null;
            ItemScript = null;
        }
        private void SpawnNewItem(BaseItem newItem) // Provavelmente quebrado
        {
            //Debug.Log($"Spawn {newItem.name}");
            _itemTransform = Instantiate(newItem.prefab, PointToSpawnItem.position, PointToSpawnItem.rotation,
                PointToSpawnItem).transform;
            ItemScript = _itemTransform.GetComponent<Item>();
            if (AlertMessageManager.InstanceExists)
            {
                AlertMessageManager.Instance.SpawnAlertMessage($"Item {ItemScript.baseItem.itemName} construído com sucesso.", MessageType.Normal);
            }
        }

        // public void SetObject()
        // {
        //     
        // }
        //
        
        // public void Batata(int id)
        // {
        //     Debug.Log("Table Batata" + id);
        //     num = id;
        //     ServerClientNum(id);
        //     // CmdBatata();
        //     // ClientCallBatata();
        //     // ServerBatata();
        //     // ServerCall(id);
        // }
        //
        // [Server]
        // public void ServerClientNum(int id)
        // {
        //     Debug.Log("ServerClientNum " + id);
        //     this.num = id;
        //     ClientNum(id);
        // }
        //
        // [ClientRpc]
        // public void ClientNum(int id)
        // {
        //     Debug.Log("ClientNum: " + id);
        //     this.num = id;
        // }
        //
        // [Client]
        // public void ClientCallBatata()
        // {
        //     Debug.Log("ClientCallBatata");
        //     ServerBatata();
        //     CmdBatata();
        // }
        //
        // [Server]
        // public void ServerBatata()
        // {
        //     Debug.Log("Server batata");
        // }
        //
        // [Command]
        // public void CmdBatata()
        // {
        //     Debug.Log("Cmd Batata");
        // }
        // [Server]
        // public void ServerCall(int id)
        // {
        //     Debug.Log("server call");
        //     var prefab = BaseItemsManager.Instance.GetBaseItemById(id);
        //     var item = Instantiate(prefab.prefab, pointToSpawnItem.position, Quaternion.identity, pointToSpawnItem);
        //     Quaternion quaternion = item.transform.rotation;
        //     NetworkServer.Spawn(item);
        //
        //     RpcSetPosition(item,quaternion);
        // }
        //
        // [ClientRpc]
        // public void RpcSetPosition(GameObject item,Quaternion quaternion)
        // {
        //     Debug.Log("Item" + item.name);
        //     // Define o item como filho do jogador em cada cliente
        //     item.transform.SetParent(this.pointToSpawnItem);
        //     item.transform.SetLocalPositionAndRotation(Vector3.zero,quaternion);
        //     item.transform.localScale = Vector3.one;
        //     
        // }
    }
}