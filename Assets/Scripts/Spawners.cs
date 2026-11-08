using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    [SerializeField] private List<Transform> _listOfPositions = new List<Transform>();
    [SerializeField] private List<GameObject> _listOfPoints = new List<GameObject>();
    [SerializeField] private GameObject[] _listOfCurrentPoints = new GameObject[5];
    public bool _inicializate;
    void Start()
    {
        
    }

    private void Update()
    {
        SpawnPoints();
    }
    private void SpawnPoints()
    {
        if (_inicializate)
        {
            _inicializate = false;

            for (int i = 0; i < _listOfCurrentPoints.Length; i++) 
            {
                int randomNumber = Random.Range(0, _listOfPoints.Count);
                _listOfCurrentPoints[i] = _listOfPoints[randomNumber];
            }

            int index = 0;
            foreach (GameObject point in _listOfCurrentPoints)
            {
                GameObject pointSpawned = Instantiate(point, _listOfPositions[index].position, Quaternion.identity);
                pointSpawned.GetComponent<NetworkObject>().Spawn();
                index++;
            }
        }
    }
}
