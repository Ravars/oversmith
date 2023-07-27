using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MadSmith.Scripts.Multiplayer.Player
{
    // [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : NetworkBehaviour
    {
        public float speed = 0.1f;

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Level01")
                
            {
                if (hasAuthority)
                {
                    Movement();
                }
            }
        }

        public void Movement()
        {
            float xDirection = UnityEngine.Input.GetAxis("Horizontal");
            float zDirection = UnityEngine.Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

            transform.position += moveDirection * (speed * Time.deltaTime);

        }
    }
}