using System;
using Cinemachine;
using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Managers;
using UnityEngine;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class UILevelSelection : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera camera;
        private CinemachineTrackedDolly dolly;
        [SerializeField] private CinemachineSmoothPath dollyPath;
        [SerializeField] private int currentLevelSelected = 0;
        
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
            dolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        }


        private void Setup()
        {
            
            dolly.m_PathPosition = dollyPath.m_Waypoints[0].position.x;
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
        }
    }
}