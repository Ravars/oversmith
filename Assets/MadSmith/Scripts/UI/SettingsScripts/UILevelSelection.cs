using System;
using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.Multiplayer.UI;
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
        // [field: SerializeField] public int CurrentLevelSelected { get; private set; } = 0;

        [SerializeField] private TextMeshProUGUI levelScoreText;
        [SerializeField] private TextMeshProUGUI levelIndexText;
        [SerializeField] private GameDataSO currentGameData;

        [SerializeField] private InputReader _inputReader;
        public UnityAction OnCloseLevelSelection;
        public UnityAction OnLevelSelected;

        [SerializeField] private Button levelSelectButton;
        private MadSmithNetworkRoomPlayer _localMadSmithNetworkRoomPlayer;
        public MadSmithNetworkRoomPlayer LocalPlayer
        { get
            {
                if (!ReferenceEquals(_localMadSmithNetworkRoomPlayer, null)) return _localMadSmithNetworkRoomPlayer;
                return _localMadSmithNetworkRoomPlayer = UiRoomManager.Instance.LocalClient;
            }
        }
        
        [Header("Listening to")]
        [SerializeField] private IntEventChannelSO onUpdateLevel = default;
        // [SerializeField] private LoadEventChannelSO _onLoadScene = default;

        [SerializeField] private Sprite[] levelImages;
        [SerializeField] private string[] levelNames;
        [SerializeField] private Image levelImage;
        private void Awake()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }
        public void Setup(MadSmithNetworkRoomPlayer localClient)
        {
            _localMadSmithNetworkRoomPlayer = localClient;
        }

        private void OnEnable()
        {
            _inputReader.MenuCloseEvent += CloseLevelSelection;
            onUpdateLevel.OnEventRaised += OnEventRaised;
            Setup();
        }

        private void OnEventRaised(int levelIndex)
        {
            UpdateLevelSelected(levelIndex);
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseLevelSelection;
            onUpdateLevel.OnEventRaised -= OnEventRaised;
        }

        public void CloseLevelSelection()
        {
            OnCloseLevelSelection?.Invoke();
        }


        private void Setup()
        {
            dolly.m_PathPosition = dollyPath.m_Waypoints[0].position.x;
            SetLevelData(0);
        }

        public void SetLevelData(int levelSelected)
        {
            //Debug.Log("SetLevelData");
            SerializedLevelScore serializedLevelScore = currentGameData.LevelScores.Find(x => (int)x.Level == levelSelected);
            // if (serializedLevelScore != null)
            // {
            //     levelScoreText.text = GameManager.CalculateScore(serializedLevelScore.score);
            // }
            // else
            // {
            //     levelScoreText.text = String.Empty;
            // }

            levelImage.sprite = levelImages[levelSelected];
            levelIndexText.text = levelNames[levelSelected];
        }

        public void UpdateLevelSelected(int levelSelected)
        {
            dolly.m_PathPosition = Mathf.Floor((int)(levelSelected / 3));
            SetLevelData(levelSelected);
        }
        public void NextLevel()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.NextLevel();
            }
        }
        public void PreviousLevel()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.PreviousLevel();
            }
        }

        public void SelectLevel()
        {
            if (!ReferenceEquals(LocalPlayer, null))
            {
                LocalPlayer.PlayGameButton();
            }
        }
    }
}