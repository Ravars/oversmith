using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.SceneManagement.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UITutorial : MonoBehaviour
    {
        private CinemachineTrackedDolly dolly;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineSmoothPath dollyPath;
        [SerializeField] private int currentTutorialIndex = 0;
        [SerializeField] private LocalizeStringEvent text;
        
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        
        [SerializeField] private InputReader _inputReader;
        public UnityAction OnCloseTutorial;
        
        
        private void Awake()
        {
            dolly = virtualCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }
        private void OnEnable()
        {
            Setup();
            _inputReader.MenuCloseEvent += CloseTutorialScreen;
        }

        private void OnDisable()
        {
            _inputReader.MenuCloseEvent -= CloseTutorialScreen;
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

        public void CloseTutorialScreen()
        {
            OnCloseTutorial?.Invoke();
            // _onLoadScene.RaiseEvent(mainMenu, true, true);
        }
    }
}