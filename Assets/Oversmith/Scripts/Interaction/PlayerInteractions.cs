using System;
using Oversmith.Scripts;
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
        // private BaseItem _baseItemHolding;
        private Item _itemScript;
        
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
                    if (_itemScript == null && interactable.table.HasItem())
                    {
                        Tuple<Transform,Item> item = interactable.table.RemoveFromTable(itemHolder);
                        _itemTransform = item.Item1;
                        _itemScript = item.Item2;
                        _itemTransform.SetLocalPositionAndRotation(itemHolder.localPosition, itemHolder.localRotation);
                        // _itemTransform.localPosition = itemHolder.position;
                        // _holdingItem = interactable.table.GetItem();
                        // _itemTransform = Instantiate(_holdingItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                        return;
                    }
                    
                    if (_itemScript != null && interactable.table.CanSetItem(_itemScript))
                    {
                        interactable.table.PutOnTable(_itemTransform,_itemScript);
                        _itemTransform = null;
                        _itemScript = null;
                        // interactable.table.SetItem(_holdingItem);
                        // _holdingItem = null;
                        // Destroy(_itemTransform.gameObject);
                        // _itemTransform = null;
                        return;
                    }

                    if (_itemScript != null && interactable.table.CanMergeItem(_itemScript))
                    {
                        interactable.table.MergeItem(_itemScript);
                        _itemScript = null;
                        Destroy(_itemTransform.gameObject);
                        _itemTransform = null;
                    }
                }
                
                if(interactable.hasDispenser && _itemScript == null)
                {
                    var baseItem = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo;
                    _itemTransform = Instantiate(baseItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                    _itemScript = _itemTransform.GetComponent<Item>();
                    return;
                }

                if (interactable.hasDelivery)
                {
                    if (interactable.delivery.CanSetItem() && _itemScript != null)
                    {
                        interactable.delivery.SetItem(_itemTransform,_itemScript);
                        _itemTransform = null;
                        _itemScript = null;
                        return;
                    }
                }

                
            }
            // _playerInteractableHandler.CurrentInteractable.Interactable 
        }
    }
}