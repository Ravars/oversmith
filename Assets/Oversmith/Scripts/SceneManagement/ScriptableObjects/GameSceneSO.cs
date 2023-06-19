using Oversmith.Scripts.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Oversmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Game Scene", menuName = "Scene Data/Game Scene")]
    public class GameSceneSO : DescriptionBaseSO
    {
        public GameSceneType sceneType;
        public AssetReference sceneReference;
        public GameSceneSO nextScene;
    }

    public enum GameSceneType
    {
        Initialization,
        PersistentManager,
        Gameplay,
        Menu,
        Location
    }
}