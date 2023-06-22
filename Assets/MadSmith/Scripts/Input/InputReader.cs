using System;
using MadSmith.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MadSmith.Scripts.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : DescriptionBaseSO, GameInput.IGameplayActions, GameInput.IDialoguesActions, GameInput.IMenusActions, GameInput.ICheatsActions
    {
        private GameInput _gameInput;
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
                _gameInput.Menus.SetCallbacks(this);
                _gameInput.Dialogues.SetCallbacks(this);
                _gameInput.Cheats.SetCallbacks(this);
            }
#if UNITY_EDITOR
            _gameInput.Cheats.Enable();
#endif
        }

        private void OnDisable()
        {
            DisableAllInput();
        }
        public void DisableAllInput()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Menus.Disable();
            _gameInput.Dialogues.Disable();
        }
        
        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;

        #region Public Functions
        public void EnableDialogueInput()
        {
            _gameInput.Menus.Enable();
            _gameInput.Gameplay.Disable();
            _gameInput.Dialogues.Enable();
        }

        public void EnableGameplayInput()
        {
            _gameInput.Menus.Disable();
            _gameInput.Dialogues.Disable();
            _gameInput.Gameplay.Enable();
        }
        public void EnableMenuInput()
        {
            _gameInput.Dialogues.Disable();
            _gameInput.Gameplay.Disable();

            _gameInput.Menus.Enable();
        }

        #endregion
        
        #region Unity Action events
        
        //Shared between Menus And Dialogues
        public event UnityAction MoveSelectionEvent = delegate { };
        
        //Menus
        public event UnityAction MenuPauseEvent = delegate { };
        public event UnityAction MenuUnpauseEvent = delegate { };
        public event UnityAction MenuCloseEvent = delegate { };
        public event UnityAction MenuMouseMoveEvent = delegate { };
        public event UnityAction MenuClickButtonEvent = delegate { };
        public event UnityAction<float> TabSwitched = delegate { };
        
        //Gameplay
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction MoveCanceledEvent = delegate { };
        public event UnityAction DashEvent = delegate { };
        public event UnityAction GrabEvent = delegate { };
        public event UnityAction InteractEvent = delegate { };
        
        #endregion
        
        #region GameInputCallbacks
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveEvent.Invoke(context.ReadValue<Vector2>());
            }else if (context.canceled)
            {
                MoveCanceledEvent.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                InteractEvent.Invoke();
            }
        }

        public void OnGrab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                GrabEvent.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuPauseEvent.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                DashEvent.Invoke();
            }
        }

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MoveSelectionEvent.Invoke();
            }
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuClickButtonEvent.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuCloseEvent.Invoke();
            }
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuMouseMoveEvent.Invoke();
            }
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuUnpauseEvent.Invoke();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                TabSwitched.Invoke(context.ReadValue<float>());
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }
        
        public void OnAdvanceDialogue(InputAction.CallbackContext context)
        {
        }

        public void OnOpenCheatMenu(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}