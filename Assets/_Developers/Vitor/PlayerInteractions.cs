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
        
        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
            InputManager.Controls.Gameplay.Grab.performed += GrabOnPerformed;  
        }

        private void GrabOnPerformed(InputAction.CallbackContext obj)
        {
            Debug.Log("grab");
            if (_playerInteractableHandler.CurrentInteractable != null)
            {
                Debug.Log("has current");
                if (_playerInteractableHandler.CurrentInteractable.Interactable.hasDispenser)
                {
                    Debug.Log("instantiate");
                    Instantiate(_playerInteractableHandler.CurrentInteractable.Interactable.dispenser.rawMaterial.prefab,
                        itemHolder.position, Quaternion.identity,itemHolder);
                }
            }
            // _playerInteractableHandler.CurrentInteractable.Interactable 
        }
    }
}