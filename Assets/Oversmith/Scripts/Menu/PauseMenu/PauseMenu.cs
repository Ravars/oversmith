using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        // Screens the pause menu is composed of
        [Header("Screens")]
        [SerializeField] private GameObject MainScr;    // Main screen: list of buttons
        [SerializeField] private GameObject ConfigScr;  // Configuration screen

        private InputActionAsset actions;

        [Space(20)] public UnityEvent OnResume; // Dispatch this event when player returns to game

        public InputActionAsset Actions 
        {
            get => actions;
            set {
                actions = value;
                ConfigScr.GetComponent<ConfigScreenUI>().inputActions = actions;
            }
        }

        public void Awake() // Pause menu begins disabled
        {
            GameObject go = GameObject.Find("EventSystem");
            Actions = go.GetComponent<InputSystemUIInputModule>()?.actionsAsset;
            gameObject.SetActive(false);
        }

        public void OnResumeGame()
        {
            UnableAll();
            gameObject.SetActive(false); // Disable everything

            OnResume.Invoke();           // Dispatch event
        }

        public void OnReturnScr()
        {
            UnableAll();
            MainScr.SetActive(true);
        }

        public void OnOpenConfig()
        {
            UnableAll();
            ConfigScr.SetActive(true);
        }
        
        public void OnReturnMain()
        {
            SceneManager.LoadScene(LevelNames.MainMenu.ToString());
        }

        public void OnQuit()
        {
            Debug.Log("Application quit.");
            Application.Quit();
        }

        public void OnEnable()
        {
            UnableAll();
            transform.GetChild(0).gameObject.SetActive(true);
        }

        private void UnableAll() // Disable all screens of pause menu
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}