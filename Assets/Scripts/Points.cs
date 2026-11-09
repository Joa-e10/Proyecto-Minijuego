using Unity.Netcode;
using UnityEngine;

public class Points : NetworkBehaviour
{
   [SerializeField] public string namePoint;
   [SerializeField] public int amount;

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
