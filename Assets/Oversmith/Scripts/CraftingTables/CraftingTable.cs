using System.Collections.Generic;
using UnityEngine;

namespace _Developers.Vitor
{
    [RequireComponent(typeof(InteractableHolder),typeof(CraftingInteractionHandler))]
    public class CraftingTable : MonoBehaviour
    {
        [SerializeField] protected float timeToPrepareItem = 10f;
        [SerializeField] protected float currentTimeToPrepareItem;
        protected List<PlayerInteractions> PlayerInteractionsArray = new();
        private Table _table;
        private CraftingInteractionHandler _craftingInteractionHandler;
        public CraftingTableType type;
        public ParticleSystem[] particleSystems;

        private void Awake()
        {
            _table = GetComponent<Table>();
            _craftingInteractionHandler = GetComponent<CraftingInteractionHandler>();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerInteractions playerInteraction))
            {
                if (PlayerInteractionsArray.Contains(playerInteraction))
                {
                    PlayerInteractionsArray.Remove(playerInteraction);
                    _craftingInteractionHandler.NumberOfPlayers = PlayerInteractionsArray.Count;
                }
            }
        } 
        public void AddPlayer(PlayerInteractions playerInteraction)
        {
            // Debug.Log(PlayerInteractionsArray.Contains(playerInteraction));
            if (!PlayerInteractionsArray.Contains(playerInteraction))
            {
                PlayerInteractionsArray.Add(playerInteraction);
            }
            
            if (!_craftingInteractionHandler.isRunning)
            {
                _craftingInteractionHandler.Init(timeToPrepareItem,2);
                
            }
            else
            {
                _craftingInteractionHandler.NumberOfPlayers = PlayerInteractionsArray.Count;
            }
        }

        public void SetParticlesState(bool state)
        {
            foreach (var particle in particleSystems)
            {
                particle.gameObject.SetActive(false);
            }
        }

        // private void Update()
        // {
        //     if()
        // }
    }
}