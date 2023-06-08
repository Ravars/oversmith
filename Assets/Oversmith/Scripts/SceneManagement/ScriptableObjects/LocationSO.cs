using UnityEngine;
using UnityEngine.Localization;

namespace Oversmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewLocation", menuName = "Scene Data/Location")]
    public class LocationSO : GameSceneSO
    {
        public LocalizedString locationName;
    }
}