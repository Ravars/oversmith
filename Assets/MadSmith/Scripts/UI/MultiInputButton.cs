using MadSmith.Scripts.Menu;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI
{
    public class MultiInputButton : Button
    {
        // TODO: Copiar do projeto da Unity UOP_Project
        [ReadOnly] public bool IsSelected;
        private MenuSelectionHandler _menuSelectionHandler;

        protected override void Awake()
        {
            _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            _menuSelectionHandler.HandleMouseEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            _menuSelectionHandler.HandleMouseExit(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            _menuSelectionHandler.UpdateSelection(gameObject);
            base.OnSelect(eventData);
        }
        public void UpdateSelected()
        {
            if (_menuSelectionHandler == null)
                _menuSelectionHandler = transform.root.gameObject.GetComponentInChildren<MenuSelectionHandler>();
		
            _menuSelectionHandler.UpdateSelection(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (_menuSelectionHandler.AllowsSubmit())
                base.OnSubmit(eventData);
        }
    }
}