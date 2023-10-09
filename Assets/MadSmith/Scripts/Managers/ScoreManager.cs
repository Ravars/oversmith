using UnityEngine;
using MadSmith.Scripts.Scoring;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Utils;

namespace MadSmith.Scripts.Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
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
