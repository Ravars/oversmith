using _Developers.Vitor;
using Oversmith.Scripts;
using System.Collections;
using Test1.Scripts.Prototype;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = .2f;
    [SerializeField] private float dashCooldown = 1f;
    private CharacterController _cc;
    private Quaternion _targetRotation;
    public float smoothing = 5f;
    private Vector2 _previousInput;
    private Vector3 _dashDirection;
    private bool _isDashing = false;
    private bool _canDash = true;
    [SerializeField] private Animator _animator; // temporary
    [SerializeField] private PlayerInteractions _playerInteractions; // temporary

    void Start()
    {
        _cc = GetComponent<CharacterController>(); // Get the character controller component
        InputManager.Controls.Gameplay.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Gameplay.Move.canceled += ctx => ResetMovement();
        InputManager.Controls.Gameplay.Dash.performed += DashOnPerformed;
    }
    private void SetMovement(Vector2 movement)
    {
        _previousInput = movement;
    }

    private void ResetMovement()
    {
        _previousInput = Vector2.zero;
    }

    private void DashOnPerformed(InputAction.CallbackContext obj)
    {
        if (!_canDash || _dashDirection == Vector3.zero)
            return;

        _canDash = false;
        _isDashing = true;
        transform.rotation = Quaternion.LookRotation(_dashDirection);
        StartCoroutine(DashTimer(dashTime));
    }

    void Update()
    {
        if (_isDashing)
        {
            _cc.SimpleMove(_dashDirection * dashSpeed);
            //_animator.SetBool("Dash", true);
            return;
        }
        
        Vector3 movementDirection = new Vector3(_previousInput.x, 0, _previousInput.y);
        _dashDirection = movementDirection;
        if (movementDirection.magnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(movementDirection);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
        movementDirection.Normalize();
        
        _cc.SimpleMove(movementDirection * moveSpeed);
        
        
        
        //_animator.SetBool("Run",movementDirection.magnitude > 0f);
        //_animator.SetBool("Carry",!ReferenceEquals(_playerInteractions.ItemScript,null));
    }

    IEnumerator DashTimer(float dashTimer)
    {
        yield return new WaitForSeconds(dashTimer);
        _isDashing = false;
        StartCoroutine(DashCooldownTimer(dashCooldown));
    }

    IEnumerator DashCooldownTimer(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _canDash = true;
    }
}
