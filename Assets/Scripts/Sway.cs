using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] private Transform _followTransform;
    [SerializeField] private float _time;
    [SerializeField] private float _interpolationSpeed = 0.25f;

    void FixedUpdate()
    {
        _time = _interpolationSpeed;
        transform.position = Vector3.Slerp(transform.position, _followTransform.position, _time);
        transform.rotation = Quaternion.Slerp(transform.rotation, _followTransform.rotation, _time);
    }
}
