using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private float _fogDensityReducer = 0.03f;
    void OnEnable()
    {
        FogManager.Instance.SetDensity(FogManager.normalFogDensity - _fogDensityReducer);
    }

    void OnDisable()
    {
        FogManager.Instance.SetDensity(FogManager.normalFogDensity);
    }
}
