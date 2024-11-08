using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeFormatterUtil;

public class Timer : MonoBehaviour
{
    private float _timeStart;
    private float _timer;

    void Start() => _timeStart = Time.time;

    void Update()
    {
            _timer = Time.time - _timeStart;
            UIManager.Instance.UpdateTimerText(TimeFormatter.GetTextFromTime(_timer));
    }
}
