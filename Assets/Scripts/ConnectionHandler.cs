using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionHandler : NetworkBehaviour
{
    private int _playerQuantity = 2;
    private int _currentPlayers = 0;
    public static event Action OnPlayersWaiting;
    public static event Action OnStartGame;
    private MeshRenderer _color;
    public List<Color> _listOfColors = new List<Color> { Color.beige, Color.aquamarine};
    
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
            ColorClientsUpdate();
        }
        else if (_currentPlayers == 1)
        {
            OnPlayersWaiting?.Invoke();
        }
    }

    private void ColorClientsUpdate() 
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList) 
        {
            if (client.IsSessionOwner)
            {
                MeshRenderer playerColor = client.PlayerObject.GetComponent<MeshRenderer>();
                playerColor.material.color = _listOfColors[0];
            }
            else 
            {
                MeshRenderer playerColor = client.PlayerObject.GetComponent<MeshRenderer>();
                playerColor.material.color = _listOfColors[1];
            }
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
