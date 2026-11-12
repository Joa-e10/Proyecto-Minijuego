using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PanelHudUI : MonoBehaviour
{

    [Header("GameObject UI")]
    [SerializeField] private TextMeshProUGUI _pointTextOwner;
    [SerializeField] private TextMeshProUGUI _pointTextOther;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TimerManager _timerManager;

    private Player _playerObject;

    public static event Action OnEndGame;
    public void SetTimerReference(TimerManager _actuallyTimer) 
    {
        _timerManager = _actuallyTimer;
    }
    public void RefreshScores() 
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.PlayerObject.IsOwner)
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _pointTextOwner.text = _playerObject.GetPointPlayer().ToString();
            }
            else 
            {
                _playerObject = client.PlayerObject.GetComponent<Player>();
                _pointTextOther.text = _playerObject.GetPointPlayer().ToString();
            }
        }
    }

    public void RefreshTimer() 
    {
        if (_timerManager == null) return;
        Debug.Log("Estamos actualizando la wea");
        _timerText.text = _timerManager.GetTimer().ToString("F1");
        if (_timerManager.GetTimer() <= 0) 
        {
            Debug.Log("Tenemos el fin");
            OnEndGame?.Invoke();
        }
    }

    void Update()
    {
        RefreshScores();
        RefreshTimer();
    }
}
