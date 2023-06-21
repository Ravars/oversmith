using System;
using System.Collections.Generic;
using Oversmith.Scripts.UI;
using Oversmith.Scripts.UI.SettingsScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Oversmith.Scripts.Systems.Settings
{
    struct GraphicsSettings
    {
        public int Resolution;
        public int AntiAliasing;
        public int ShadowDistance;
        

    }
    
    public class UISettingsGraphicsComponent : MonoBehaviour
    {
        [SerializeField] private UISettingItemFiller resolutionField;
        [SerializeField] private UISettingItemFiller _fullscreenField = default;
        [SerializeField] private UISettingItemFiller _antiAliasingField = default;
        [SerializeField] private UISettingItemFiller _graphicsQualityField = default;
        
        [SerializeField] private List<Resolution> resolutionsList;
        private Resolution _currentResolution;
        [SerializeField] private UniversalRenderPipelineAsset _uRPAsset = default;
        
        private bool _isFullscreen;
        private bool _savedFullscreenState;
        
        private int _currentGraphicsQualityIndex;
        private int _savedGraphicsQualityIndex;
        
        private int _currentResolutionIndex = default;
        private int _savedResolutionIndex = default;
        
        private int _currentAntiAliasingIndex = default;
        private int _savedAntiAliasingIndex = default;
        private List<string> _currentAntiAliasingList = default;
        
        public event UnityAction<int, int, int, bool> _save = delegate { };
        [SerializeField] private UIGenericButton _saveButton;
        [SerializeField] private UIGenericButton _resetButton;
        
        private void OnEnable()
        {
            resolutionField.OnNextOption += NextResolution;
            resolutionField.OnPreviousOption += PreviousResolution;
            
            _fullscreenField.OnNextOption += NextFullscreenState;
            _fullscreenField.OnPreviousOption += PreviousFullscreenState;
            
            // _antiAliasingField.OnNextOption += NextAntiAliasingTier;
            // _antiAliasingField.OnPreviousOption += PreviousAntiAliasingTier;
            
            _graphicsQualityField.OnNextOption += NextGraphicsState;
            _graphicsQualityField.OnPreviousOption += PreviousGraphicsState;
            
            _saveButton.Clicked += SaveSettings;
            _resetButton.Clicked += ResetSettings;
            GetCurrentGraphicsState();
        }

        private void OnDisable()
        {
            resolutionField.OnNextOption -= NextResolution;
            resolutionField.OnPreviousOption -= PreviousResolution;
            
            _fullscreenField.OnNextOption -= NextFullscreenState;
            _fullscreenField.OnPreviousOption -= PreviousFullscreenState;
            
            _antiAliasingField.OnNextOption -= NextAntiAliasingTier;
            _antiAliasingField.OnPreviousOption -= PreviousAntiAliasingTier;
            
            _saveButton.Clicked -= SaveSettings;
            _resetButton.Clicked -= ResetSettings;
        }
        public void Init()
        {
            resolutionsList = GetResolutionsList();
            // _currentShadowDistanceTier = GetCurrentShadowDistanceTier();
            
            _currentAntiAliasingList = GetDropdownData(Enum.GetNames(typeof(MsaaQuality)));

            _currentResolution = Screen.currentResolution;
            _currentResolutionIndex = GetCurrentResolutionIndex();
            _isFullscreen = GetCurrentFullscreenState();
            _currentGraphicsQualityIndex = GetCurrentGraphicsState();
            _currentAntiAliasingIndex = GetCurrentAntialiasing();

            _savedResolutionIndex = _currentResolutionIndex;
            _savedAntiAliasingIndex = _currentAntiAliasingIndex;
            _savedGraphicsQualityIndex = _currentGraphicsQualityIndex;
            // _savedShadowDistanceTier = _currentShadowDistanceTier;
            _savedFullscreenState = _isFullscreen;
        }

        public void Setup()
        {
            Init();
            SetResolutionField();
            SetFullscreen();
            SetAntiAliasingField();
            SetGraphicsField();
        }
        public void SaveSettings()
        {
            _savedResolutionIndex = _currentResolutionIndex;
            _savedAntiAliasingIndex = _currentAntiAliasingIndex;
            _savedGraphicsQualityIndex = _currentGraphicsQualityIndex;
            // _savedShadowDistanceTier = _currentShadowDistanceTier;
            _savedFullscreenState = _isFullscreen;
            // float shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
            // _save.Invoke(_currentResolutionIndex, _currentAntiAliasingIndex, shadowDistance, _isFullscreen);
            _save.Invoke(_currentResolutionIndex, _currentAntiAliasingIndex, _currentGraphicsQualityIndex, _isFullscreen);
        }
        public void ResetSettings()
        {
            _currentResolutionIndex = _savedResolutionIndex;
            OnResolutionChange();
            _currentAntiAliasingIndex = _savedAntiAliasingIndex;
            OnAntiAliasingChange();
            // _currentShadowDistanceTier = _savedShadowDistanceTier;
            // OnShadowDistanceChange();
            _isFullscreen = _savedFullscreenState;
            OnFullscreenChange();

            _currentGraphicsQualityIndex = _savedGraphicsQualityIndex;
            OnGraphicsChange();
        }
        
        private List<string> GetDropdownData(string[] optionNames, params string[] customOptions)
        {
            List<string> options = new List<string>();
            foreach (string option in optionNames)
            {
                options.Add(option);
            }

            foreach (string option in customOptions)
            {
                options.Add(option);
            }
            return options;
        }

        #region Fullscreen

        private bool GetCurrentFullscreenState()
        {
            return Screen.fullScreen;
        }

        private void SetFullscreen()
        {
            if (_isFullscreen)
            {
                _fullscreenField.FillSettingField_Localized(2, 1, "On");
            }
            else
            {
                _fullscreenField.FillSettingField_Localized(2, 0, "Off");
            }
        }

        private void NextFullscreenState()
        {
            _isFullscreen = true;
            OnFullscreenChange();
        }

        private void PreviousFullscreenState()
        {
            _isFullscreen = false;
            OnFullscreenChange();
        }

        private void OnFullscreenChange()
        {
            Screen.fullScreen = _isFullscreen;
            SetFullscreen();
        }
        

        #endregion

        #region Resolution
        private void NextResolution()
        {
            Debug.Log("next");
            _currentResolutionIndex++;
            _currentResolutionIndex = Mathf.Clamp(_currentResolutionIndex, 0, resolutionsList.Count - 1);
            OnResolutionChange();
        }

        private void PreviousResolution()
        {
            _currentResolutionIndex--;
            _currentResolutionIndex = Mathf.Clamp(_currentResolutionIndex, 0, resolutionsList.Count - 1);
            OnResolutionChange();
        }

        private List<Resolution> GetResolutionsList()
        {
            List<Resolution> options = new List<Resolution>();
            foreach (var resolution in Screen.resolutions)
            {
                options.Add(resolution);
            }

            return options;
        }
        int GetCurrentResolutionIndex()
        {
            if (resolutionsList == null)
            { resolutionsList = GetResolutionsList(); }
            int index = resolutionsList.FindIndex(o => o.width == _currentResolution.width && o.height == _currentResolution.height);
            return index;
        }
        private void OnResolutionChange()
        {
            _currentResolution = resolutionsList[_currentResolutionIndex];
            Screen.SetResolution(_currentResolution.width, _currentResolution.height, _isFullscreen);
            SetResolutionField();
        }

        private void SetResolutionField()
        {
            string displayText = resolutionsList[_currentResolutionIndex].ToString();
            Debug.Log(displayText);
            resolutionField.FillSettingField(resolutionsList.Count, _currentResolutionIndex, displayText);
        }
        #endregion
        #region Anti Aliasing
        void SetAntiAliasingField()
        {
            Debug.Log(_currentAntiAliasingIndex);
            _currentAntiAliasingIndex = 0;
            string optionDisplay = _currentAntiAliasingList[_currentAntiAliasingIndex].Replace("_", "");
            _antiAliasingField.FillSettingField(_currentAntiAliasingList.Count, _currentAntiAliasingIndex, optionDisplay);

        }
        int GetCurrentAntialiasing()
        {
            return _uRPAsset.msaaSampleCount;

        }
        void NextAntiAliasingTier()
        {
            _currentAntiAliasingIndex++;
            _currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
            OnAntiAliasingChange();
        }
        void PreviousAntiAliasingTier()
        {
            _currentAntiAliasingIndex--;
            _currentAntiAliasingIndex = Mathf.Clamp(_currentAntiAliasingIndex, 0, _currentAntiAliasingList.Count - 1);
            OnAntiAliasingChange();
        }

        void OnAntiAliasingChange()
        {
            _uRPAsset.msaaSampleCount = _currentAntiAliasingIndex;
            SetAntiAliasingField();

        }
        #endregion

        #region Graphics

        private int GetCurrentGraphicsState()
        {
            return QualitySettings.GetQualityLevel();
        }
        private void OnGraphicsChange()
        {
            QualitySettings.SetQualityLevel(_currentGraphicsQualityIndex);
            SetGraphicsField();
        }
        private void SetGraphicsField()
        {
            string qualityLevel = String.Empty;
            switch (_currentGraphicsQualityIndex)
            {
                case 0:
                    qualityLevel = "Low";
                    break;
                case 1:
                    qualityLevel = "Medium";
                    break;
                case 2:
                    qualityLevel = "High";
                    break;
            }
            
            _graphicsQualityField.FillSettingField_Localized(3, _currentGraphicsQualityIndex, qualityLevel);
        }
        private void NextGraphicsState()
        {
            _currentGraphicsQualityIndex++;
            _currentGraphicsQualityIndex = Mathf.Clamp(_currentGraphicsQualityIndex, 0, 2);
            OnGraphicsChange();
        }

        private void PreviousGraphicsState()
        {
            _currentGraphicsQualityIndex--;
            _currentGraphicsQualityIndex = Mathf.Clamp(_currentGraphicsQualityIndex, 0, 2);
            OnGraphicsChange();
        }

        #endregion
    }
}