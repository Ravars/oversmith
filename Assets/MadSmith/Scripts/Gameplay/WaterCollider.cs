using System;
using UnityEngine;

namespace MadSmith.Scripts.Gameplay
{
    public class WaterCollider : MonoBehaviour
    {
        [SerializeField] private Transform pointToSpawn;
        [SerializeField] private GameObject splashVFX;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                Instantiate(splashVFX, other.transform.position, Quaternion.identity);
                playerMovement.DisableInput(pointToSpawn);
            }
        }
    }
}