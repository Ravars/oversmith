using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Oversmith.Scripts.Figurants
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Figurant : MonoBehaviour
    {
        [Range(0.01f, 1f)]
        public float threshold = 0.1f;
        public float limitTime = 20f;
        public UnityEvent<Figurant> OnArrival;

        NavMeshAgent agent;
        Vector3 destination;
        Animator animator;
        bool isMoving = false;
        bool isWalking = false;
        float timeMoving;

        void Start ()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            OnArrival.Invoke(this);
        }

        void Update ()
        {
            Moving();
        }

        public bool SetDestination (Vector3 destination)
        {
            bool bb = false;
            this.destination = destination;
            if (agent)
            {
                bb = agent.SetDestination(destination);
                timeMoving = 0;
                isMoving = true;
            }
            agent.speed = Random.Range(3f, 5f);
            return bb;
        }

        void Moving ()
        {
            if (!isMoving) return;
            timeMoving += Time.deltaTime;

            if (agent.remainingDistance < threshold || timeMoving > limitTime)
            {
                    animator.SetBool("Run", false);
                    isWalking = false;
                    isMoving = false;
                    OnArrival.Invoke(this);
            }
            else if (!isWalking && agent.velocity.magnitude > 0)
            {
                animator.SetBool("Run", true);
                isWalking = true;
            }
        }
    }

}