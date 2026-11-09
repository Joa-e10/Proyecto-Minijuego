using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawners : NetworkBehaviour
{
    [SerializeField] private List<Transform> _listOfPositions = new List<Transform>();
    [SerializeField] private List<GameObject> _listOfPoints = new List<GameObject>();
    [SerializeField] private GameObject[] _listOfCurrentPoints = new GameObject[5];
    private GameObject pointSpawned;
    public bool _inicializate;
    void Start()
    {
        
    }

    void OnEnable()
    {
        ConnectionHandler.OnStartGame += SpawnPoints;
        TimerManager.OnResetSpawn += SpawnPoints;
    }

    void OnDisable()
    {
        ConnectionHandler.OnStartGame -= SpawnPoints;
        TimerManager.OnResetSpawn -= SpawnPoints;
    }

    private void Update()
    {
       //SpawnPoints();
    }
    private void SpawnPoints()
    {
        if (IsHost)
        {

            for (int i = 0; i < _listOfCurrentPoints.Length; i++) 
            {
                int randomNumber = Random.Range(0, _listOfPoints.Count);
                _listOfCurrentPoints[i] = _listOfPoints[randomNumber];
            }

            int index = 0;
            foreach (GameObject point in _listOfCurrentPoints)
            {
                pointSpawned = Instantiate(point, _listOfPositions[index].position, Quaternion.identity);
                PointSpawn_ServerRpc();
                index++;

                Debug.Log("Objeto spawneado correctamente");
            }
        }
    }

    [ServerRpc]
    private void PointSpawn_ServerRpc()
    {
        pointSpawned.GetComponent<NetworkObject>().Spawn();
    }
}
