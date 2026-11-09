using Unity.Netcode;
using UnityEngine;

public class Points : NetworkBehaviour
{
   [SerializeField] public string namePoint;
   [SerializeField] public int amount;
    private float _positionItemY;
    private float _additionValue = 0.4f;

    void OnEnable()
    {
        TimerManager.OnResetSpawn += PointDespawn_ServerRpc;
    }

    private void OnDisable()
    {
        TimerManager.OnResetSpawn -= PointDespawn_ServerRpc;
    }
    public override void OnNetworkSpawn()
    {
        PositionOnLand();
    }

    public void PositionOnLand()
    {
        _positionItemY = transform.position.y;

          _positionItemY = _positionItemY + _additionValue;
          transform.position = new Vector3(transform.position.x, _positionItemY, transform.position.z);
    }

    public void PointDespawn()
    {
       GetComponent<NetworkObject>().Despawn(true);
    }

    [ServerRpc]
    private void PointDespawn_ServerRpc()
    {
        PointDespawn();
    }
}
