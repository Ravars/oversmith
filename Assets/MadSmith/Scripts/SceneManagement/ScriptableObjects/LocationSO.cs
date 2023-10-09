using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
    public class LocationSO : GameSceneSO
    {
        public TutorialDataSO tutorialDataSo;
        public LocalizedString locationName;
    }
}