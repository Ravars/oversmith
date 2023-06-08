using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Oversmith.Scripts.Systems.Settings
{
    public class UISettingsVideoComponent : MonoBehaviour
    {
        [SerializeField] private SettingsSO settingsSo;
        private int _width;
        private int _widthSaved;
        private int _height;
        private int _heightSaved;
        private string _displayTypeValue;
        private string _displayTypeValueSaved;

        [SerializeField] public GameObject popUp;


        [Header("Buttons")] 
        public Button buttonSave;
        public Button buttonApply;
        public Button buttonRevert;
        public int maximumPopUpTimer = 15;
        
        
        
        [Header("DropDowns")] 
        public TMP_Dropdown resolutionDimension;

        public TMP_Dropdown display;
        private int _countRes;
        private FullScreenMode _screenMode;

        private Resolution[] _storeResolutions;
        public event UnityAction<string, int, int> _save = delegate { };

        [Obsolete("Obsolete")]
        void Start()
        {
            Resolution[] resolutions = Screen.resolutions;
            Array.Reverse(resolutions);
            _storeResolutions = new Resolution[resolutions.Length];
        
            ScreenInitialize();
            AddResolution(resolutions);
            ResolutionInitialize(_storeResolutions);
        }

        private void OnEnable()
        {
            _width = _width = settingsSo.WidthValue;
            _height = _height = settingsSo.HeightValue;
            _displayTypeValue = _displayTypeValue = settingsSo.DisplayTypeValue;
            // popUp.SetActive(false);
            
            display.onValueChanged.AddListener(delegate { ScreenOptions(display.options[display.value].text); });
            resolutionDimension.onValueChanged.AddListener(delegate{Screen.SetResolution(_storeResolutions[resolutionDimension.value].width,
                _storeResolutions[resolutionDimension.value].height, _screenMode);});
        }

        private void OnDisable()
        {
            display.onValueChanged.RemoveAllListeners();
            resolutionDimension.onValueChanged.RemoveAllListeners();
            // popUp.SetActive(false);
        }

        public void Save()
        {
            _save?.Invoke(_displayTypeValue, _width,_height);
        }

        public void Reset()
        {
            _width = _widthSaved;
            _height= _heightSaved;
            _displayTypeValue= _displayTypeValueSaved;
        }

        public void PopUpHandler()
        {
            
        }
        

        #region Resolution and Display

        [Obsolete("Obsolete")]
        private void AddResolution(Resolution[] res)
        {
            _countRes = 0;
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i].refreshRate == Screen.currentResolution.refreshRate)
                {
                    _storeResolutions[_countRes] = res[i];
                    _countRes++;
                }
            }

            for (int i = 0; i < _countRes; i++)
            {
                resolutionDimension.options.Add(new TMP_Dropdown.OptionData(ResolutionToString(_storeResolutions[i])));
            }
        }

        private void ScreenOptions(string mode)
        {
            switch (mode)
            {
                case "Full Screen":
                    _screenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case "Windowed":
                    _screenMode = FullScreenMode.Windowed;
                    break;
                default:
                    _screenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
            Screen.fullScreenMode = _screenMode;
        }

        private void ResolutionInitialize(Resolution[] res)
        {
            for (int i = 0; i < res.Length; i++)
            {
                if (Screen.width == res[i].width && Screen.height == res[i].height)
                {
                    resolutionDimension.value = i;
                }
            }
            resolutionDimension.RefreshShownValue();
        }

        private void ScreenInitialize()
        {
            switch (Screen.fullScreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    display.value = 0;
                    _screenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case FullScreenMode.Windowed:
                    display.value = 1;
                    _screenMode = FullScreenMode.Windowed;
                    break;
                default:
                    display.value = 2;
                    _screenMode = FullScreenMode.FullScreenWindow;
                    break;
            }  
        }

        private string ResolutionToString(Resolution screenRes)
        {
            return screenRes.width + " x " + screenRes.height;
        }

        #endregion
    }
}