using System;
using Unity.Netcode;
using UnityEngine;

public class TimerManager : NetworkBehaviour
{
    protected NetworkVariable<float> _timer = new NetworkVariable<float>(30f);
    private float _intervals = 25f;
    public bool _isReady;
    public CanvasManager _canvasManager;
    public PanelHudUI _panelHud;

    public static event Action OnResetSpawn;
    public static event Action OnEndGame;

    public override void OnNetworkSpawn()
    {
        _canvasManager = GameObject.Find("Background_PanelUI").GetComponent<CanvasManager>();
        _panelHud = _canvasManager.GetPanelHud();

        _panelHud.SetTimerReference(this.GetComponent<TimerManager>());
    }

    private void OnEnable()
    {
        
        ConnectionHandler.OnStartGame += SetIsReady;
    }

    private void OnDisable()
    {
        ConnectionHandler.OnStartGame -= SetIsReady;
    }

    public float GetTimer() 
    {
        return _timer.Value;
    }

    private void SetIsReady() 
    {
        _isReady = true;
    }
    public void ResetTime()
    {
       if (_timer.Value <= _intervals)
       {
           Debug.Log("INTERVALO DE RESETTTTT");
           _intervals -= 10;
           OnResetSpawn?.Invoke();
       }
    }

    public void EndTime() 
    {
        OnEndGame?.Invoke();
        this.GetComponent<NetworkObject>().Despawn();
    }
    void Update()
    {
        if (!IsHost) return;
        if (!_isReady) return;

        if (_timer.Value >= 0)
        {
            _timer.Value -= Time.deltaTime;
            ResetTime();
        }
        else 
        {
            _timer.Value = 0;

            EndTime();
        }
    }
}
