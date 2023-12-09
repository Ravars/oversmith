using MadSmith.Scripts.Multiplayer.Old.Player;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Developers.Vitor
{
    public class TestNetworkManager : NetworkManager
    {
        [Scene]
        public string gameScene;
        public void EnableMovement()
        {
            NetworkPlayerMovement[] networkPlayerMovement = FindObjectsOfType<NetworkPlayerMovement>();
            
            
            foreach (var playerMovement in networkPlayerMovement)
            {
                playerMovement.CmdEnableMovement();
            }
            // GameObject orderManagerInstance = Instantiate(orderManager);
            // NetworkServer.Spawn(orderManagerInstance);
        }

        public void LoadScenes()
        {
            Debug.Log("LoadScenes");
            SceneManager.LoadSceneAsync(gameScene, new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics3D });
        }
    }
}
