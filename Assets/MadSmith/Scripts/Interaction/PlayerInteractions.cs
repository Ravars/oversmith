using System;
using System.Linq;
using MadSmith.Scripts.CraftingTables;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Systems;
using Mirror;
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
        public Item ItemScript { get; set; }
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
                GameObject itemTransform = Instantiate(baseItem.prefab, itemHolder.position, Quaternion.identity,this.itemHolder);
                _itemTransform = itemTransform.transform;
                Quaternion quaternion = itemTransform.transform.rotation;
                NetworkServer.Spawn(itemTransform);
                
                ItemScript = _itemTransform.GetComponent<Item>();
                RpcSyncItem(itemTransform,quaternion);
                
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
                if (interactable.hasTable && ItemScript?.baseItem.itemName != "Delivery Box")
                {
                    Debug.Log("2");
                    if (ItemScript == null && interactable.table.HasItem())
                    {
                        Debug.Log("2.1");
                        Tuple<Transform,Item> item = interactable.table.RemoveFromTable(itemHolder);
                        _itemTransform = item.Item1;
                        ItemScript = item.Item2;
                        ItemScript.PlaySound(SoundType.SoundOut);
                        _itemTransform.SetPositionAndRotation(itemHolder.position, Quaternion.identity);
                        return;
                    }
                    
                    Debug.Log("3");
                    if (ItemScript != null && interactable.table.CanSetItem(ItemScript))
                    {
                        Debug.Log("3.1");
                        // interactable.table.PutOnTable(_baseItemHoldingId);
                        if (!isLocalPlayer) return;
                        if (_itemTransform != null && interactable.table != null)
                        {
                            Debug.Log("not nulls");
                            _itemTransform.transform.SetParent(null);
                            var networkIdentity = _itemTransform.GetComponent<NetworkIdentity>();
                            if (networkIdentity != null)
                            {
                                Debug.Log("not null networkIdentity");
                                var tableNetworkIdentity = interactable.table.transform.GetComponent<NetworkIdentity>();
                                interactable.table.Batata(_baseItemHoldingId);
                                CmdBatata();
                                // CmdTransferItem(networkIdentity, tableNetworkIdentity);
                            }
                        }
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        
                        // interactable.table.PutOnTableCallCmd(_itemTransform);
                        if (interactable.hasCraftingTable)
                            ItemScript.LastCraftingTable = interactable.craftingTable.type;
                        ItemScript.PlaySound(SoundType.SoundIn);
                         CmdDestroyItem();
                        _itemTransform = null;
                         ItemScript = null;
                        return;
                    }

                    Debug.Log("4");
                    if (ItemScript != null && interactable.table.CanMergeItem(ItemScript))
                    {
                        Debug.Log("4.1");
                        interactable.table.MergeItem(ItemScript);
                        ItemScript.PlaySound(SoundType.CraftSound);
                        ItemScript = null;
                        if (_itemTransform != null)
                        {
                            Destroy(_itemTransform.gameObject);
                        }
                        _itemTransform = null;
                    }
                }
                //Debug.Log("3");
                if (interactable.hasDispenser && ItemScript == null)
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
                
                    if (ItemScript != null && interactable.hasDelivery)
                    {
                        if (interactable.deliveryPlace.DeliverItem(ItemScript.baseItem))
                        {
						    if (_itemTransform != null)
						    {
							    Destroy(_itemTransform.gameObject);
						    }
						    _itemTransform = null;
						    ItemScript = null;
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
                    ItemScript = null;
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
    }
}