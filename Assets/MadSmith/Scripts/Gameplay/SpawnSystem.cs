// using System;
// using MadSmith.Scripts.Events.ScriptableObjects;
// using MadSmith.Scripts.Input;
// using MadSmith.Scripts.Managers;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace MadSmith.Scripts.Gameplay
// {
//     public class SpawnSystem : MonoBehaviour
//     {
//         [SerializeField] private InputReader _inputReader;
//
//         [Header("Scene Ready Event")] 
//         [SerializeField] private VoidEventChannelSO _onGameStart;
//         [SerializeField] private Transform spawnLocation;
//
//         private void OnEnable()
//         {
//             // _onGameStart.OnEventRaised += SpawnPlayer;
//         }
//
//         private void OnDestroy()
//         {
//             // _onGameStart.OnEventRaised -= SpawnPlayer;
//         }
//
//         public void SpawnPlayer()
//         {
//             _onGameStart.OnEventRaised -= SpawnPlayer;
//             var index = GameManager.Instance.characterIndex;
//             var player = Instantiate(GameManager.Instance.charactersPrefabs[index], spawnLocation.position, Quaternion.identity, spawnLocation);
//         }
//         
//     }
// }