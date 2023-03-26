using System;
using _Developers.Vitor;
using Test1.Scripts.Prototype;
using UnityEngine;

namespace Oversmith.Scripts._Dev
{
    public class SpawnItem : MonoBehaviour
    {
        public BaseItem baseItem;
        private PlayerInteractableHandler _playerInteractableHandler;

        private void Start()
        {
            _playerInteractableHandler = GetComponent<PlayerInteractableHandler>();
        }

        [ContextMenu("Spawn item on current table")]
        public void SpawnItemOnCurrentTable()
        {
            if (_playerInteractableHandler.CurrentInteractable != null
                && _playerInteractableHandler.CurrentInteractable.InteractableHolder.hasTable
                && _playerInteractableHandler.CurrentInteractable.InteractableHolder.table.ItemScript == null)
            {
                _playerInteractableHandler.CurrentInteractable.InteractableHolder.table.CraftItem(baseItem);
            }
        }
    }
}