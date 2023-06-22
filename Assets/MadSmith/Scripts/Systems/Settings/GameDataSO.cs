using System.Collections.Generic;
using MadSmith.Scripts.SavingSystem;
using UnityEngine;
using UnityEngine.Localization;

namespace MadSmith.Scripts.Systems.Settings
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/Create new Game Data")]
    public class GameDataSO : ScriptableObject
    {
        //Levels
        [SerializeField] private List<SerializedLevelScore> levelScores;
       

        public List<SerializedLevelScore> LevelScores => levelScores;
        

        public GameDataSO() { }

        public void LoadSavedSettings(Save savedFile)
        {
            levelScores = new List<SerializedLevelScore>();
            levelScores.AddRange(savedFile.levelScores);
        }

        public void LoadDefaultSettings()
        {
            levelScores = new List<SerializedLevelScore>()
            {
                new() { score = -1, Level = Levels.Level01 }
            };
        }

        public void SaveLevelScore(Levels level, int score)
        {
            var index = levelScores.FindIndex(x => x.Level == level);
            if (index != -1)
            {
                if (levelScores[index].score < score)
                {
                    levelScores[index].score = score;
                }
            }
            else
            {
                levelScores.Add(new SerializedLevelScore { score = score, Level = level});
            }
        }
    }
}