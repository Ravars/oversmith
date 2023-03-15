using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class StartScreenScript : MonoBehaviour
{
	public PlayerInput input;
	public UnityEvent onContinue;

	private FadeUI fade;

	// Start is called before the first frame update
	void Start()
	{
		input.SwitchCurrentActionMap("StartUp");
	}

	void OnEnable()
	{
		fade = GetComponent<FadeUI>();
		fade.BeginFadeIn();
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
			fade = GetComponent<FadeUI>();
			fade.BeginFadeOut();
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
