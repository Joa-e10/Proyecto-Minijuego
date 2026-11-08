using UnityEngine;

public class TimerManager : MonoBehaviour
{
    private float _timer;
    private float _intervals = 10;
    private bool _isReady;

    public void ResetTime() 
    {
        if (_timer >= _intervals) 
        {
            _intervals += 10;
        }
    }
    void Update()
    {
        Debug.Log("Timer: "+_timer);
        if (!_isReady) return;
        _timer += Time.deltaTime;
    }
}
