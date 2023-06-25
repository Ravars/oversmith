using System;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace MadSmith.Scripts.Gameplay
{
    [RequireComponent(typeof( NavMeshAgent))]
    public class Client : MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");
        
        [Range(0.01f, 1f)] 
        private const float Threshold = 0.1f;
        private bool _isMoving;
        private NavMeshAgent _agent;
        private Animator _animator;
        private int _npcIndex;
        private ClientsManager _clientsManager;
        private bool _arrived;
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
        }

        public void Init(Vector3 destination, int newNpcIndex, ClientsManager clientsManager)
        {
            _agent.SetDestination(destination);
            _isMoving = true;
            _npcIndex = newNpcIndex;
            _clientsManager = clientsManager;
        }

        private void Update()
        {
            Moving();
        }

        private void Moving()
        {
            if (!_isMoving) return;
            if (_agent.remainingDistance < Threshold)
            {
                _isMoving = false;
                _agent.isStopped = true;
                if (!_arrived)
                {
                    _clientsManager.ClientArrived(_npcIndex);
                    _animator.SetBool(Run, false);
                    return;
                }
            }
            _animator.SetBool(Run,_agent.velocity.magnitude > 0);
        }

        public void MoveAway(Vector3 pointToMoveAway)
        {
            _agent.SetDestination(pointToMoveAway);
            _isMoving = true;
            
        }
    }
}