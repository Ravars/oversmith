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
        [SerializeField] private FloatEventChannelSO _onLevelCompleted;
        
        [Header("Broadcasting to")]
        [SerializeField] private FloatEventChannelSO onPlayerScore;
        [SerializeField] private FloatEventChannelSO onEnemyScore;

        bool scoringEnded = false;
        float playerScore = 0;
        float enemyScore = 0;
        bool isTouching = false;

        public float PlayerScore => playerScore;
        public float EnemyScore => enemyScore;
        public bool IsTouching => isTouching;

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
            onEnemyScore.RaiseEvent(enemyScore/totalScore);
            if (enemyScore >= totalScore - playerScore) 
            {
                playerScore = totalScore - enemyScore;
                isTouching = true;
            }
            if (enemyScore >= totalScore)
            {
                _onLevelCompleted.RaiseEvent((playerScore / totalScore) * 100);
                scoringEnded = true;
            }
        }
        public void PlayerScores() => PlayerScores(orderPoints);
        public void PlayerScores (float points)
        {
            if (scoringEnded) return;
            playerScore = Mathf.Min(playerScore + points, totalScore);
            onPlayerScore.RaiseEvent(playerScore/totalScore);
            if (playerScore >= totalScore - enemyScore)
            {
                enemyScore = totalScore - playerScore;
                onEnemyScore.RaiseEvent(enemyScore/totalScore);
                isTouching = true;
            }
            if (playerScore >= totalScore)
            {
                _onLevelCompleted.RaiseEvent((playerScore / totalScore) * 100);
                scoringEnded = true;
            }
        }
        public void ApplyPenalty ()
        {
            if (scoringEnded) return;
            playerScore = Mathf.Max(playerScore - orderPoints * penaltyCost, 0);
            onPlayerScore.RaiseEvent(playerScore/totalScore);
            isTouching = false;
        }
        public void Reset ()
        {
            playerScore = 0;
            enemyScore = 0;
            isTouching = false;
            onPlayerScore.RaiseEvent(playerScore/totalScore);
            onEnemyScore.RaiseEvent(enemyScore/totalScore);
        }
    }
}
