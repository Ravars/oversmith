using MadSmith.Scripts.BaseClasses;
using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MadSmith.Scripts.SceneManagement.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Game Scene", menuName = "Scene Data/Game Scene")]
    public class GameSceneSO : DescriptionBaseSO
    {
        public GameSceneType sceneType;
        public AssetReference sceneReference;
        public Levels level;
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