using System;
using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.SavingSystem;
using MadSmith.Scripts.Systems.Settings;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        [SerializeField] private InputReader _inputReader;
        public UnityAction OnCloseLevelSelection;
        public UnityAction OnLevelSelected;

        [SerializeField] private Button levelSelectButton;
        [Header("Broadcasting to")]
        [SerializeField] private LoadEventChannelSO _onLoadScene = default;

        [SerializeField] private Sprite[] levelImages;
        [SerializeField] private string[] levelNames;
        [SerializeField] private Image levelImage;
        private void Awake()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseLevelSelection;
            Setup();
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseLevelSelection;
        }

        public void CloseLevelSelection()
        {
            OnCloseLevelSelection?.Invoke();
        }


        private void Setup()
        {
            dolly.m_PathPosition = dollyPath.m_Waypoints[0].position.x;
            SetLevelData();
        }

        public void SetLevelData()
        {
            SerializedLevelScore serializedLevelScore = currentGameData.LevelScores.Find(x => (int)x.Level == currentLevelSelected);
            // if (serializedLevelScore != null)
            // {
            //     levelScoreText.text = GameManager.CalculateScore(serializedLevelScore.score);
            // }
            // else
            // {
            //     levelScoreText.text = String.Empty;
            // }

            levelImage.sprite = levelImages[currentLevelSelected];
            levelIndexText.text = levelNames[currentLevelSelected];
        }

        public void Play() // talvez adicionar aqui um Cmd
        {
            OnLevelSelected?.Invoke();
            _onLoadScene.RaiseEvent(GameManager.Instance.sceneSos[currentLevelSelected],true);
        }

        public void LeftButton()
        {
            if (currentLevelSelected == 0)
            {
                currentLevelSelected = GameManager.Instance.sceneSos.Length - 1;
            }
            else
            {
                currentLevelSelected--;
            }
            dolly.m_PathPosition = Mathf.Floor((int)(currentLevelSelected / 3));
            SetLevelData();
        }



        public void RightButton()
        {
            if (currentLevelSelected == GameManager.Instance.sceneSos.Length - 1)
            {
                currentLevelSelected = 0;
            }
            else
            {
                currentLevelSelected++;
            }
            
            dolly.m_PathPosition = Mathf.Floor((int)(currentLevelSelected / 3));
            SetLevelData();
        }
    }
}