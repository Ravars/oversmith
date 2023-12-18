using _Developers.Nicole.ScriptableObjects.Data_Structures;
using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
    public class LocationSO : GameSceneSO
    {
        public string sceneName;
        public TutorialDataSO tutorialDataSo;
        public LocalizedString locationName;
        public LevelConfigItems levelConfigItems;
    }
}