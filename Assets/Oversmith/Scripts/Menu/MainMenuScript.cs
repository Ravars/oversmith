using System.Collections;
using System.Collections.Generic;
using Oversmith.Scripts.Managers;
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
		private GameObject curScr;

		// Start is called before the first frame update
		void Start()
		{
			curScr = startScr;
			curScr.SetActive(true);
		}

		public void LoadHomePage()
		{
			GameManager.Instance.LoadHomePage();
		}

		public void ReturnToMainScreen()
		{
			startScr.SetActive(false);
			curScr.SetActive(false);
			curScr = menuScr;
			menuScr.SetActive(true);
		}

		public void SwitchToConfigScreen()
		{
			startScr.SetActive(false);
			curScr.SetActive(false);
			curScr = configScr;
			configScr.SetActive(true);
		}
	}
}