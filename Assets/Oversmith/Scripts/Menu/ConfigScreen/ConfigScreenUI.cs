using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

namespace Oversmith.Scripts.Menu
{
    public class ConfigScreenUI : MonoBehaviour
    {
        public GameObject[] itemsList;
        private MenuItemMenuUI itemUI;
        private FadeUI fade;
        private FadeUI Fade
        { 
            get
            {
                if (fade == null)
                    fade = GetComponent<FadeUI>();
                return fade;
            } 
        }
        [Space(10)]

        public PlayerInput input;

        public UnityEvent OnExit;
        
        // When enable, set effect of fade in
        void OnEnable()
        {
            Fade.FadeIn(OnInitialize); // Call OnInitialize after fade in
            input.actions["Navigate"].performed += NavigateOptions;
            input.actions["Submit"].performed += OnApply;
            input.actions["Cancel"].performed += OnCancel;
        }

        void OnDisable()
        {
            if (input != null)
            {
                input.actions["Navigate"].performed -= NavigateOptions;
                input.actions["Submit"].performed -= OnApply;
                input.actions["Cancel"].performed -= OnCancel;
            }
        }

        // Set the first config element as selected
        private void OnInitialize()
        {
            itemUI = itemsList[0].GetComponent<MenuItemMenuUI>();
            itemUI.onSelect();
        }

        // Navigate through a list of configurations
        // Allowing the player to select an item vertically (WIP)
        // And horizontally modify its properties
        public void NavigateOptions(InputAction.CallbackContext ctx)
        {
            float horizontal = ctx.ReadValue<Vector2>().x;
            try
            {
                if (horizontal < 0) itemUI.onSwitchLeft();
                else if (horizontal > 0) itemUI.onSwitchRight();
            }
            catch(MissingReferenceException)
            {
                // Reference to object that owns NavigateOptions
                // Can be missed in Editor playmode
                // If not unlinked appropriately to Input Action
                ctx.action.performed -= NavigateOptions;
            }
        }

        // Apply the configuration changes
        // And return to main menu
        public void OnApply()
        {
            int i;
            // Apply language configuration.
            switch(itemsList[0].GetComponent<MenuItemMenuUI>().Code)
            {
                case "en": i = 1; break;
                case "pt-br": i = 0; break;
                default: i = 1; break;
            }
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];

            Fade.FadeOut(OnExit.Invoke);
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

        // Cancel modifications, keep configs as before
        // And return to main menu
        public void OnCancel()
        {
            Fade.FadeOut(OnExit.Invoke);
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