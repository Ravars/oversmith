using UnityEngine;
using MadSmith.Scripts.Scoring;
using MadSmith.Scripts.Events.ScriptableObjects;

namespace MadSmith.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] float totalPoints;
        [SerializeField] float itemPoints;
        [SerializeField] float penaltyCost;
        [SerializeField] float enemyScoringRate;
        [SerializeField] ScoringSO scoring;
        [Header("Listening to")]
        [SerializeField] VoidEventChannelSO onItemDelivering;
        [SerializeField] VoidEventChannelSO onItemMissed;

        public float TotalScore => totalPoints;
        public float ItemPoints => itemPoints;
        public float PenaltyCost => penaltyCost;
        public float EnemyScoringRate => enemyScoringRate;
        public float PlayerScore => scoring.PlayerScore;
        public float EnemyScore => scoring.EnemyScore;

        private void Awake()
        {
            onItemDelivering.OnEventRaised += OnItemDelivered;
            onItemMissed.OnEventRaised += OnItemMissed;
        }
        private void Start ()
        {
            scoring.totalScore = totalPoints;
            scoring.orderPoints = itemPoints;
            scoring.penaltyCost = penaltyCost;
            scoring.Reset();
        }
        private void Update ()
        {
            scoring.EnemyScores(enemyScoringRate * Time.deltaTime);
        }

        public void OnItemDelivered ()
        {
            scoring.PlayerScores();
        }
        public void OnItemMissed ()
        {
            scoring.ApplyPenalty();
        }
    }
}
