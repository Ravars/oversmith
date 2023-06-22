using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
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
            Debug.Log(GameManager.InstanceExists);
            var index = GameManager.Instance.characterIndex;
            var player = Instantiate(GameManager.Instance.charactersPrefabs[index], spawnLocation.position, Quaternion.identity, spawnLocation);
            _inputReader.EnableGameplayInput();
        }
    }
}