using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Menu
{
	public class MainMenuScript : MonoBehaviour
	{
		public GameObject startScr;
		public GameObject menuScr;
		public GameObject configScr;

		private PlayerInput input;
		private GameObject curScr;
		[Space(10)]
		public LevelNames NewGameLevel;

		// Start is called before the first frame update
		void Start()
		{
			input = GetComponent<PlayerInput>();
			curScr = menuScr;
			curScr.SetActive(true);
		}

		public void LoadNewGame()
		{
			SceneManager.LoadScene((int)NewGameLevel);
		}

		public void ReturnToMainScreen()
		{
			curScr.SetActive(false);
			curScr = menuScr;
			menuScr.SetActive(true);
		}

		public void SwitchToConfigScreen()
		{
			curScr.SetActive(false);
			curScr = configScr;
			configScr.SetActive(true);
		}
	}
}