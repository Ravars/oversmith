using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Oversmith.Scripts.Menu
{
    public class MainScreenScript : MonoBehaviour
    {
        public PlayerInput input;
        public Button initBtt;
        private FadeUI fade;

        void OnEnable ()
        {
            input.SwitchCurrentActionMap("UI");
            fade = GetComponent<FadeUI>();
            fade.BeginFadeIn();
        }

        public void OnFadedIn()
        {
            initBtt.Select();
        }

        public void OnClickPlay()
        {
            input.DeactivateInput();
            fade = GetComponent<FadeUI>();
            fade.BeginFadeOut();
        }

        public void LoadNewGame()
        {
            SceneManager.LoadScene((int)LevelNames.Level01);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
