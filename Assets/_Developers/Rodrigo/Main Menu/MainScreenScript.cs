using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreenScript : MonoBehaviour
{
    public PlayerInput input;
    public Button initBtt;
    private FadeUI fade;
    [Space(10)]

    public string newGameSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable ()
    {
        input.SwitchCurrentActionMap("UI");
        fade = GetComponent<FadeUI>();
        fade.BeginFadeIn();
    }

    public void OnFadedIn()
    {
        initBtt.Select();
    }

    public void OnClickPlay()
    {
        input.DeactivateInput();
        fade = GetComponent<FadeUI>();
        fade.BeginFadeOut();
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(newGameSceneName);
    }
}
