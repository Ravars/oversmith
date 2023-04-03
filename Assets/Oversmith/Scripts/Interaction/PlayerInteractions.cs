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
        public Item ItemScript { get; set; }
        
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
                    if (ItemScript == null && interactable.table.HasItem())
                    {
                        Tuple<Transform,Item> item = interactable.table.RemoveFromTable(itemHolder);
                        _itemTransform = item.Item1;
                        ItemScript = item.Item2;
                        _itemTransform.SetLocalPositionAndRotation(itemHolder.localPosition, itemHolder.localRotation);
                        return;
                    }
                    
                    if (ItemScript != null && interactable.table.CanSetItem(ItemScript))
                    {
                        interactable.table.PutOnTable(_itemTransform,ItemScript);
                        _itemTransform = null;
                        ItemScript = null;
                        return;
                    }

                    if (ItemScript != null && interactable.table.CanMergeItem(ItemScript))
                    {
                        interactable.table.MergeItem(ItemScript);
                        ItemScript = null;
                        Destroy(_itemTransform.gameObject);
                        _itemTransform = null;
                    }
                }
                
                if(interactable.hasDispenser && ItemScript == null)
                {
                    var baseItem = _playerInteractableHandler.CurrentInteractable.InteractableHolder.dispenser.rawMaterialSo;
                    _itemTransform = Instantiate(baseItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                    ItemScript = _itemTransform.GetComponent<Item>();
                    return;
                }

                if (interactable.hasDelivery)
                {
                    if (interactable.delivery.CanSetItem() && ItemScript != null)
                    {
                        interactable.delivery.SetItem(_itemTransform,ItemScript);
                        _itemTransform = null;
                        ItemScript = null;
                        return;
                    }
                }
            }
        }
    }
}