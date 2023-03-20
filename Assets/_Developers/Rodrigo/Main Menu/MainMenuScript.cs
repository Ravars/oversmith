using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace Oversmith.Script.Menu
{
	public class MainMenuScript : MonoBehaviour
	{
		//delegate void Ends();

		public GameObject startScr;
		public GameObject menuScr;
		public GameObject configScr;

		private PlayerInput input;
		private GameObject curScr;
		[Space(10)]
		public string newGameSceneName;

		// Start is called before the first frame update
		void Start()
		{
			input = GetComponent<PlayerInput>();
			curScr = menuScr;
			curScr.SetActive(true);
		}

		public void LoadNewGame()
		{
			SceneManager.LoadScene(newGameSceneName);
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