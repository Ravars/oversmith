using System;
using System.Collections.Generic;
using Oversmith.Scripts.UI;
using Oversmith.Scripts.UI.SettingsScripts;
using UnityEngine;
using UnityEngine.Events;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsGraphicsComponent : MonoBehaviour
    {
        [SerializeField] private UISettingItemFiller resolutionField;
        [SerializeField] private UISettingItemFiller _fullscreenField = default;
        
        [SerializeField] private List<Resolution> resolutionsList;
        private Resolution _currentResolution;
        
        private bool _isFullscreen;
        private bool _savedFullscreenState;
        private int _currentResolutionIndex = default;
        private int _savedResolutionIndex = default;
        
        public event UnityAction<int, int, float, bool> _save = delegate { };
        [SerializeField] private UIGenericButton _saveButton;
        [SerializeField] private UIGenericButton _resetButton;
        
        private void OnEnable()
        {
            resolutionField.OnNextOption += NextResolution;
            resolutionField.OnPreviousOption += PreviousResolution;
            
            _fullscreenField.OnNextOption += NextFullscreenState;
            _fullscreenField.OnPreviousOption += PreviousFullscreenState;
            
            _saveButton.Clicked += SaveSettings;
            _resetButton.Clicked += ResetSettings;
        }

        private void OnDisable()
        {
            resolutionField.OnNextOption -= NextResolution;
            resolutionField.OnPreviousOption -= PreviousResolution;
            
            _fullscreenField.OnNextOption -= NextFullscreenState;
            _fullscreenField.OnPreviousOption -= PreviousFullscreenState;
            
            _saveButton.Clicked -= SaveSettings;
            _resetButton.Clicked -= ResetSettings;
        }
        public void Init()
        {
            resolutionsList = GetResolutionsList();
            // _currentShadowDistanceTier = GetCurrentShadowDistanceTier();
            // _currentAntiAliasingList = GetDropdownData(Enum.GetNames(typeof(MsaaQuality)));

            _currentResolution = Screen.currentResolution;
            _currentResolutionIndex = GetCurrentResolutionIndex();
            _isFullscreen = GetCurrentFullscreenState();
            // _currentAntiAliasingIndex = GetCurrentAntialiasing();

            _savedResolutionIndex = _currentResolutionIndex;
            // _savedAntiAliasingIndex = _currentAntiAliasingIndex;
            // _savedShadowDistanceTier = _currentShadowDistanceTier;
            _savedFullscreenState = _isFullscreen;
        }

        public void Setup()
        {
            Init();
            SetResolutionField();
            SetFullscreen();
        }
        public void SaveSettings()
        {
            _savedResolutionIndex = _currentResolutionIndex;
            // _savedAntiAliasingIndex = _currentAntiAliasingIndex;
            // _savedShadowDistanceTier = _currentShadowDistanceTier;
            _savedFullscreenState = _isFullscreen;
            // float shadowDistance = _shadowDistanceTierList[_currentShadowDistanceTier].Distance;
            // _save.Invoke(_currentResolutionIndex, _currentAntiAliasingIndex, shadowDistance, _isFullscreen);
            _save.Invoke(_currentResolutionIndex, 0, 0, _isFullscreen);
        }
        public void ResetSettings()
        {
            _currentResolutionIndex = _savedResolutionIndex;
            OnResolutionChange();
            // _currentAntiAliasingIndex = _savedAntiAliasingIndex;
            // OnAntiAliasingChange();
            // _currentShadowDistanceTier = _savedShadowDistanceTier;
            // OnShadowDistanceChange();
            _isFullscreen = _savedFullscreenState;
            OnFullscreenChange();
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
    }
}