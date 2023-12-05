using System;
using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Managers;
using MadSmith.Scripts.Multiplayer.Managers;
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

        // public void Play() // talvez adicionar aqui um Cmd
        // {
        //     OnLevelSelected?.Invoke();
        //     var manager = NetworkManager.singleton as MadSmithNetworkManager;
        //     // if (manager != null) manager.StartGame("Level01-1");
        //     if (manager != null)
        //     {
        //         manager.StartGame(GameManager.Instance.sceneSos[CurrentLevelSelected + 1].name);
        //     }
        //     // _onLoadScene.RaiseEvent(GameManager.Instance.sceneSos[currentLevelSelected],true);
        // }

        // public void LeftButton()
        // {
        //     //Debug.Log("LeftButton" + currentLevelSelected);
        //     if (CurrentLevelSelected == 0)
        //     {
        //         CurrentLevelSelected = GameManager.Instance.sceneSos.Length - 1;
        //     }
        //     else
        //     {
        //         CurrentLevelSelected--;
        //     }
        //     dolly.m_PathPosition = Mathf.Floor((int)(CurrentLevelSelected / 3));
        //     SetLevelData();
        // }

        public void UpdateLevelSelected(int levelSelected)
        {
            dolly.m_PathPosition = Mathf.Floor((int)(levelSelected / 3));
            SetLevelData(levelSelected);
        }



        // public void RightButton()
        // {
        //     //Debug.Log("RightButton" + currentLevelSelected + ", GameManager: " + GameManager.InstanceExists);
        //     if (CurrentLevelSelected == GameManager.Instance.sceneSos.Length - 1)
        //     {
        //         CurrentLevelSelected = 0;
        //     }
        //     else
        //     {
        //         CurrentLevelSelected++;
        //     }
        //
        //     //Debug.Log("dolly.m_PathPosition" + ReferenceEquals(dolly, null) + ":" + currentLevelSelected);
        //     dolly.m_PathPosition = Mathf.Floor((int)(CurrentLevelSelected / 3));
        //     //Debug.Log("dolly.m_PathPosition");
        //     SetLevelData();
        // }
    }
}