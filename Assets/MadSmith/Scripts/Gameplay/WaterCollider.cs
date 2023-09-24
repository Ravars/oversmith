using System;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    public class WaterCollider : MonoBehaviour
    {
        [SerializeField] private Transform pointToSpawn;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                playerMovement.DisableInput(pointToSpawn);
            }
        }
    }
}