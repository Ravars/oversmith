using System.Collections;
using System.Collections.Generic;
using Oversmith.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementInputSystem : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CharacterController _cc;
    private Quaternion _targetRotation;
    public float smoothing = 5f;
    private Vector2 _previousInput;
    private PlayerInput _playerInput;
    private PlayerInputAction _inputAction;
    public GameObject menu;

    void Start()
    {
        _cc = GetComponent<CharacterController>(); // Get the character controller component
        _playerInput = GetComponent<PlayerInput>();
        _inputAction = new PlayerInputAction();
    }

    private void SetMovement(Vector2 movement)
    {
        _previousInput = movement;
    }

    private void ResetMovement()
    {
        _previousInput = Vector2.zero;
    }

    void Update()
    {
        Vector3 movementDirection = new Vector3(_previousInput.x, 0, _previousInput.y);
        if (movementDirection.magnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(movementDirection);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
        movementDirection.Normalize();
        _cc.SimpleMove(movementDirection * moveSpeed);
    }

    void OnMove(InputValue value)
    {
        Vector2 vec = value.Get<Vector2>();
        _previousInput = vec;       
    }

    void OnUIEnter()
    {
        menu.SetActive(true);
        _playerInput.SwitchCurrentActionMap("UI");
    }
}
