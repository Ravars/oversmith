using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Old.Managers
{
    public class PlayerSpawnSystem : NetworkBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;

        private static List<Transform> spawnPoints = new List<Transform>();

        private int nextIndex = 0;
        private MadSmithNetworkManager _manager;
        private MadSmithNetworkManager Manager
        { get {
                if (!ReferenceEquals(_manager, null)) return _manager;
                return _manager = NetworkManager.singleton as MadSmithNetworkManager;
            }
        }

        private void Start()
        {
            Debug.Log("PlayerSpawnSystem");
        }

        public static void AddSpawnPoint(Transform transform)
        {
            spawnPoints.Add(transform);

            spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
        }
        public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

        
        public override void OnStartServer()
        {
            Debug.Log("OnStartServer PlayerSpawnSystem");
            MadSmithNetworkManager.OnServerReadied += SpawnPlayer;
        }

        public override void OnStartClient()
        {
            Debug.Log("Enable Look");
            Manager.NotifyPlayerReady(NetworkClient.localPlayer.connectionToClient);
            // InputManager.Add(ActionMapNames.Player);
            // InputManager.Controls.Player.Look.Enable();
        }

        // [ServerCallback]
        private void OnDestroy() => MadSmithNetworkManager.OnServerReadied -= SpawnPlayer;

        [Server]
        public void SpawnPlayer(NetworkConnection conn)
        {
            Debug.Log(gameObject.name);
            Transform spawnPoint = spawnPoints.ElementAtOrDefault(nextIndex);

            if (spawnPoint == null)
            {
                Debug.LogError($"Missing spawn point for player {nextIndex}");
                return;
            }

            var prefab = Manager.GetPlayerGameObject(conn.identity.connectionToClient);

            GameObject playerInstance = Instantiate(prefab.gameObject, spawnPoints[nextIndex].position, spawnPoints[nextIndex].rotation);
            NetworkServer.Spawn(playerInstance, conn);

            nextIndex++;
        }
    }
}