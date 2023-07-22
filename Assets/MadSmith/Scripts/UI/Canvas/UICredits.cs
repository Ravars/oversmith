using System;
using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.UI.Canvas
{
    public class UICredits : MonoBehaviour
    {
        public UnityAction OnCloseCredits;
        [SerializeField] private InputReader _inputReader;

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseCreditsScreen;
        }
        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseCreditsScreen;
        }
        public void CloseCreditsScreen()
        {
            OnCloseCredits.Invoke();
        }
    }
}