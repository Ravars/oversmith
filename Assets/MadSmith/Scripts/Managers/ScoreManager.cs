using UnityEngine;
using MadSmith.Scripts.Scoring;

namespace MadSmith.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] float totalPoints;
        [SerializeField] float itemPoints;
        [SerializeField] float penaltyCost;
        [SerializeField] float enemyScoringRate;
        [SerializeField] ScoringSO scoring;

        public float TotalScore => totalPoints;
        public float ItemPoints => itemPoints;
        public float PenaltyCost => penaltyCost;
        public float EnemyScoringRate => enemyScoringRate;
        public float PlayerScore => scoring.PlayerScore;
        public float EnemyScore => scoring.EnemyScore;

        private void Start ()
        {
            scoring.totalScore = totalPoints;
            scoring.orderPoints = itemPoints;
            scoring.penaltyCost = penaltyCost;
        }
        private void Update ()
        {
            scoring.EnemyScores(enemyScoringRate * Time.deltaTime);
        }
    }
}
