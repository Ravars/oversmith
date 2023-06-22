using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace MadSmith.Scripts.UI
{
    public class UIGenericButton : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent buttonText;
        [SerializeField] private MultiInputButton button = default;
        private bool _isDefaultSelection = false;

        public UnityAction Clicked = default;

        private void OnDisable()
        {
            button.IsSelected = false;
            _isDefaultSelection = false;
        }

        public void SetButton(bool isSelect)
        {
            _isDefaultSelection = isSelect;
            if (isSelect)
            {
                button.UpdateSelected();
            }
        }
        public void SetButton(LocalizedString localizedString, bool isSelected)
        {
            buttonText.StringReference = localizedString;

            if (isSelected)
                SelectButton();
        }

        public void SetButton(string tableEntryReference, bool isSelected)
        {
            buttonText.StringReference.TableEntryReference = tableEntryReference;

            if (isSelected)
                SelectButton();
        }

        void SelectButton()
        {
            button.Select();
        }

        public void Click()
        {
            Clicked.Invoke();
        }
    }
}