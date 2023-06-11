using Oversmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Oversmith.Scripts.UI
{
    public enum PopupButtonType
    {
        Confirm,
        Cancel,
        Close,
        DoNothing,
    }
    public enum PopupType
    {
        Quit,
        NewGame,
        BackToMenu,
    }
    public class UIPopup : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent titleText;
        [SerializeField] private LocalizeStringEvent descriptionText;
        [SerializeField] private Button buttonClose;
        [SerializeField] private UIGenericButton popupButton1;
        [SerializeField] private UIGenericButton popupButton2;
        [SerializeField] private InputReader _inputReader = default;
        private PopupType _actualType;
        
        public event UnityAction<bool> ConfirmationResponseAction;
        public event UnityAction ClosePopupAction;

        public void SetPopup(PopupType popupType)
        {
            _actualType = popupType;
            bool isConfirmation = false;
            bool hasExitButton = false;

            titleText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_title";
            descriptionText.StringReference.TableEntryReference = _actualType.ToString() + "_Popup_Description";
            string tableEntryReferenceConfirm = PopupButtonType.Confirm + "_" + _actualType;
            string tableEntryReferenceCancel = PopupButtonType.Cancel + "_" + _actualType;
            switch (_actualType)
            {
                case PopupType.NewGame:
                case PopupType.BackToMenu:
                    isConfirmation = true;
                    
                    popupButton1.SetButton(tableEntryReferenceConfirm, true);
                    popupButton1.SetButton(tableEntryReferenceCancel,false);
                    hasExitButton = true;
                    break;
                case PopupType.Quit:
                    isConfirmation = true;
                    popupButton1.SetButton(tableEntryReferenceConfirm, true);
                    popupButton2.SetButton(tableEntryReferenceCancel, false);
                    hasExitButton = false;
                    break;
                default:
                    isConfirmation = false;
                    hasExitButton = false;
                    break;
            }
            if (isConfirmation) // needs two button : Is a decision 
            {
                popupButton1.gameObject.SetActive(true);
                popupButton2.gameObject.SetActive(true);

                popupButton2.Clicked += CancelButtonClicked;
                popupButton1.Clicked += ConfirmButtonClicked;
            }
            else // needs only one button : Is an information 
            {
                popupButton1.gameObject.SetActive(true);
                popupButton2.gameObject.SetActive(false);

                popupButton1.Clicked += ConfirmButtonClicked;
            }

            buttonClose.gameObject.SetActive(hasExitButton);

            if (hasExitButton) // can exit : Has to take the decision or aknowledge the information
            {
                _inputReader.MenuCloseEvent += ClosePopupButtonClicked;
            }
        }
        
        
        
        private void OnDisable()
        {
            popupButton2.Clicked -= CancelButtonClicked;
            popupButton1.Clicked -= ConfirmButtonClicked;
            _inputReader.MenuCloseEvent -= ClosePopupButtonClicked;
        }
        public void ClosePopupButtonClicked()
        {
            ClosePopupAction.Invoke();
        }

        void ConfirmButtonClicked()
        {
            ConfirmationResponseAction.Invoke(true);
        }

        void CancelButtonClicked()
        {
            ConfirmationResponseAction.Invoke(false);
        }
        
    }
}