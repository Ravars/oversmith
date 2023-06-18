using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using Oversmith.Scripts.Input;
using UnityEngine;

namespace Oversmith.Scripts.Gameplay
{
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private PlayerMovement _playerPrefab;

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
            var player = Instantiate(_playerPrefab, spawnLocation.position, Quaternion.identity, spawnLocation);
            _inputReader.EnableGameplayInput();
        }
    }
}