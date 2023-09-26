using MadSmith.Scripts.Events.ScriptableObjects;
using UnityEngine;

namespace MadSmith.Scripts.Scoring
{
    [CreateAssetMenu(menuName = "Data/Scoring Data")]
    public class ScoringSO : ScriptableObject
    {
        [HideInInspector] public float totalScore = 0;
        [HideInInspector] public float orderPoints = 0;
        [HideInInspector] public float penaltyCost = 0;
        [Header("Listening to")]
        [SerializeField] VoidEventChannelSO onItemDelivering;
        [SerializeField] VoidEventChannelSO onItemMissed;
        [Header("Broadcasting to")]
        [SerializeField] VoidEventChannelSO onPlayerWin;
        [SerializeField] VoidEventChannelSO onEnemyWin;

        bool scoringEnded = false;
        float playerScore = 0;
        float enemyScore = 0;
        bool isTouching = false;

        public float PlayerScore => playerScore;
        public float EnemyScore => enemyScore;
        public bool IsTouching => isTouching;

        private void Awake()
        {
            onItemDelivering.OnEventRaised += PlayerScores;
            onItemMissed.OnEventRaised += ApplyPenalty;
        }
        private void OnEnable()
        {
            if (playerScore != totalScore && enemyScore != totalScore)
                scoringEnded = true;
        }
        private void OnDisable()
        {
            scoringEnded = false;
        }

        public void EnemyScores (float points)
        {
            if (scoringEnded) return;
            enemyScore = Mathf.Min(enemyScore + points, totalScore);
            if (enemyScore >= totalScore - playerScore) 
            {
                playerScore = totalScore - enemyScore;
                isTouching = true;
            }
            if (enemyScore == totalScore)
            {
                onEnemyWin.RaiseEvent();
                scoringEnded = true;
            }
        }
        public void PlayerScores() => PlayerScores(orderPoints);
        public void PlayerScores (float points)
        {
            if (scoringEnded) return;
            playerScore = Mathf.Min(playerScore + points, totalScore);
            if (playerScore >= totalScore - enemyScore)
            {
                enemyScore = totalScore - playerScore;
                isTouching = true;
            }
            if (playerScore == totalScore)
            {
                onPlayerWin.RaiseEvent();
                scoringEnded = true;
            }
        }
        public void ApplyPenalty ()
        {
            if (scoringEnded) return;
            playerScore = Mathf.Max(playerScore - orderPoints * penaltyCost, 0);
            isTouching = false;
        }
    }
}
