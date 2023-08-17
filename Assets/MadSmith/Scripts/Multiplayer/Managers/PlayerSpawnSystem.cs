using System.Collections.Generic;
using System.Linq;
using _Developers.Vitor.Multiplayer_1.Scripts;
using MadSmith.Scripts.Input;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        // private static List<Transform> spawnPoints = new List<Transform>();
        // private int nextIndex = 0;
        // [SerializeField] private InputReader inputReader;
        // [SerializeField] private GameObject playerPrefab = null;
        // public override void OnStartServer()
        // {
        //     HelloWorldNetworkManager.OnServerReadied += SpawnPlayer;
        // }
        // public static void AddSpawnPoint(Transform transform)
        // {
        //     spawnPoints.Add(transform);
        //
        //     spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        // }
        // public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);
        //
        // public override void OnStartClient()
        // {
        //     inputReader.EnableGameplayInput();
        // }

        // [Server]
        // private void SpawnPlayer(NetworkConnection conn)
        // {
        //     Debug.Log("SpawnPlayer");
        //     Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);
        //
        //     if (spawnPoint == null)
        //     {
        //         Debug.LogError($"Missing spawn point for player {nextIndex}");
        //         return;
        //     }
        //
        //     GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
        //     NetworkServer.Spawn(playerInstance, conn);
        //
        //     nextIndex++;
        // }
        
        // [ServerCallback]
        // private void OnDestroy() => HelloWorldNetworkManager.OnServerReadied -= SpawnPlayer;
    }
}