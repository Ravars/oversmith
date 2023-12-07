﻿using System;
using System.Linq;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Items;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Interaction
{
    [RequireComponent(typeof(PlayerInteractableHandler))]
    public class PlayerInteractions : NetworkBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        private PlayerInteractableHandler _playerInteractableHandler;
        public Transform _itemTransform;
        // private BaseItem _baseItemHolding;
        public Item ItemScript { get; set; }
        public Transform itemHolder;
        
        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
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
            //Debug.Log("Interact");
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

        private void Grab()
        {
            //Debug.Log("Grab");
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                //Debug.Log("1");
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                //Debug.Log("2");
                if (interactable.hasTable && ItemScript?.baseItem.itemName != "Delivery Box")
                {
                    if (ItemScript == null && interactable.table.HasItem())
                    {
                        Tuple<Transform,Item> item = interactable.table.RemoveFromTable(itemHolder);
                        _itemTransform = item.Item1;
                        ItemScript = item.Item2;
                        ItemScript.PlaySound(SoundType.SoundOut);
                        _itemTransform.SetPositionAndRotation(itemHolder.position, Quaternion.identity);
                        return;
                    }
                    
                    if (ItemScript != null && interactable.table.CanSetItem(ItemScript))
                    {
                        interactable.table.PutOnTable(_itemTransform,ItemScript);
                        if (interactable.hasCraftingTable)
                            ItemScript.LastCraftingTable = interactable.craftingTable.type;
                        ItemScript.PlaySound(SoundType.SoundIn);
                        _itemTransform = null;
                        ItemScript = null;
                        return;
                    }

                    if (ItemScript != null && interactable.table.CanMergeItem(ItemScript))
                    {
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
                    //Debug.Log("3.1");
                    var baseItem = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo;
                    _itemTransform = Instantiate(baseItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                    ItemScript = _itemTransform.GetComponent<Item>();
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
                        Destroy(_itemTransform.gameObject);
                    }
                    _itemTransform = null;
                }
            }
        }
    }
}