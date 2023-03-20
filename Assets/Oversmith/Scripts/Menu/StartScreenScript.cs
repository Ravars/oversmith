using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Oversmith.Scripts.Menu
{
	public class StartScreenScript : MonoBehaviour
	{
		public PlayerInput input;
		public UnityEvent onContinue;

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

		// Start is called before the first frame update
		void Start()
		{
			input.SwitchCurrentActionMap("StartUp");
		}

		void OnEnable()
		{
			Fade.BeginFadeIn();
		}

		void OnDisable()
		{
			if (input != null)
			{
				input.actions["StartUp"].performed -= OnAnyPressed;
			}
		}

		// Called when any button is pressed in start screen
		public void OnAnyPressed(InputAction.CallbackContext ctx)
		{
			if (this != null)
			{
				Fade.BeginFadeOut();
			}
			ctx.action.performed -= OnAnyPressed;
		}

		// Enable press any button after fading in
		public void EnablePress()
		{
			input.actions["StartUp"].performed += OnAnyPressed;
		}

		// Go to menu
		public void Continue()
		{
			if (onContinue != null)
			{
				onContinue.Invoke();
			}
		}
	}
}