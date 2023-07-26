using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MadSmith.Scripts.Menu.Old
{
    [AddComponentMenu("Menu/Canvas/Main Menu")]
    public class MainMenu : MenuCanvas
    {
        [SerializeField] private Button btnIntial;
        public UnityEvent OnPlay;
        public UnityEvent OnConfig;
        public UnityEvent OnQuit;

        public override void Begin()
        {
            btnIntial.Select();
        }

        #region INVOKES
        public void PlayInvoke () {
            if (enabled) OnPlay?.Invoke();
        }
        public void ConfigInvoke () {
            if (enabled) OnConfig?.Invoke();
        }
        public void QuitInvoke () {
            if (enabled) OnQuit?.Invoke();
        }
        #endregion
    }
}
