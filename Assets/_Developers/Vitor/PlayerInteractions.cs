using System;
using Oversmith.Scripts;
using Test1.Scripts.Prototype;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Developers.Vitor
{
    public class PlayerInteractions : MonoBehaviour
    {
        private PlayerInteractableHandler _playerInteractableHandler;
        // [SerializeField] private Vector3 positionToSpawnItems;
        [SerializeField] private Transform itemHolder;
        private Transform _itemTransform;
        private BaseItem _holdingItem;
        
        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
            InputManager.Controls.Gameplay.Grab.performed += GrabOnPerformed;  
        }

        private void GrabOnPerformed(InputAction.CallbackContext obj)
        {
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                var Interactable = _playerInteractableHandler.CurrentInteractable.Interactable;
                if (Interactable.hasTable)
                {
                    if (_holdingItem == null && Interactable.table.HasItem())
                    {
                        _holdingItem = Interactable.table.GetItem();
                        _itemTransform = Instantiate(_holdingItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                        return;
                    }
                    
                    if (_holdingItem != null && Interactable.table.CanSetItem(_holdingItem))
                    {
                        Interactable.table.SetItem(_holdingItem);
                        _holdingItem = null;
                        Destroy(_itemTransform.gameObject);
                        _itemTransform = null;
                        return;
                    }
                }
                
                if(Interactable.hasDispenser && _holdingItem == null)
                {
                    _holdingItem = _playerInteractableHandler.CurrentInteractable.Interactable.dispenser.rawMaterial;
                    _itemTransform = Instantiate(_holdingItem.prefab, itemHolder.position, Quaternion.identity,itemHolder).transform;
                    return;
                }
            }
            // _playerInteractableHandler.CurrentInteractable.Interactable 
        }
    }
}