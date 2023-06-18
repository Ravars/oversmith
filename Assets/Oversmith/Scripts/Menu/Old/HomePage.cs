using System;
using Oversmith.Scripts.Managers;
using UnityEngine;

//namespace Oversmith.Scripts.Menu
//{
//    public enum ScreenType
//    {
//        Main,
//        MultiPlayer,
//        SinglePlayer,
//        LocalHost
//    }
//    public class HomePage : MonoBehaviour
//    {
//        public FadeUI mainScreen;
//        public FadeUI multiPlayerScreen;
//        public FadeUI singlePlayerScreen;
//        public FadeUI localHostScreen;
//        private ScreenType _currentScreen;
        
//        private void Start()
//        {
//            ChangeScreen(ScreenType.Main,true);
//        }

//        public void ChangeScreen(ScreenType newScreenType, bool ignoreFadeOut = false)
//        {
//            if (!ignoreFadeOut)
//            {
//                switch (_currentScreen)
//                {
//                    case ScreenType.Main:
//                        mainScreen.BeginFadeOut();
//                        break;
//                    case ScreenType.MultiPlayer:
//                        multiPlayerScreen.BeginFadeOut();
//                        break;
//                    case ScreenType.SinglePlayer:
//                        singlePlayerScreen.BeginFadeOut();
//                        break;
//                    case ScreenType.LocalHost:
//                        localHostScreen.BeginFadeOut();
//                        break;
//                }
//            }
            
//            switch (newScreenType)
//            {
//                case ScreenType.Main:
//                    mainScreen.gameObject.SetActive(true);
//                    mainScreen.BeginFadeIn();
//                    break;
//                case ScreenType.MultiPlayer:
//                    multiPlayerScreen.gameObject.SetActive(true);
//                    multiPlayerScreen.BeginFadeIn();
//                    break;
//                case ScreenType.SinglePlayer:
//                    singlePlayerScreen.gameObject.SetActive(true);
//                    singlePlayerScreen.BeginFadeIn();
//                    break;
//                case ScreenType.LocalHost:
//                    localHostScreen.gameObject.SetActive(true);
//                    localHostScreen.BeginFadeIn();
//                    break;
//            }
//        }

//        public void LoadLevel1()
//        {
//            if (GameManager.InstanceExists)
//            { 
//                GameManager.Instance.LoadGameLevel(1);
//            }
//        }
//    }
//}