using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Oversmith.Scripts.Menu
{
	public class MainScreenScript : MonoBehaviour
	{
		public PlayerInput input;
		public Button initBtt;
		private FadeUI fade;
		private FadeUI Fade
		{
			get
			{
				if (fade == null)
					fade = GetComponent<FadeUI>();
				return fade;
			}
		}
		[Space(10)]

		public UnityEvent OnNewGameClick;
		public UnityEvent OnOptionClick;

		void OnEnable()
		{
			input.SwitchCurrentActionMap("UI");
			initBtt.Select();
			Fade.FadeIn();
		}

		public void OnClickPlay()
		{
			input.DeactivateInput();
			Fade.FadeOut(OnNewGameClick.Invoke);
		}

		public void OnClickOption()
		{
			Fade.FadeOut(OnOptionClick.Invoke);
		}

		public void QuitGame()
		{
			Debug.Log("Quit game");
			Application.Quit();
		}
	}
}