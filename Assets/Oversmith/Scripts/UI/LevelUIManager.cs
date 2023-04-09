using System;
using Oversmith.Scripts.Managers;
using UnityEngine;

namespace Oversmith.Scripts.UI
{
    public enum ScreenType
    {
        Hud,
        Pause,
    }
    public class LevelUIManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject hudMenuCanvas;
        [SerializeField] private GameObject pauseMenuCanvas;
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
            ChangeScreen(ScreenType.Hud);
        }

        private void OnPauseGame()
        {
            ChangeScreen(ScreenType.Pause);
        }

        private void ChangeScreen(ScreenType newScreenType)
        {
            
            switch (_currentScreenType)
            {
                case ScreenType.Hud:
                    hudMenuCanvas.SetActive(false);
                    break;
                case ScreenType.Pause:
                    pauseMenuCanvas.SetActive(false);
                    break;
            }

            _currentScreenType = newScreenType;
            
            switch (newScreenType)
            {
                case ScreenType.Hud:
                    hudMenuCanvas.SetActive(true);
                    break;
                case ScreenType.Pause:
                    pauseMenuCanvas.SetActive(true);
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
