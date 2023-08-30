using System;
using Mirror;
using TMPro;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts.UI
{
    public class MenuManagerNetwork : NetworkBehaviour
    {
        [SyncVar(hook = nameof(HandleStateChanged))]
        public MenuState menuState;
        public TextMeshProUGUI menuStateText;

        // public override void OnStartAuthority()
        // {
        //     base.OnStartAuthority();
        //     Debug.Log("On start authority");
        //     SetMainMenu();
        // }

        private void Start()
        {
            Debug.Log("On start authority");
            SetMainMenu();
        }

        private void HandleStateChanged(MenuState oldValue, MenuState newValue)
        {
            menuStateText.text = $"Old: {oldValue} - New: {newValue}";
        }

        [Command]
        public void CmdChangeState(MenuState newMenuState)
        {
            menuState = newMenuState;
        }

        public void SetMainMenu()
        {
            CmdChangeState(MenuState.MainMenu);
        }
        public void SetSettingsMenu()
        {
            CmdChangeState(MenuState.Settings);
        }
    }
}