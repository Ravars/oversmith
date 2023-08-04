using MadSmith.Scripts.Input;
using Mirror;
using UnityEngine;

namespace _Developers.Vitor.Multiplayer_1.Scripts
{
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private InputReader inputReader;
        private Vector2 previousInput;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private CharacterController controller = null;
        public override void OnStartAuthority()
        {
            Debug.Log("OnStartAuthority");
            inputReader.EnableGameplayInput();
            inputReader.MoveEvent += SetMovement;
            inputReader.MoveCanceledEvent += ResetMovement;
            
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
            Vector3 right = controller.transform.right;
            Vector3 forward = controller.transform.forward;
            right.y = 0f;
            forward.y = 0f;

            Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

            controller.Move(movement * movementSpeed * Time.deltaTime);
        }
    }
}