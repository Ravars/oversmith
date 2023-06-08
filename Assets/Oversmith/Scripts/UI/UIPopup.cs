using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Oversmith.Scripts.UI
{
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
        private PopupType _actualType;
        [SerializeField] private UIGenericButton popupButton1;
        [SerializeField] private UIGenericButton popupButton2;
        
        // public event UnityAction<bool> ConfirmationResponseAction;
        // public event UnityAction ClosePopupAction;
        
        private void OnDisable()
        {
            // popupButton2.Clicked -= CancelButtonClicked;
            // popupButton1.Clicked -= ConfirmButtonClicked;
            // _inputReader.MenuCloseEvent -= ClosePopupButtonClicked;
        }
    }
}