using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MainMenuScript : MonoBehaviour
{
	delegate void Ends();

	public GameObject startScr;
	public GameObject menuScr;

	private PlayerInput input;

	// Start is called before the first frame update
	void Start()
	{
		input = GetComponent<PlayerInput>();
		startScr.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void SwitchToMainScreen()
    {
		startScr.SetActive(false);
		menuScr.SetActive(true);
    }

	public void LoadNewGame()
    {

    }
}
