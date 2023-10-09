using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "new Tutorial", menuName = "Scene Data/Tutorial")]
    public class TutorialDataSO : ScriptableObject
    {
        public Sprite tutorialImage;
        public LocalizedString locationTitle;
        public LocalizedString locationSecondaryText;
    }
}