using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using NUnit.Framework;
using System.Collections.Generic;

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
    [SerializeField] private List<Transform> _SpawnsPlayer = new List<Transform>();

    public Points pointColision;
    private bool _playAgain;

    public bool GetPlayAgain()
    {
        return _playAgain;
    }
    public void SetPlayAgain()
    {
         _playAgain = true;
    }
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

        if (IsServer && IsOwner)
        {
            Transform spawn = GameObject.Find("PositionPlayer").GetComponent<Transform>();

            if (spawn != null)
                transform.position = spawn.position;
        }
        else if (IsOwner)
        {
            Transform spawn = GameObject.Find("PositionClient").GetComponent<Transform>();

            if (spawn != null)
                transform.position = spawn.position;
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

    private void PointUpdate(int amount, string nameTag) 
    {
        if (nameTag == "Positive")
        {
            Debug.Log("Tag: "+nameTag);
            _totalPoints.Value = _totalPoints.Value + amount;
        }
        else 
        {
            _totalPoints.Value = _totalPoints.Value - amount;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        NetworkObject pointObject = other.GetComponent<NetworkObject>();

        if (pointObject != null)
        {
            PointUpdate_ServerRpc(new NetworkObjectReference(pointObject));
        }
    }

    [ServerRpc]
    private void PointUpdate_ServerRpc(NetworkObjectReference newPoint)
    {
        if (!newPoint.TryGet(out NetworkObject pointObject)) return;

        if (!pointObject.TryGetComponent<Points>(out Points point)) return;

        PointUpdate(point.amount, point.tag);
       point.PointDespawn();
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



