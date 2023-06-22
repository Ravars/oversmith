using System;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.UI.Canvas
{
    public class UICredits : MonoBehaviour
    {
        public UnityAction OnCloseCredits;


        private void OnEnable()
        {
            // _inputReader.MenuCloseEvent += CloseCreditsScreen;
        }
        public void CloseCreditsScreen()
        {
            OnCloseCredits.Invoke();
        }
    }
}