using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float _timer;
    private float _intervals = 5;
    public bool _isReady;

    public static event Action OnResetSpawn;
    public void ResetTime()
    {
        if (_timer >= _intervals)
        {
            Debug.Log("INTERVALO DE RESETTTTT");
            _intervals += 5;
            OnResetSpawn?.Invoke();
        }
    }
    void Update()
    {
        Debug.Log("Timer: "+_timer);
        if (!_isReady) return;
        _timer = _timer + Time.deltaTime;
        ResetTime();
    }
}
