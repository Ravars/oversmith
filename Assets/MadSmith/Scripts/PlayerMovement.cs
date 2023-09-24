using System.Collections;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Interaction;
using UnityEngine;

namespace MadSmith.Scripts
{
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
        private bool _canMove = true;
        [SerializeField] private Animator _animator; // temporary
        [SerializeField] private PlayerInteractions _playerInteractions; // temporary
        [SerializeField] private InputReader _inputReader;
        private static readonly int DoubleHand = Animator.StringToHash("DoubleHand");
        private static readonly int Run = Animator.StringToHash("Run");

        [SerializeField] private GameObject visual;
        [SerializeField] private GameObject respawnVfx;

        void Start()
        {
            _cc = GetComponent<CharacterController>(); // Get the character controller component
        }

        private void OnEnable()
        {
            _inputReader.MoveEvent += SetMovement;
            _inputReader.MoveCanceledEvent += ResetMovement;
            _inputReader.DashEvent += DashOnPerformed;
        }

        private void OnDisable()
        {
            _inputReader.MoveEvent -= SetMovement;
            _inputReader.MoveCanceledEvent -= ResetMovement;
            _inputReader.DashEvent -= DashOnPerformed;
        }

        private void SetMovement(Vector2 movement)
        {
            _previousInput = movement;
        }

        private void ResetMovement()
        {
            _previousInput = Vector2.zero;
        }

        private void DashOnPerformed()
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
            if (!_canMove) return;
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

            _animator.SetBool(Run,movementDirection.magnitude > 0f);
            
            bool isCarrying = !ReferenceEquals(_playerInteractions.ItemScript, null);
            if (isCarrying)
            {
                bool doubleHand = _playerInteractions.ItemScript.baseItem.isDoubleHand;
                _animator.SetBool(DoubleHand,doubleHand);
            }
            _animator.SetLayerWeight(1,isCarrying ? 1 : 0);
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
        IEnumerator RespawnTimer(float cooldown)
        {
            yield return new WaitForSeconds(1);
            // spawn vfx
            respawnVfx.SetActive(true);
            yield return new WaitForSeconds(cooldown);
            respawnVfx.SetActive(false);
            // turn off spawn vfx
            visual.SetActive(true);
            _cc.enabled = true;
            _canMove = true;
            _inputReader.EnableGameplayInput();
        }

        public void DisableInput(Transform pointToSpawn)
        {
            Debug.Log("Disable");
            _inputReader.DisableAllInput();
            visual.SetActive(false);
            _canMove = false;
            _cc.enabled = false;
            ResetMovement();
            transform.position = pointToSpawn.position;
            // Splash VFX
            StartCoroutine(RespawnTimer(2));
        }
    }
}
