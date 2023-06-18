using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Oversmith.Scripts.Figurants
{
    public class FigurantsManager : MonoBehaviour
    {
        public List<Figurant> agents = new List<Figurant>();
        public List<Transform> points = new List<Transform>();

        void Start ()
        {
            foreach (var agent in agents)
            {
                SetAgent(agent);
            }
        }

        public void SetAgent (Figurant agent)
        {
            int target;
            target = Random.Range(0, points.Count);
            agent.transform.position = points[target].position;
            target = Random.Range(0, points.Count);
            agent.SetDestination(points[target].position);
        }
    }
}
