using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace MadSmith.Scripts.Multiplayer.Player
{
    // [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : NetworkBehaviour
    {
        public float speed = 0.1f;
        public GameObject playerModel;
        
        private void Start()
        {
            playerModel.SetActive(false);   
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Showcase")
            {
                if (!playerModel.activeSelf)
                {
                    SetPosition();
                    playerModel.SetActive(true);
                }
                if (hasAuthority)
                {
                    Movement();
                }
            }
        }

        public void SetPosition()
        {
            transform.position = new Vector3(Random.Range(-5, 5), 0.8f, Random.Range(-5, 5));
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