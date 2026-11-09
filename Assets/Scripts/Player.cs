using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    protected RaycastHit _hit;
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] Rigidbody _rb;
    public float _JumpForce;
    private Vector3 _directionMove;
    private bool _isGrounded;
    public LayerMask _layerGround;
    public float _rayCastHeight;
    //Character controller
    private float _speed = 5f;
    public int _pointValue = 0;
    protected NetworkVariable<int> _totalPoints = new NetworkVariable<int>();

    public Points pointColision;

    public int GetPointPlayer()
    {
        return _totalPoints.Value;
    }
    public override void OnNetworkSpawn()
    {
        OwnerInitialized();
        _rb = GetComponent<Rigidbody>();
    }

    private void OwnerInitialized() 
    {
        if (IsOwner) 
        {
            _playerInput.enabled = true;
        }
    }

    private void JumpCharacter() 
    {
        if (_isGrounded == true)
        {
            _rb.AddForce(new Vector3(0, _JumpForce,0), ForceMode.Impulse);
        }
        else 
        {
            Debug.Log("No se puede saltar");
        }
    }

    public bool IsGroundedState() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, _rayCastHeight, _layerGround))
        {
            return _isGrounded = true;
        }
        else 
        {
            return _isGrounded = false;
        }
    }
    private void OnJump(InputValue inputValue) 
    {
        JumpCharacter();
    }

    private void OnMove(InputValue inputValue)  // Utilizamos el metodo OnMove designado para la accion de mover.
    {
            Vector2 move = inputValue.Get<Vector2>(); // Tomamos el valor recibido de la accion.

            _directionMove = new Vector3(move.x, 0, move.y);
    }
    private void OnAttack(InputValue inputValue) 
    {
        if (inputValue.isPressed) 
        {
            //NetworkManager.Singleton.StartHost();
        }
    }

    private void PointUpdate(int amount) 
    {
        _pointValue = _pointValue + amount;

        _totalPoints.Value =+ _pointValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        pointColision = other.GetComponent<Points>();
        PointUpdate_ServerRpc();
    }

    [ServerRpc]
    private void PointUpdate_ServerRpc()
    {
        if (pointColision != null)
        {
            PointUpdate(pointColision.amount);
            pointColision.PointDespawn();
        }
    }

    private void Update()
    {
        IsGroundedState();
        _rb.linearVelocity = new Vector3(_directionMove.x * _speed, _rb.linearVelocity.y, _directionMove.z * _speed );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, Vector3.down * _rayCastHeight);
    }
}



