using System;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Oversmith.Scripts.Multiplayer.Player
{
    public class PlayerMovementController : NetworkBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        public float smoothing = 10f;
        private CharacterController _cc;
        private Vector2 _previousInput;
        private Quaternion _targetRotation;

        public GameObject playerModel;
        private void Start()
        {
            Debug.Log("Start player");
            _cc = GetComponent<CharacterController>(); // Get the character controller component
            InputManager.Controls.Gameplay.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
            InputManager.Controls.Gameplay.Move.canceled += ctx => ResetMovement();
            playerModel.SetActive(false);
            // InputManager.Controls.Gameplay.Dash.performed += DashOnPerformed;
        }
        private void SetMovement(Vector2 movement)
        {
            _previousInput = movement;
        }

        private void ResetMovement()
        {
            _previousInput = Vector2.zero;
        }
        
        // void Update()
        // {
        //     // if (_isDashing)
        //     // {
        //     //     _cc.SimpleMove(_dashDirection * dashSpeed);
        //     //     //_animator.SetBool("Dash", true);
        //     //     return;
        //     // }
        //
        //     Vector3 movementDirection = new Vector3(_previousInput.x, 0, _previousInput.y);
        //     // _dashDirection = movementDirection;
        //     if (movementDirection.magnitude > 0.01f)
        //     {
        //         _targetRotation = Quaternion.LookRotation(movementDirection);
        //     }
        //     transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
        //     movementDirection.Normalize();
        //
        //     _cc.SimpleMove(movementDirection * moveSpeed);
        //
        //     // _animator.SetBool("Run",movementDirection.magnitude > 0f);
        //     // _animator.SetBool("Carry",!ReferenceEquals(_playerInteractions.ItemScript,null));
        // }
    }
}