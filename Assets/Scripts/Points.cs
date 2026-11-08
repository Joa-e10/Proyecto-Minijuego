using Unity.Netcode;
using UnityEngine;

public class Points : NetworkBehaviour
{
   [SerializeField] public string namePoint;
   [SerializeField] public int amount;

    void OnEnable()
    {
        TimerManager.OnResetSpawn += PointDespawn;
    }

    private void OnDisable()
    {
        TimerManager.OnResetSpawn -= PointDespawn;
    }
    public override void OnNetworkSpawn()
    {
        
    }
    public void PointDespawn()
    {
       GetComponent<NetworkObject>().Despawn();
    }
}
