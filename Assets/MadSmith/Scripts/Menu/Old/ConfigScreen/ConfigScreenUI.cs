using System;
using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

//namespace Oversmith.Scripts.Menu
//{
//    [RequireComponent(typeof(FadeUI))]
//    public class ConfigScreenUI : MonoBehaviour
//    {
//        public GameObject[] itemsList;
//        private MenuItemMenuUI itemUI;
//        [SerializeField] private MainMenuScript mainMenuScript;
//        private FadeUI _fade;
//        [Space(10)]

//        public PlayerInput input;      // Player Input component on player
//        public InputActionAsset inputActions; // Player input asset
//        private InputActionAsset Actions {
//            get {
//                if(inputActions != null) return inputActions;
//                else if(input != null) return input.actions;
//                return null;
//            }
//        }

//        private void Awake()
//        {
//            _fade = GetComponent<FadeUI>();
//        }

//        public UnityEvent OnExit;
        
//        // When enable, set effect of fade in
//        void OnEnable()
//        {
//            _fade.FadeIn(OnInitialize); // Call OnInitialize after fade in
//            if(!Actions) return;
//            Actions["Navigate"].performed += NavigateOptions;
//            Actions["Submit"].performed += OnApply;
//            Actions["Cancel"].performed += OnCancel;
//        }

//        void OnDisable()
//        {
//            if (Actions != null)
//            {
//                Actions["Navigate"].performed -= NavigateOptions;
//                Actions["Submit"].performed -= OnApply;
//                Actions["Cancel"].performed -= OnCancel;
//            }
//        }

//        // Set the first config element as selected
//        private void OnInitialize()
//        {
//            itemUI = itemsList[0].GetComponent<MenuItemMenuUI>();
//            itemUI.onSelect();
//        }

//        // Navigate through a list of configurations
//        // Allowing the player to select an item vertically (WIP)
//        // And horizontally modify its properties
//        public void NavigateOptions(InputAction.CallbackContext ctx)
//        {
//            Debug.Log("NavigateOptions");
//            float horizontal = ctx.ReadValue<Vector2>().x;
//            try
//            {
//                if (horizontal < 0) itemUI.onSwitchLeft();
//                else if (horizontal > 0) itemUI.onSwitchRight();
//            }
//            catch(MissingReferenceException)
//            {
//                // Reference to object that owns NavigateOptions
//                // Can be missed in Editor playmode
//                // If not unlinked appropriately to Input Action
//                ctx.action.performed -= NavigateOptions;
//            }
//        }

//        // Apply the configuration changes
//        // And return to main menu
//        public void OnApply()
//        {
//            int i;
//            // Apply language configuration.
//            switch(itemsList[0].GetComponent<MenuItemMenuUI>().Code)
//            {
//                case "en": i = 1; break;
//                case "pt-br": i = 0; break;
//                default: i = 1; break;
//            }
//            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
//            // Debug.Log(LocalizationSettings.AvailableLocales.Locales[i].ToString());
//            PlayerPrefsManager.SetString(PlayerPrefsKeys.Language,LocalizationSettings.AvailableLocales.Locales[i].ToString());
//            _fade.FadeOut(OnExit.Invoke);
//            // mainMenuScript.OpenMainScreen();
//        }
//        private void OnApply(InputAction.CallbackContext ctx)
//        {
//            try
//            {
//                OnApply();
//            }
//            catch(MissingReferenceException)
//            {
//                ctx.action.performed -= OnApply;
//            }
//        }

//        // Cancel modifications, keep configs as before
//        // And return to main menu
//        public void OnCancel()
//        {
//            // mainMenuScript.OpenMainScreen();
//            _fade.FadeOut(OnExit.Invoke);
//        }
//        private void OnCancel(InputAction.CallbackContext ctx)
//        {
//            try
//            {
//                OnCancel();
//            }
//            catch (MissingReferenceException)
//            {
//                ctx.action.performed -= OnCancel;
//            }
//        }
//    }
//}