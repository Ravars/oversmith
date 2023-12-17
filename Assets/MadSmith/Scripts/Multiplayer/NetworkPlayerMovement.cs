using MadSmith.Scripts.Events.ScriptableObjects;
using MadSmith.Scripts.Input;
using MadSmith.Scripts.Multiplayer.Managers;
using MadSmith.Scripts.Multiplayer.Old.Managers;
using Mirror;
using UnityEngine;

namespace MadSmith.Scripts.Multiplayer.Old.Player
{
    public class NetworkPlayerMovement : NetworkBehaviour
    {
        public float smoothing = 5f;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private InputReader inputReader;
        private Vector2 previousInput;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private CharacterController controller = null;
        private Quaternion _targetRotation;
        private static readonly int Run = Animator.StringToHash("Run");
        [SerializeField] private Animator _animator; // temporary
        
        
        [SerializeField] private VoidEventChannelSO _onGameStart = default;
        private MadSmithNetworkRoomManager _room;
        private MadSmithNetworkRoomManager Room
        {
            get
            {
                if (_room != null) { return _room; }
                return _room = NetworkManager.singleton as MadSmithNetworkRoomManager;
            }
        }
        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority");
            enabled = true;
            
            // RpcEnableMovement();
            // Room.GamePlayers.Add(this);
            // NetworkClient.PrepareToSpawnSceneObjects();
        }
        [Command]
        private void CmdSceneReady()
        {
            //Debug.Log("CMD Scene ready");
            // Manager.ClientSceneReady();
        }
        private void Start()
        {
            //Debug.Log("Start");
            _onGameStart.OnEventRaised += OnStartGame;
            // DontDestroyOnLoad(gameObject);
        }

        private void OnStartGame()
        {
            EnableMovement();
        }
        public void EnableMovement()
        {
            // Debug.Log("before hasAuthority");
            if (!hasAuthority) return;
            // Debug.Log("authority Enabled RpcEnableMovement");
            inputReader.EnableGameplayInput();
            inputReader.MoveEvent += SetMovement;
            inputReader.MoveCanceledEvent += ResetMovement;
            // inputReader.DashEvent += DashOnPerformed;
        }

        private void OnDisable()
        {
            inputReader.MoveEvent -= SetMovement;
            inputReader.MoveCanceledEvent -= ResetMovement;
            // inputReader.DashEvent -= DashOnPerformed;
        }

        private void DashOnPerformed()
        {
            
        }

        [ClientCallback]
        private void Update() => Move();

        [Client]
        private void SetMovement(Vector2 movement) => previousInput = movement;

        [Client]
        private void ResetMovement() => previousInput = Vector2.zero;

        [Client]
        private void Move()
        {
            var transform1 = controller.transform;
            Vector3 right = transform1.right;
            Vector3 forward = transform1.forward;
            right.y = 0f;
            forward.y = 0f;
            
            Vector3 movementDirection = new Vector3(previousInput.x, 0, previousInput.y);
            if (movementDirection.magnitude > 0.01f)
            {
                _targetRotation = Quaternion.LookRotation(movementDirection);
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * smoothing);
            movementDirection.Normalize();
        
            controller.SimpleMove(movementDirection * moveSpeed);
            _animator.SetBool(Run,movementDirection.magnitude > 0f);
            

            // Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;
            //
            // controller.Move(movement * (movementSpeed * Time.deltaTime));
        }
        // public void UpdateCountdown(int value)
        // {
        //     RpcUpdateCountdown(value);
        // }
        //
        // public void RpcUpdateCountdown(int value)
        // {
        //     
        // }
    }
}