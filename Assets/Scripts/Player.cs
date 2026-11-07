using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.EventSystems;

public class Player : NetworkBehaviour
{

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    [SerializeField] PlayerInput _playerInput;

    //Character controller
    private CharacterController _characterController;
    private Vector2 _input;
    private float _yVelocity;
    private float _gravity = -9.81f;
    private float _speed = 5f;

    public override void OnNetworkSpawn()
    {
        _characterController = GetComponent<CharacterController>();
        //_playerInput.enabled = IsOwner;
    }

    private Vector3 ApplyGravity(Vector3 moveDirection)
    {
        if (_characterController.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -2f;
        }

        _yVelocity += _gravity * Time.deltaTime;
        moveDirection.y = _yVelocity;

        return moveDirection;
    }
    private void RotateCharacter(Vector3 moveDirection)
    {
        if (moveDirection.magnitude <= 0.1f) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    private void MoveCharacter(Vector3 moveDirection)
    {
        _characterController.Move(moveDirection * _speed * Time.deltaTime);

    }

    private void OnMove(InputValue inputValue)  // Utilizamos el metodo OnMove designado para la accion de mover.
    {
        _input = inputValue.Get<Vector2>();
    }

    private Vector3 GetCameraRelativeDirection()
    {
        Transform cam = Camera.main.transform;

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        return camRight * _input.x + camForward * _input.y;
    }

    void Update()
    {
        Vector3 moveDirection = GetCameraRelativeDirection();
        RotateCharacter(moveDirection);
        moveDirection = ApplyGravity(moveDirection);
        MoveCharacter(moveDirection);
    }
}
