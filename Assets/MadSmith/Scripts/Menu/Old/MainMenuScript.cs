using System;
using System.Collections;
using System.Collections.Generic;
using MadSmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//namespace Oversmith.Scripts.Menu
//{
//	public enum MainMenuCanvas
//	{
//		StartScreen,
//		MenuScreen,
//		ConfigScreen
//	}
	
//	public class MainMenuScript : MonoBehaviour
//	{
//		public FadeUI startScr;
//		public FadeUI menuScr;
//		public FadeUI configScr;
//		private MainMenuCanvas _currentScreen;
		
//		// public PlayerInput input;
//		public Button initBtt;

//		// Start is called before the first frame update
//		void Start()
//		{
//			ChangeScreen(MainMenuCanvas.StartScreen);
//			initBtt.Select();
//		}

//		public void LoadHomePageButton()
//		{
//			GameManager.Instance.LoadHomePage();
//		}

//		public void OpenMainScreen()
//		{
//			ChangeScreen(MainMenuCanvas.MenuScreen);
//		}

//		public void OpenConfigScreen()
//		{
//			ChangeScreen(MainMenuCanvas.ConfigScreen);
//		}

//		private void ChangeScreen(MainMenuCanvas mainMenuCanvas)
//		{
//			UnableAll();
//			_currentScreen = mainMenuCanvas;
//			switch (_currentScreen)
//			{
//				case MainMenuCanvas.StartScreen:
//					startScr.gameObject.SetActive(true);
//					startScr.BeginFadeIn();
//					break;
//				case MainMenuCanvas.MenuScreen:
//					menuScr.gameObject.SetActive(true);
//					menuScr.BeginFadeIn();
//					break;
//				case MainMenuCanvas.ConfigScreen:
//					configScr.gameObject.SetActive(true);
//					configScr.BeginFadeIn();
//					break;
//				default:
//					throw new ArgumentOutOfRangeException(nameof(mainMenuCanvas), mainMenuCanvas, null);
//			}
//		}

//		private void UnableAll()
//		{
//			startScr.gameObject.SetActive(false);
//			menuScr.gameObject.SetActive(false); 
//			configScr.gameObject.SetActive(false);
//		}
//	}
//}