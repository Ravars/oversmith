using System.Collections;
using System.Collections.Generic;
using Oversmith.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//namespace Oversmith.Scripts.Menu
//{
//	[RequireComponent(typeof(FadeUI))]
//	public class MainScreenScript : MonoBehaviour
//	{
//		private FadeUI _fade;
//		[SerializeField] private MainMenuScript mainMenuScript;
		
//		public void PlayButton()
//		{
//			mainMenuScript.LoadHomePageButton();
//		}

//		public void OptionsButton()
//		{
//			mainMenuScript.OpenConfigScreen();
//		}
		
		
		
//		public void QuitGame()
//		{
//			if (GameManager.InstanceExists)
//			{
//				GameManager.Instance.QuitGame();
//			}
//			else
//			{
//				Debug.LogError("Game Manager not instanced");
//			}
//		}
//	}
//}