using _Developers.Vitor;
using Oversmith.Scripts;
using Test1.Scripts.Prototype;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private CharacterController _cc;
    private Quaternion _targetRotation;
    public float smoothing = 5f;
    private Vector2 _previousInput;
    [SerializeField] private Animator _animator; // temporary
    [SerializeField] private PlayerInteractions _playerInteractions; // temporary

    void Start()
    {
        _cc = GetComponent<CharacterController>(); // Get the character controller component
        InputManager.Controls.Gameplay.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Gameplay.Move.canceled += ctx => ResetMovement();
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
        
        
        
        //_animator.SetBool("Run",movementDirection.magnitude > 0f);
        //_animator.SetBool("Carry",!ReferenceEquals(_playerInteractions.ItemScript,null));
    }
}
