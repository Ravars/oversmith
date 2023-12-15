using System;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Multiplayer.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _onSceneReady = default; //picked up by the SpawnSystem

        private void Awake()
        {
            if (!GameManager.InstanceExists)
            {
                SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive).completed += OnCompleted;
            }
        }

        private void OnCompleted(AsyncOperation obj)
        {
            _onSceneReady.RaiseEvent();
        }
    }
}