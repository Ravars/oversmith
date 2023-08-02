using System.Collections.Generic;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Developers.Vitor.Multiplayer_1.Scripts
{
    public class HelloWorldNetworkManager : NetworkManager
    {
        [SerializeField] private LobbyClient gamePlayerPrefab;
        public List<LobbyClient> players = new List<LobbyClient>();
        [Scene] [SerializeField] private string menuScene;
        public override void OnStartServer()
        {
            base.OnStartServer();
            Debug.Log("Server Started");
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            Debug.Log("Server stopped");
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // base.OnServerAddPlayer(conn);
            LobbyClient lobbyClient = Instantiate(gamePlayerPrefab);
            lobbyClient.ConnectionID = conn.connectionId;

            NetworkServer.AddPlayerForConnection(conn, lobbyClient.gameObject);
        }

        public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
        {
            base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
            Debug.Log("Client changed scene" + newSceneName);
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            Debug.Log("Client scene changed");
        }

        public override void OnServerChangeScene(string newSceneName)
        {
            Debug.Log("OnServerChangeScene "  + newSceneName);
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Level"))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var conn = players[i].connectionToClient;
                    var instance = Instantiate(gamePlayerPrefab);
                    NetworkServer.Destroy(conn.identity.gameObject);
                    NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                }
            }
            base.OnServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            Debug.Log("OnServerSceneChanged");
            // base.OnServerSceneChanged(sceneName);
            if (sceneName.StartsWith("Level"))
            {
                //
            }
        }

        public void Opa(string newSceneName)
        {
            // ServerChangeScene("Level1");
            Debug.Log(newSceneName);
            if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("Level"))
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var conn = players[i].connectionToClient;
                    // var instance = Instantiate(gamePlayerPrefab);
                    NetworkServer.Destroy(conn.identity.gameObject);
                    // NetworkServer.ReplacePlayerForConnection(conn, instance.gameObject);
                }
            }
        }
    }
}
