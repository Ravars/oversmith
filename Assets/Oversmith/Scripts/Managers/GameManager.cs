using System;
using System.Collections;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Menu;
using Oversmith.Scripts.SceneManagement.ScriptableObjects;
using Oversmith.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public int characterIndex = 0;
        public GameObject[] charactersPrefabs;

        public GameSceneSO _currentSceneSo { get; private set; }

        [Header("Listening To")] 
        [SerializeField] private LoadEventChannelSO loadLocation = default;


        private void OnEnable()
        {
         loadLocation.OnLoadingRequested += OnLoadingRequested;   
        }

        private void OnLoadingRequested(GameSceneSO arg0, bool arg1, bool arg2)
        {
            _currentSceneSo = arg0;
        }

        private void OnDisable()
        {
            
        }
    }
}