using System;
using System.Collections;
using Oversmith.Scripts.Menu;
using Oversmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public bool buttonClicked;
        public bool sceneLoadLoaded;
        private AsyncOperation async;

        public delegate void PauseGameEvent();
        public PauseGameEvent OnPauseGame;
        public delegate void ResumeGameEvent();
        public ResumeGameEvent OnResumeGame;

        private void Start()
        {
            StartCoroutine(LoadSceneAsync(LevelNames.HomePage));
        }

        public void LoadHomePage()
        {
            buttonClicked = true;
            async.allowSceneActivation = sceneLoadLoaded && buttonClicked;
        }

        public void LoadGameLevel(int level)
        {
            switch (level)
            {
                case 1:
                    StartCoroutine(LoadSceneAsync(LevelNames.Level01));
                    break;
            }
        }

        private IEnumerator LoadSceneAsync(LevelNames levelNames){
            async = SceneManager.LoadSceneAsync((int)levelNames, LoadSceneMode.Single);
            async.allowSceneActivation = false;
            while(async.progress <= 0.89f ){
                // progressText.text = async.progress.ToString();
                yield return null;
            }
            sceneLoadLoaded = true;
            async.allowSceneActivation = sceneLoadLoaded && buttonClicked;

        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            OnPauseGame?.Invoke();
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
            OnResumeGame?.Invoke();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}