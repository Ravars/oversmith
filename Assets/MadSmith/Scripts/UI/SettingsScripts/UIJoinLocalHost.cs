using System;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.UI.Managers;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UIJoinLocalHost : MonoBehaviour
    {
        public UnityAction Closed;
        public UnityAction JoinButtonAction;
        private string ipAddress;
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private InputReader _inputReader;
        public void SetJoinLocalHost()
        {
            //Debug.Log("Set join host");
        }
        private void Awake()
        {
            _inputField.onValueChanged.AddListener(OnTextChange);
        }

        private void OnTextChange(string value)
        {
            ipAddress = value;
        }

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseScreen;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseScreen;
        }
        public void CloseScreen()
        {
            Closed?.Invoke();
        }
        public void Join()
        {
            UIMenuManager.Instance.Manager.networkAddress = ipAddress != string.Empty ? ipAddress : "localhost";
            JoinButtonAction?.Invoke();
        }
    }
}