using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Utils;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Managers
{
    public class NetworkScoreManager : NetworkSingleton<NetworkScoreManager>
    {
        private float _playerScore;
        private float _enemyScore;
        private bool _isTouching;
        private bool _isScoring;
        
        
        [SerializeField] private float totalScore = 5000;
        [SerializeField] private float orderPoints = 1000;
        [SerializeField] private float penaltyCost = 0.2f;
        [SerializeField] private float enemyScoringRate;
        
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
            onDeliveryOrder.OnEventRaised += PlayerScore;
            onMissedOrder.OnEventRaised += ApplyPenalty;
            _isScoring = true;
            onCountdownTimerUpdated.OnEventRaised += CountDownUpdated;
            // if (!isServer) return;
        }
        private void OnDisable()
        {
            onDeliveryOrder.OnEventRaised -= PlayerScore;
            onMissedOrder.OnEventRaised -= ApplyPenalty;
            onCountdownTimerUpdated.OnEventRaised -= CountDownUpdated;
            // if (!isServer) return;
        }
        private void CountDownUpdated(int timeRemaining)
        {
            if (!isServer) return;
            EnemyScore(Time.fixedDeltaTime * enemyScoringRate);
        }
        
        private void EnemyScore(float points)
        {
            if (!isServer || !_isScoring) return;
            _enemyScore = Mathf.Min(_enemyScore + points, totalScore);
            RpcEnemyScore(_enemyScore/totalScore);
            if (_enemyScore >= totalScore - _playerScore) 
            {
                _playerScore = totalScore - _enemyScore;
                _isTouching = true;
            }
            if (_enemyScore >= totalScore)
            {
                RpcLevelCompleted((_playerScore / totalScore) * 100);
                _isScoring = false;
            }
        }
        private void PlayerScore(OrderData orderData)
        {
            if (!isServer || !_isScoring) return;
            _playerScore = Mathf.Min(_playerScore + orderPoints, totalScore);
            RpcPlayerScore(_playerScore / totalScore);
            if (_playerScore >= totalScore - _enemyScore)
            {
                _enemyScore = totalScore - _playerScore;
                RpcEnemyScore(_enemyScore/totalScore);
                _isTouching = true;
            }
            if (_playerScore >= totalScore)
            {
                RpcLevelCompleted((_playerScore / totalScore) * 100);
                _isScoring = false;
            }
        }
        private void ApplyPenalty(OrderData orderData)
        {
            if (!isServer) return;
            _playerScore = Mathf.Max(_playerScore - orderPoints * penaltyCost, 0);
            RpcPlayerScore(_playerScore/totalScore);
            _isTouching = false;
        }
        [ClientRpc]
        private void RpcPlayerScore(float score)
        {
            onPlayerScore.RaiseEvent(score);
        }

        [ClientRpc]
        private void RpcEnemyScore(float score)
        {
            onEnemyScore.RaiseEvent(score);
        }
        [ClientRpc]
        private void RpcLevelCompleted(float score)
        {
            onLevelCompleted.RaiseEvent(score);
        }
    }
}