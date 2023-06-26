using System.Collections.Generic;
using MadSmith.Scripts.Interaction;
using MadSmith.Scripts.Items;
using UnityEngine;

namespace MadSmith.Scripts.CraftingTables
{
    [RequireComponent(typeof(InteractableHolder),typeof(CraftingInteractionHandler), typeof(AudioSource))]
    public class CraftingTable : MonoBehaviour
    {
        [SerializeField] protected float timeToPrepareItem = 10f;
        [SerializeField] protected float currentTimeToPrepareItem;
        protected List<PlayerInteractions> PlayerInteractionsArray = new();
        private Table _table;
        protected CraftingInteractionHandler _craftingInteractionHandler;
        public CraftingTableType type;
        public ParticleSystem[] particleSystems;
        public bool CanAddPlayer { get; protected set; } = true;

        [HideInInspector] public AudioSource _audioSource;
        private Animator _animator;
        
        protected virtual void Awake()
        {
            _table = GetComponent<Table>();
            _craftingInteractionHandler = GetComponent<CraftingInteractionHandler>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponentInChildren<Animator>();
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
                if (particle != null)
                {
                    particle.gameObject.SetActive(state);
                }
            }

            if (_animator != null)
            {
                _animator.SetBool("Rolling", state);
            }
        }

        public virtual void ItemAddedToTable()
        {
            // Debug.Log("Item added");
        }
    }
}