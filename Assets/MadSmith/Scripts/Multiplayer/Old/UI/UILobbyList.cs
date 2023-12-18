using MadSmith.Scripts.Input;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Multiplayer.Old.UI
{
    public class UILobbyList : MonoBehaviour
    {
        public UnityAction Closed;
        [SerializeField] private InputReader _inputReader;
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
    }
}