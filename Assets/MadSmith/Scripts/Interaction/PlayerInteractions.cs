﻿using System;
using System.Linq;
using MadSmith.Scripts.CraftingTables;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Systems;
using Mirror;
using Unity.Mathematics;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [RequireComponent(typeof(PlayerInteractableHandler))]
    public class PlayerInteractions : NetworkBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        private PlayerInteractableHandler _playerInteractableHandler;
        [SyncVar] public Transform _itemTransform;
        [SyncVar] private int _baseItemHoldingId;
        // public Item ItemScript;
        public Transform itemHolder;
        // public BaseItem baseItemTest;
        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
            // networkTransformChild = GetComponent<NetworkTransformChild>();
        }

        private void OnEnable()
        {
            _inputReader.GrabEvent += Grab; 
            _inputReader.InteractEvent += Interact;
        }

        private void OnDisable()
        {
            _inputReader.GrabEvent -= Grab; 
            _inputReader.InteractEvent -= Interact;
        }
        private void Interact()
        {
            Debug.Log("_playerInteractableHandler.CurrentInteractable");
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                Debug.Log("2");
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                if (interactable.hasCraftingTable && interactable.craftingTable.CanAddPlayer)
                {
                    Debug.Log("3");
                    // interactable.craftingTable.AddPlayer();
                    CmdAddPlayerCraftingTable(interactable);
                }
                
                    Debug.Log("4");
                if (interactable.hasInteractable)
                {
                    interactable.interactable.Interact(this);
                }
            }
            
        }

        [Command]
        public void CmdAddPlayerCraftingTable(InteractableHolder interactableHolder)
        {
            // interactableHolder.craftingTable._craftingInteractionHandler.SetObject(interactableHolder.table._itemTransform.GetComponent<Item>().gameObject);
            // interactableHolder.craftingTable._craftingInteractionHandler.Slider = .slider;
            interactableHolder.craftingTable.AddPlayer();
            RpcAddPlayerCraftingTable(interactableHolder);
        }

        [ClientRpc]
        public void RpcAddPlayerCraftingTable(InteractableHolder interactableHolder)
        {
            // interactableHolder.craftingTable._craftingInteractionHandler.SetObject(interactableHolder.table._itemTransform.GetComponent<Item>().gameObject);
            // interactableHolder.craftingTable._craftingInteractionHandler.Slider = interactableHolder.table._itemTransform.GetComponent<Item>().slider;
            interactableHolder.craftingTable.AddPlayer();
        }

        [Command]
        private void SetBaseItem(int id)
        {
            _baseItemHoldingId = id;
        }

        [Command]
        private void CmdSpawnItemOnPlayer()
        {
            // Debug.Log("CMD Nulo" + (_baseItemHolding.prefab == null));
            if (!isClient) return;

            // Verifica se quem chamou o comando é o servidor
            if (isServer)
            {
                var baseItem = BaseItemsManager.Instance.GetBaseItemById(_baseItemHoldingId);
                GameObject itemGameObject = Instantiate(baseItem.prefab, itemHolder.position, Quaternion.identity,this.itemHolder);
                _itemTransform = itemGameObject.transform;
                Quaternion quaternion = itemGameObject.transform.rotation;
                NetworkServer.Spawn(itemGameObject);
                RpcSyncItemOnPlayer(itemGameObject,quaternion);
            }
        }
        [ClientRpc]
        void RpcSyncItemOnPlayer(GameObject item,Quaternion quaternion)
        {
            //Debug.Log("Item" + item.name);
            // Define o item como filho do jogador em cada cliente
            item.transform.SetParent(this.itemHolder);
            item.transform.SetLocalPositionAndRotation(Vector3.zero,quaternion);
            item.transform.localScale = Vector3.one;
        }
        [Command]
        private void CmdSpawnItemOnTable(Table table, int itemId)
        {
            // Debug.Log("CMD Nulo" + (_baseItemHolding.prefab == null));
            if (!isClient) return;

            // Verifica se quem chamou o comando é o servidor
            if (isServer)
            {
                var baseItem = BaseItemsManager.Instance.GetBaseItemById(itemId);
                GameObject itemGameObject = Instantiate(baseItem.prefab, table.PointToSpawnItem.position, Quaternion.identity, table.PointToSpawnItem);
                Quaternion quaternion = itemGameObject.transform.rotation;
                NetworkServer.Spawn(itemGameObject);
                RpcSyncItemOnTable(table, itemGameObject,quaternion);
            }
        }
        [ClientRpc]
        void RpcSyncItemOnTable(Table table, GameObject item,Quaternion quaternion)
        {
            //Debug.Log("Item: " + table.PointToSpawnItem.name);
            // Define o item como filho do jogador em cada cliente
            item.transform.SetParent(table.PointToSpawnItem);
            item.transform.SetLocalPositionAndRotation(Vector3.zero,quaternion);
            item.transform.localScale = Vector3.one;
            table.SetObject(item);
        }
        private void Grab()
        {
            if (!hasAuthority ) return;
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                Debug.Log("1");
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                var itemScript = _itemTransform != null ? _itemTransform.GetComponent<Item>() : null;
                if (interactable.hasTable && itemScript?.baseItem.itemName != "Delivery Box")
                {
                    Debug.Log("2");
                    // Grab from table - OK
                    if (itemScript == null && interactable.table.HasItem())
                    {
                        //Debug.Log("2.1");
                        SetBaseItem(interactable.table.ItemScript.baseItem.id);
                        GetObjectFromTable(interactable.table);
                        return;
                    }
                    
                    Debug.Log("3");
                    // Put On Table - Ok
                    if (_itemTransform != null && interactable.table.CanSetItem(itemScript))
                    {
                        Debug.Log("1");
                        // if (!isLocalPlayer) return;
                        Debug.Log("2");
                        if (_itemTransform != null && interactable.table != null)
                        {
                            Debug.Log("3");
                            _itemTransform.transform.SetParent(null);
                            var networkIdentity = _itemTransform.GetComponent<NetworkIdentity>();
                            if (networkIdentity != null)
                            {
                                Debug.Log("4");
                                if (!interactable.table.CanMergeItem(itemScript))
                                {
                                    MoveObjectToSceneObject(interactable.table, _baseItemHoldingId);
                                    _itemTransform = null;
                                    _baseItemHoldingId = -1;
                                    return;
                                }
                            }
                        }
                    }

                    Debug.Log("4");
                    // Merge item - Ok
                    if (itemScript != null && (interactable.hasCraftingTable) && interactable.table.CanMergeItem(itemScript))
                    {
                        Debug.Log("4.1");
                        var itemToSpawn = interactable.table.MergeItem(itemScript);
                        // Destruir o item na mao do player
                        CmdDestroyItem();
                        _itemTransform = null;
                        // Destruir o item na mesa
                        CmdDestroyItem(interactable.table.ItemScript.gameObject);
                        // interactable.table._itemTransform = null;
                        // Spawnar item
                        // Mover o item para a mesa
                        CmdSpawnItemOnTable(interactable.table, itemToSpawn);
                        return;
                    }
                }
                //Debug.Log("5");
                //Grab from Dispenser
                if (interactable.hasDispenser && itemScript == null)
                {
                    var id = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo.id;
                    SetBaseItem(id);
                    CmdSpawnItemOnPlayer();
                    return;
                }
                if (interactable.hasDelivery)
                {
                    // if (ItemScript == null)
                    // {
                    //     interactable.transform.SetParent(transform);
                    //     _itemTransform = interactable.transform;
                    //     ItemScript = interactable.delivery.GetComponent<Item>();
                    //     _itemTransform.SetPositionAndRotation(itemHolder.position, Quaternion.identity);
                    //     // interactable.delivery.SetTrigger(false);
                    //     _playerInteractableHandler.ClearList();
                    // }
                
                    if (itemScript != null && interactable.hasDelivery)
                    {
                        if (interactable.deliveryPlace.DeliverItem(itemScript.baseItem))
                        {

                            //Debug.Log("item delivered");
                            itemScript = null;
                            if (_itemTransform != null)
                            {
                                CmdDestroyItem();
                            }
                            _itemTransform = null;
                            return;
						    if (_itemTransform != null)
						    {
							    Destroy(_itemTransform.gameObject);
						    }
						    _itemTransform = null;
						    itemScript = null;
                            return;
                        }
                        else
                        {
                            //Não foi permitido entregar
                        }
                    }
                
                }
                // Trash can
                if (interactable.hasTrashCan)
                {
                    interactable.trashCan.DestroyItem();
                    itemScript = null;
                    if (_itemTransform != null)
                    {
                        CmdDestroyItem();
                    }
                    _itemTransform = null;
                }
            }
        }

        [Command]
        private void CmdDestroyItem()
        {
            NetworkServer.Destroy(_itemTransform.gameObject);
        }
        [Command]
        private void CmdDestroyItem(GameObject itemToDestroy)
        {
            NetworkServer.Destroy(itemToDestroy);
        }

        // [Command]
        // private void CmdTransferItem(NetworkIdentity itemToTransfer, NetworkIdentity destination)
        // {
        //     Debug.Log("Cmd transfer");
        //     Table transferScript = destination.GetComponent<Table>();
        //     if (itemToTransfer != null && destination != null)
        //     {
        //         Debug.Log("not null transfer" + transferScript.hasAuthority);
        //         
        //         transferScript.CmdReceiveItem(itemToTransfer);
        //         
        //     }
        // }

        #region Put On Table
        public void MoveObjectToSceneObject(Table sceneObject, int itemId)
        {
            if (!hasAuthority || _itemTransform == null || sceneObject == null)
                return;

            CmdMoveObjectToScene(_itemTransform.gameObject, sceneObject, itemId);
        }
        [Command]
        void CmdMoveObjectToScene(GameObject objectToMove, Table table, int itemId)
        {
            // Verifica se o jogador tem autoridade sobre o objeto
            // if (objectToMove.GetComponent<NetworkIdentity>().connectionToClient != connectionToClient)
            //     return;

            // Movimenta o objeto filho para o objeto do cenário
            objectToMove.transform.SetParent(table.PointToSpawnItem, true);
            objectToMove.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            table.SetObject(objectToMove);
            // sceneObject._itemTransform = objectToMove.transform;
            RpcSyncMovedObject(objectToMove, table,itemId);
        }
        [ClientRpc]
        void RpcSyncMovedObject(GameObject movedObject, Table table, int itemId)
        {
            // Atualiza a posição do objeto movido em todos os clientes
            table.SetObject(movedObject);
            // sceneObject._itemTransform = movedObject.transform;
            movedObject.transform.SetParent(table.PointToSpawnItem, true);
            movedObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        #endregion
        
        #region Grab from table
        public void GetObjectFromTable(Table table)
        {
            // if (!hasAuthority || _itemTransform == null || objectToMove == null)
            //     return;
            //Debug.Log("GetObjectFromTable");
            CmdGetObjectFromTable(table);
        }
        [Command]
        void CmdGetObjectFromTable(Table table)
        {
            // Verifica se o jogador tem autoridade sobre o objeto
            // if (objectToMove.GetComponent<NetworkIdentity>().connectionToClient != connectionToClient)
            //     return;
            // Debug.Log("CmdGetObjectFromTable");
            // Movimenta o objeto filho para o objeto do cenário
            _itemTransform = table.ItemScript.transform;
            _itemTransform.transform.SetParent(this.itemHolder, true);
            _itemTransform.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            // sceneObject._itemTransform = objectToMove.transform;
            RpcGetObjectFromTable(table);
            // table._itemTransform = null;
        }
        [ClientRpc]
        void RpcGetObjectFromTable(Table table)
        {
            // Atualiza a posição do objeto movido em todos os clientes
            _itemTransform = table.ItemScript.transform;
            _itemTransform.transform.SetParent(this.itemHolder, true);
            _itemTransform.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            table.SetObject(null);
            // table._itemTransform = null;
        }

        #endregion
    }
}