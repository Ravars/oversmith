using System;
using TMPro;
using UnityEngine;

namespace MadSmith.Scripts.UI.SettingsScripts
{
    public class Settings : MonoBehaviour // Agora se chama UISettingsVideoComponent
    {
        [Header("DropDowns")] 
        public TMP_Dropdown resolutionDimension;
        public TMP_Dropdown display;

        private Resolution[] _storeResolutions;
        private FullScreenMode _screenMode;
        private int _countRes;
    
        [Obsolete("Obsolete")]
        void Start()
        {
            Resolution[] resolutions = Screen.resolutions;
            Array.Reverse(resolutions);
            _storeResolutions = new Resolution[resolutions.Length];
        
            ScreenInitialize();
            AddResolution(resolutions);
            ResolutionInitialize(_storeResolutions);
        
            display.onValueChanged.AddListener(delegate { ScreenOptions(display.options[display.value].text); });
            resolutionDimension.onValueChanged.AddListener(delegate{Screen.SetResolution(_storeResolutions[resolutionDimension.value].width,
                _storeResolutions[resolutionDimension.value].height, _screenMode);});
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
