using System;
using Unity.Netcode;
using UnityEngine;

public class ConnectionHandler : MonoBehaviour
{
    private int _playerQuantity = 2;
    private int _currentPlayers = 0;
    public static event Action OnPlayersWaiting;
    public static event Action OnStartGame;
    void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }
    void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Se Conecto el jugador nro: " + clientId);

        _currentPlayers++;

        if (NetworkManager.Singleton.ConnectedClientsList.Count >= _playerQuantity)
        {
            OnStartGame?.Invoke();
        }
        else if (_currentPlayers == 1)
        {
            OnPlayersWaiting?.Invoke();
        }
    }

    public void OnClientDisconnected(ulong clientId)
    {
        Debug.Log("Se Desconecto el jugador nro: " + clientId);
    }

    void Update()
    {
       
    }
}
