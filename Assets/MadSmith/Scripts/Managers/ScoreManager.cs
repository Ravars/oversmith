using System;
using UnityEngine;
using MadSmith.Scripts.Scoring;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Utils;

namespace MadSmith.Scripts.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        private float _playerScore;
        private float _enemyScore;
        private bool _isTouching;
        private bool _isScoring;
        
        
        [SerializeField] private float totalScore = 5000;
        [SerializeField] private float orderPoints = 1000;
        [SerializeField] private float penaltyCost = 0.2f;
        [SerializeField] float enemyScoringRate;
        
        [Header("Listening to")]
        [SerializeField] private IntEventChannelSO onCountdownTimerUpdated;
        [SerializeField] private OrderUpdateEventChannelSO onMissedOrder;
        [SerializeField] private OrderUpdateEventChannelSO onDeliveryOrder;
        
        [Header("Broadcasting to")]
        [SerializeField] private FloatEventChannelSO onPlayerScore;
        [SerializeField] private FloatEventChannelSO onEnemyScore;
        [SerializeField] private FloatEventChannelSO onLevelCompleted;
        private void Start()
        {
            onCountdownTimerUpdated.OnEventRaised += CountDownUpdated;
            onDeliveryOrder.OnEventRaised += PlayerScore;
            onMissedOrder.OnEventRaised += ApplyPenalty;
            _isScoring = true;
        }
        private void OnDisable()
        {
            onCountdownTimerUpdated.OnEventRaised -= CountDownUpdated;
            onDeliveryOrder.OnEventRaised -= PlayerScore;
            onMissedOrder.OnEventRaised -= ApplyPenalty;
        }

        private void CountDownUpdated(int timeRemaining)
        {
            EnemyScore(Time.fixedDeltaTime * enemyScoringRate);
        }

        private void EnemyScore(float points)
        {
            if (!_isScoring) return;
            _enemyScore = Mathf.Min(_enemyScore + points, totalScore);
            onEnemyScore.RaiseEvent(_enemyScore/totalScore);
            if (_enemyScore >= totalScore - _playerScore) 
            {
                _playerScore = totalScore - _enemyScore;
                _isTouching = true;
            }
            if (_enemyScore >= totalScore)
            {
                onLevelCompleted.RaiseEvent((_playerScore / totalScore) * 100);
                _isScoring = false;
            }
        }

        private void PlayerScore(OrderData orderData)
        {
            if (!_isScoring) return;
            _playerScore = Mathf.Min(_playerScore + orderPoints, totalScore);
            onPlayerScore.RaiseEvent(_playerScore/totalScore);
            if (_playerScore >= totalScore - _enemyScore)
            {
                _enemyScore = totalScore - _playerScore;
                onEnemyScore.RaiseEvent(_enemyScore/totalScore);
                _isTouching = true;
            }
            if (_playerScore >= totalScore)
            {
                onLevelCompleted.RaiseEvent((_playerScore / totalScore) * 100);
                _isScoring = false;
            }
        }

        private void ApplyPenalty(OrderData orderData)
        {
            _playerScore = Mathf.Max(_playerScore - orderPoints * penaltyCost, 0);
            onPlayerScore.RaiseEvent(_playerScore/totalScore);
            _isTouching = false;
        }
        



        // [SerializeField] float totalPoints;
        // [SerializeField] float itemPoints;
        // [SerializeField][Range(0,1)] float penaltyCost;
        // [SerializeField] float enemyScoringRate;
        // [SerializeField] ScoringSO scoring;
        // [Header("Listening to")]
        // [SerializeField] VoidEventChannelSO onItemDelivering;
        // [SerializeField] VoidEventChannelSO onItemMissed;
        //
        // public float TotalScore => totalPoints;
        // public float ItemPoints => itemPoints;
        // public float PenaltyCost => penaltyCost;
        // public float EnemyScoringRate => enemyScoringRate;
        // public float PlayerScore => scoring.PlayerScore;
        // public float EnemyScore => scoring.EnemyScore;
        //
        // protected override void Awake()
        // {
        //     base.Awake();
        //     onItemDelivering.OnEventRaised += OnItemDelivered;
        //     onItemMissed.OnEventRaised += OnItemMissed;
        // }
        //
        // protected override void OnDestroy()
        // {
        //     base.OnDestroy();
        //     onItemDelivering.OnEventRaised -= OnItemDelivered;
        //     onItemMissed.OnEventRaised -= OnItemMissed;
        // }
        // private void Start ()
        // {
        //     scoring.totalScore = totalPoints;
        //     scoring.orderPoints = itemPoints;
        //     scoring.penaltyCost = penaltyCost;
        //     scoring.Reset();
        // }
        // private void Update ()
        // {
        //     scoring.EnemyScores(enemyScoringRate * Time.deltaTime);
        // }
        //
        // public void OnItemDelivered ()
        // {
        //     scoring.PlayerScores();
        // }
        // public void OnItemMissed ()
        // {
        //     scoring.ApplyPenalty();
        // }
    }
}
