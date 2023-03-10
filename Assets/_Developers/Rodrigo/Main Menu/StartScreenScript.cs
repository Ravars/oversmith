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

	// Update is called once per frame
	void Update()
	{
		
	}

	void OnEnable()
	{
		fade = GetComponent<FadeUI>();
		fade.BeginFadeIn();
	}

	void OnDisable()
    {
		input.actions["StartUp"].performed -= OnAnyPressed;
    }

	public void OnAnyPressed(InputAction.CallbackContext ctx)
    {
		fade = GetComponent<FadeUI>();
		fade.BeginFadeOut();
    }

	public void EnablePress()
    {
		input.actions["StartUp"].performed += OnAnyPressed;
    }

	public void Continue()
    {
		if (onContinue != null)
        {
			onContinue.Invoke();
        }
    }
}
