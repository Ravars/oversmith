using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UITutorial : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera camera;
        private CinemachineTrackedDolly dolly;
        [SerializeField] private CinemachineSmoothPath dollyPath;
        [SerializeField] private int currentTutorialIndex = 0;
        [Header("Listening on")] 
        [SerializeField] private VoidEventChannelSO _onSceneReady = default;

        [SerializeField] private LocalizeStringEvent text;

        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        [SerializeField] private GameSceneSO mainMenu;
        
        [Header("Broadcasting to")]
        [SerializeField] private LoadEventChannelSO _onLoadScene = default; //TODO: remove this and change this scene to MainMenu
        
        private void Awake()
        {
            dolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }
        private void OnEnable()
        {
            _onSceneReady.OnEventRaised += Setup;
        }

        private void OnDisable()
        {
            _onSceneReady.OnEventRaised -= Setup;
        }

        private void Setup()
        {
            dolly.m_PathPosition = 0;
            SetData();
        }

        private void SetData()
        {
            leftButton.gameObject.SetActive(currentTutorialIndex != 0);
            rightButton.gameObject.SetActive(currentTutorialIndex != dollyPath.m_Waypoints.Length - 1);
            text.StringReference.TableEntryReference = "Tutorial_" + currentTutorialIndex;
        }

        public void LeftButton()
        {
            if (currentTutorialIndex == 0)
            {
                currentTutorialIndex = dollyPath.m_Waypoints.Length - 1;
            }
            else
            {
                currentTutorialIndex--;
            }
            dolly.m_PathPosition = currentTutorialIndex;
            SetData();
        }

        public void RightButton()
        {
            if (currentTutorialIndex == dollyPath.m_Waypoints.Length - 1)
            {
                currentTutorialIndex = 0;
            }
            else
            {
                currentTutorialIndex++;
            }
            dolly.m_PathPosition = currentTutorialIndex;
            SetData();
        }

        public void BackToMenuButton()
        {
            _onLoadScene.RaiseEvent(mainMenu);
        }
    }
}