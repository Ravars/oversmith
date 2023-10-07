using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace MadSmith.Scripts.Gameplay
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        [Header("Scene Ready Event")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady;
        [SerializeField] private Transform spawnLocation;

        [SerializeField] private Image tutorialImage;

        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += SpawnTutorialUI;
            _inputReader.EnableMenuInput();
            _inputReader.MenuCloseEvent += CloseTutorial;
        }

        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= SpawnTutorialUI;
            _inputReader.MenuCloseEvent -= CloseTutorial;
        }

        private void SpawnTutorialUI()
        {
            _onSceneReady.OnEventRaised -= SpawnTutorialUI;
            if (tutorialImage != null)
            {
                tutorialImage.gameObject.SetActive(true);
            }
            else
            {
                CloseTutorial();
            }
        }

        public void CloseTutorial()
        {
            _inputReader.MenuCloseEvent -= CloseTutorial;
            if (tutorialImage != null)
            {
                tutorialImage.gameObject.SetActive(false);
            }
            var index = GameManager.Instance.characterIndex;
            var player = Instantiate(GameManager.Instance.charactersPrefabs[index], spawnLocation.position, Quaternion.identity, spawnLocation);
            _inputReader.EnableGameplayInput();
            
            
        }
        
    }
}