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
            Debug.Log("Interact");
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                if (interactable.hasCraftingTable && interactable.craftingTable.CanAddPlayer)
                {
                    interactable.craftingTable.AddPlayer(this);
                }
                
                if (interactable.hasInteractable)
                {
                    interactable.interactable.Interact(this);
                }
            }
            
        }

        [Command]
        private void SetBaseItem(int id)
        {
            _baseItemHoldingId = id;
        }

        [Command]
        private void CmdSpawn()
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
                
                // this.ItemScript = _itemTransform.GetComponent<Item>();
                // Debug.Log((this.ItemScript == null) + " null");
                RpcSyncItem(itemGameObject,quaternion);
                
            }
        }
        [ClientRpc]
        void RpcSyncItem(GameObject item,Quaternion quaternion)
        {
            Debug.Log("Item" + item.name);
            // Define o item como filho do jogador em cada cliente
            item.transform.SetParent(this.itemHolder);
            item.transform.SetLocalPositionAndRotation(Vector3.zero,quaternion);
            item.transform.localScale = Vector3.one;
        }
        private void Grab()
        {
            if (!hasAuthority ) return;
            Debug.Log("Grab" + (_playerInteractableHandler.CurrentInteractable == null));
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                Debug.Log("1");
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                var itemScript = _itemTransform != null ? _itemTransform.GetComponent<Item>() : null;
                Debug.Log("itemScript " + (itemScript == null));
                if (interactable.hasTable && itemScript?.baseItem.itemName != "Delivery Box")
                {
                    Debug.Log("2");
                    if (itemScript == null && interactable.table.HasItem())
                    {
                        Debug.Log("2.1");
                        Tuple<Transform,Item> item = interactable.table.RemoveFromTable(itemHolder);
                        _itemTransform = item.Item1;
                        // itemScript = item.Item2; // Nao sei pra que serve
                        if (itemScript != null) itemScript.PlaySound(SoundType.SoundOut);
                        _itemTransform.SetPositionAndRotation(itemHolder.position, Quaternion.identity);
                        return;
                    }
                    
                    // Debug.Log("3: " + (ItemScript != null));
                    if (_itemTransform != null && interactable.table.CanSetItem(itemScript))
                    {
                        Debug.Log("3.1");
                        // interactable.table.PutOnTable(_baseItemHoldingId);
                        Debug.Log("3.2" + (_itemTransform != null) + " - " + (interactable.table != null));
                        if (!isLocalPlayer) return;
                        Debug.Log("3.2" + (_itemTransform != null) + " - " + (interactable.table != null));
                        if (_itemTransform != null && interactable.table != null)
                        {
                            Debug.Log("3.3");
                            Debug.Log("not nulls");
                            _itemTransform.transform.SetParent(null);
                            var networkIdentity = _itemTransform.GetComponent<NetworkIdentity>();
                            if (networkIdentity != null)
                            {
                                Debug.Log("not null networkIdentity");
                                MoveObjectToSceneObject(interactable.table);
                                // var tableNetworkIdentity = interactable.table.transform.GetComponent<NetworkIdentity>();
                                // interactable.table.Batata(_baseItemHoldingId);
                                // interactable.table.num = 5;
                                // CmdBatata();
                                // CmdTransferItem(networkIdentity, tableNetworkIdentity);
                            }
                        }
                        
                        // interactable.table.PutOnTableCallCmd(_itemTransform);
                        // if (interactable.hasCraftingTable)
                            // itemScript.LastCraftingTable = interactable.craftingTable.type;
                            // if (itemScript != null)
                            // {
                            //     itemScript.PlaySound(SoundType.SoundIn);
                            //     CmdDestroyItem();
                            //     _itemTransform = null;
                            // }
                        return;
                    }

                    Debug.Log("4");
                    if (itemScript != null && interactable.table.CanMergeItem(itemScript))
                    {
                        Debug.Log("4.1");
                        interactable.table.MergeItem(itemScript);
                        itemScript.PlaySound(SoundType.CraftSound);
                        itemScript = null;
                        if (_itemTransform != null)
                        {
                            Destroy(_itemTransform.gameObject);
                        }
                        _itemTransform = null;
                    }
                }
                //Debug.Log("3");
                if (interactable.hasDispenser && itemScript == null)
                {
                    var id = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo.id;
                    SetBaseItem(id);
                    CmdSpawn();
                    return;
                }

                // if (interactable.hasPallet && ItemScript?.baseItem.itemName == "Delivery Box")
                // {
                //     if (interactable.pallet.CanSetBox())
                //     {
                //         if (interactable.pallet.PutOnPallet(ItemScript.transform))
                //         {
                //             _itemTransform = null;
                //             ItemScript = null;
                //         }
                //         return;
                //     }
                // }

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
        private void CmdTransferItem(NetworkIdentity itemToTransfer, NetworkIdentity destination)
        {
            Debug.Log("Cmd transfer");
            Table transferScript = destination.GetComponent<Table>();
            if (itemToTransfer != null && destination != null)
            {
                Debug.Log("not null transfer" + transferScript.hasAuthority);
                
                transferScript.CmdReceiveItem(itemToTransfer);
                
            }
        }

        [Command]
        private void CmdBatata()
        {
            Debug.Log("Batata");
        }
        public void MoveObjectToSceneObject(Table sceneObject)
        {
            if (!hasAuthority || _itemTransform == null || sceneObject == null)
                return;

            CmdMoveObjectToScene(_itemTransform.gameObject, sceneObject);
        }
        [Command]
        void CmdMoveObjectToScene(GameObject objectToMove, Table sceneObject)
        {
            // Verifica se o jogador tem autoridade sobre o objeto
            // if (objectToMove.GetComponent<NetworkIdentity>().connectionToClient != connectionToClient)
            //     return;

            // Movimenta o objeto filho para o objeto do cenário
            objectToMove.transform.SetParent(sceneObject.PointToSpawnItem, true);
            objectToMove.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            sceneObject.num = 80;
            
            RpcSyncMovedObject(objectToMove, sceneObject);
        }
        [ClientRpc]
        void RpcSyncMovedObject(GameObject movedObject, Table sceneObject)
        {
            // Atualiza a posição do objeto movido em todos os clientes
            sceneObject.num = 80;
            movedObject.transform.SetParent(sceneObject.PointToSpawnItem, true);
            movedObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}