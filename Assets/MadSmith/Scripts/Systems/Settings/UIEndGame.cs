using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Events.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace MadSmith.Scripts.Systems.Settings
{
    public class UIEndGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        public event UnityAction Continued = default;

        public void Setup(int finalScore)
        {
            string scoreString = GameManager.CalculateScore(finalScore);
            scoreText.text = scoreString;
        }

        public void ContinueButton()
        {
            Continued?.Invoke();
        }
        
        // public bu
    }
}