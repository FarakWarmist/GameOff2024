using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFormatterUtil;

public class Timer : MonoSingleton<Timer>
{
    private float _timeStart;
    private float _timer;
    private bool _isStopped;

    void Start() => _timeStart = Time.time;

    void Update()
    {
        if(_isStopped == false)
        {
            _timer = Time.time - _timeStart;
            UIManager.Instance.UpdateTimerText(TimeFormatter.GetTextFromTime(_timer));
        }
    }

    public void StopTimer() => _isStopped = true;

    public float GetTime() => _timer;
}
