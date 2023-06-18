using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Input;
using Oversmith.Scripts.Managers;
using UnityEngine;

namespace Oversmith.Scripts.Gameplay
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;

        [Header("Scene Ready Event")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady;

        [SerializeField] private Transform spawnLocation;

        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += SpawnPlayer;
        }

        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            var index = GameManager.Instance.characterIndex;
            var player = Instantiate(GameManager.Instance.charactersPrefabs[index], spawnLocation.position, Quaternion.identity, spawnLocation);
            _inputReader.EnableGameplayInput();
        }
    }
}