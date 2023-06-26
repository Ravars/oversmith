using System;
using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.Systems.Settings;
using TMPro;
using UnityEngine;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UILevelSelection : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CinemachineTrackedDolly dolly;
        [SerializeField] private CinemachineSmoothPath dollyPath;
        [SerializeField] private int currentLevelSelected = 0;

        [SerializeField] private TextMeshProUGUI levelScoreText;
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private GameDataSO currentGameData;
        
        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;
        
        [Header("Broadcasting to")]
        [SerializeField] private LoadEventChannelSO _onLoadScene = default;
        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += Setup;
        }

        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= Setup;
        }

        private void Awake()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }


        private void Setup()
        {
            dolly.m_PathPosition = dollyPath.m_Waypoints[0].position.x;
            SetLevelData();
        }

        public void SetLevelData()
        {
            SerializedLevelScore serializedLevelScore = currentGameData.LevelScores.Find(x => (int)x.Level == currentLevelSelected);
            if (serializedLevelScore != null)
            {
                levelScoreText.text = GameManager.CalculateScore(serializedLevelScore.score);
            }
            else
            {
                levelScoreText.text = String.Empty;
            }

            levelIndexText.text = $"Level {currentLevelSelected + 1}";
        }

        public void Play()
        {
            _onLoadScene.RaiseEvent(GameManager.Instance.sceneSos[currentLevelSelected],true,true);
        }

        public void LeftButton()
        {
            Debug.Log("left");
            if (currentLevelSelected == 0)
            {
                currentLevelSelected = GameManager.Instance.sceneSos.Length - 1;
            }
            else
            {
                currentLevelSelected--;
            }
            dolly.m_PathPosition = dollyPath.m_Waypoints[currentLevelSelected].position.x;
            SetLevelData();
        }

        public void RightButton()
        {
            Debug.Log("right");
            if (currentLevelSelected == GameManager.Instance.sceneSos.Length - 1)
            {
                currentLevelSelected = 0;
            }
            else
            {
                currentLevelSelected++;
            }
            dolly.m_PathPosition = dollyPath.m_Waypoints[currentLevelSelected].position.x;
            SetLevelData();
        }
    }
}