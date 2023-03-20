using UnityEngine;
using UnityEngine.InputSystem;

public class UIInpuSystemTest : MonoBehaviour
{
    public GameObject menu;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if (player != null)
        {
            PlayerInput _playerInput = player.GetComponent<PlayerInput>();
            _playerInput.actions["UIExit"].performed += OnUIExit;
        }
    }

    void OnDisable()
    {
        if (player != null)
        {
            PlayerInput _playerInput = player.GetComponent<PlayerInput>();
            _playerInput.actions["UIExit"].performed -= OnUIExit;
        }
    }

    void OnUIExit(InputAction.CallbackContext ctx)
    {
        PlayerInput _playerInput = player.GetComponent<PlayerInput>();
        _playerInput.SwitchCurrentActionMap("Player");
        menu.SetActive(false);
    }
}
