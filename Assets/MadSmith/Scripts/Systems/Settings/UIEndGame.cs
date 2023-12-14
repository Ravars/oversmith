using MadSmith.Scripts.Multiplayer.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace MadSmith.Scripts.Systems.Settings
{
    public class UIEndGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        public event UnityAction Continued = default;
        public event UnityAction BackToMenuClicked = default;
        public LocalizedString victoryText;
        public LocalizedString loseText;
        [SerializeField] private LocalizeStringEvent titleText;
        
        public void Setup(int finalScore)
        {
            string scoreString = GameManager.CalculateScore(finalScore);
            scoreText.text = scoreString;
            titleText.StringReference = finalScore >= 70 ? victoryText: loseText;
        }

        public void ContinueButton()
        {
            Continued?.Invoke();
        }

        public void BackToMenuButton()
        {
            BackToMenuClicked?.Invoke();
        }
    }
}