﻿using Oversmith.Scripts;
using Test1.Scripts.Prototype;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Developers.Vitor
{
    public class PlayerInteractions : MonoBehaviour
    {
        private PlayerInteractableHandler _playerInteractableHandler;
        [SerializeField] private Transform itemHolder;
        private Transform _itemTransform;
        private BaseItem _holdingItem;
        
        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
            InputManager.Controls.Gameplay.Grab.performed += GrabOnPerformed;  
            InputManager.Controls.Gameplay.Interact.performed += InteractOnPerformed;  
        }
        
        private void InteractOnPerformed(InputAction.CallbackContext obj)
        {
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                if (interactable.hasCraftingTable)
                {
                    interactable.craftingTable.AddPlayer(this);
                }
                
                if (interactable.hasInteractable)
                {
                    interactable.interactable.Interact();
                }
            }
            
        }

        private void GrabOnPerformed(InputAction.CallbackContext obj)
        {
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                var interactable = _playerInteractableHandler.CurrentInteractable.InteractableHolder;
                if (interactable.hasTable)
                {
                    if (_holdingItem == null && interactable.table.HasItem())
                    {
                        _holdingItem = interactable.table.GetItem();
                        _itemTransform = Instantiate(_holdingItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                        return;
                    }
                    
                    if (_holdingItem != null && interactable.table.CanSetItem(_holdingItem))
                    {
                        interactable.table.SetItem(_holdingItem);
                        _holdingItem = null;
                        Destroy(_itemTransform.gameObject);
                        _itemTransform = null;
                        return;
                    }
                }
                
                if(interactable.hasDispenser && _holdingItem == null)
                {
                    _holdingItem = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo;
                    _itemTransform = Instantiate(_holdingItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                    return;
                }

                if (interactable.hasDelivery)
                {
                    if (interactable.delivery.CanSetItem() && _holdingItem != null)
                    {
                        interactable.delivery.SetItem(_holdingItem);
                        _holdingItem = null;
                        Destroy(_itemTransform.gameObject);
                        _itemTransform = null;
                        return;
                    }
                }

                
            }
            // _playerInteractableHandler.CurrentInteractable.Interactable 
        }
    }
}