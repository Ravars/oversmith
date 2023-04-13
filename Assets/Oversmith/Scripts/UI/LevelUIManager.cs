using System;
using Oversmith.Scripts.Managers;
using Oversmith.Scripts.Menu;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public enum ScreenType
    {
        Hud,
        Pause,
        Config,
    }
    public class LevelUIManager : MonoBehaviour
    {
        
        [SerializeField] private FadeUI hudMenuCanvas;
        [SerializeField] private FadeUI pauseMenuCanvas;
        [SerializeField] private FadeUI configMenuCanvas;
        private ScreenType _currentScreenType;
        
        private void Start()
        {
            if (GameManager.InstanceExists)
            {
                GameManager.Instance.OnPauseGame += OnPauseGame;
                GameManager.Instance.OnResumeGame += OnResumeGame;
            }
            ChangeScreen(ScreenType.Hud);
        }

        private void OnResumeGame()
        {
            switch (_currentScreenType)
            {
                case ScreenType.Hud:
                    //Teoricamente nao acontece
                    break;
                case ScreenType.Pause:
                    ChangeScreen(ScreenType.Hud);
                    break;
                case ScreenType.Config:
                    //TODO: confirm screen
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPauseGame()
        {
            
            ChangeScreen(ScreenType.Pause);
        }

        private void ChangeScreen(ScreenType newScreenType)
        {
            hudMenuCanvas.gameObject.SetActive(false);
            pauseMenuCanvas.gameObject.SetActive(false);
            configMenuCanvas.gameObject.SetActive(false);

            _currentScreenType = newScreenType;
            
            switch (_currentScreenType)
            {
                case ScreenType.Hud:
                    hudMenuCanvas.gameObject.SetActive(true);
                    hudMenuCanvas.BeginFadeIn();
                    break;
                case ScreenType.Pause:
                    pauseMenuCanvas.gameObject.SetActive(true);
                    pauseMenuCanvas.BeginFadeIn();
                    break;
                case ScreenType.Config:
                    configMenuCanvas.gameObject.SetActive(true);
                    configMenuCanvas.BeginFadeIn();
                    break;
            }
        }

        public void ResumeButton()
        {
            if (GameManager.InstanceExists)
            {
                GameManager.Instance.ResumeGame();
            }
            else
            {
                Debug.LogError("Game Manager not instanced");
            }
        }
        public void ConfigButton()
        {
            ChangeScreen(ScreenType.Config);
        }

        public void CloseConfigButton()
        {
            ChangeScreen(ScreenType.Pause);
        }
        public void QuitButton()
        {
            if (GameManager.InstanceExists)
            {
                GameManager.Instance.QuitGame();
            }
            else
            {
                Debug.LogError("Game Manager not instanced");
            }
        }
    }
}
