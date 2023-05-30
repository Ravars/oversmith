using System.Collections.Generic;
using Oversmith.Scripts.Managers;
using Oversmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

namespace Oversmith.Scripts.Menu
{
    [AddComponentMenu("Menu/Canvas/Configuration Screen")]
    public class ConfigSreen : MenuCanvas
    {
        public List<MenuItemMenuUI> itemsList;
        private MenuItemMenuUI itemUI;
        public InputActionAsset inputActions;

        void Awake()
        {
            OnReturn.AddListener(RemoveListeners);
        }

        public override void Begin()
        {
            itemUI = itemsList[0];
            itemUI.onSelect();

            inputActions["Navigate"].performed += NavigateOptions;
            inputActions["Submit"].performed += OnApply;
            inputActions["Cancel"].performed += OnCancel;
        }

        private void RemoveListeners(System.Action _)
        {
            inputActions["Navigate"].performed -= NavigateOptions;
            inputActions["Submit"].performed -= OnApply;
            inputActions["Cancel"].performed -= OnCancel;
        }

        ///<summary>
        /// Navigate through a list of configurations.
        /// Allowing the player to select an item vertically (WIP).
        /// And horizontally modify their properties.
        ///</summary>
        public void NavigateOptions(InputAction.CallbackContext ctx)
        {
            try
            {
                if (!enabled) return;
                float horizontal = ctx.ReadValue<Vector2>().x;
                if (horizontal < 0) itemUI?.onSwitchLeft();
                else if (horizontal > 0) itemUI?.onSwitchRight();
            }
            catch(MissingReferenceException)
            {
                // Reference to object that owns NavigateOptions
                // Can be missed in Editor playmode
                // If not unlinked appropriately to Input Action
                ctx.action.performed -= NavigateOptions;
            }
        }

        ///<summary>
        /// Apply the configuration changes and return to main menu.
        ///</summary>
        public void OnApply()
        {
            if (!enabled) return;
            int i;
            // Apply language configuration.
            switch(itemsList[0].GetComponent<MenuItemMenuUI>().Code)
            {
                case "en": i = 1; break;
                case "pt-br": i = 0; break;
                default: i = 1; break;
            }
            OnReturn.Invoke(() => {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
                PlayerPrefsManager.SetString(PlayerPrefsKeys.Language,LocalizationSettings.AvailableLocales.Locales[i].ToString());
            });
        }
        private void OnApply(InputAction.CallbackContext ctx)
        {
            try
            {
                OnApply();
            }
            catch(MissingReferenceException)
            {
                ctx.action.performed -= OnApply;
            }
        }

        ///<summary>
        /// Cancel modifications, keep configs as before and return to main menu.
        ///</summary>
        public void OnCancel()
        {
            if (!enabled) return;
            OnReturn.Invoke(null);
        }
        private void OnCancel(InputAction.CallbackContext ctx)
        {
            try
            {
                OnCancel();
            }
            catch (MissingReferenceException)
            {
                ctx.action.performed -= OnCancel;
            }
        }
    }
}

