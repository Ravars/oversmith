using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Ok 10/04/2023

namespace Oversmith.Scripts.Menu
{
	[RequireComponent(typeof(FadeUI))]
	public class StartScreenScript : MonoBehaviour
	{
		public PlayerInput input;
		public UnityEvent onContinue;
		private FadeUI _fade;
		[SerializeField] private MainMenuScript mainMenuScript;

		private void Awake()
		{
			_fade = GetComponent<FadeUI>();
		}

		// Start is called before the first frame update
		void Start()
		{
			EnablePress();
		}
		// Enable press any button after fading in
		public void EnablePress()
		{
			input.actions["StartUp"].performed += OnAnyPressed;
		}

		void OnDisable()
		{
			if (input != null)
			{
				input.actions["StartUp"].performed -= OnAnyPressed;
			}
		}

		// Called when any button is pressed in start screen
		private void OnAnyPressed(InputAction.CallbackContext ctx)
		{
			if (this != null)
			{
				Debug.Log("OnAnyPressed");
				_fade.BeginFadeOut();
			}
			ctx.action.performed -= OnAnyPressed;
		}
	}
}