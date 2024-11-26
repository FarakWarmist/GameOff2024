using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FogManager : MonoSingleton<FogManager>
{
    public const float normalFogDensity = 0.12f;
    private float _fogDensity;
    private float _interpolationSpeed = 0.5f;

    void Start() => SetDensity(normalFogDensity);

    void Update()
    {
        if(RenderSettings.fogDensity != _fogDensity)
        {
            if(_fogDensity > RenderSettings.fogDensity)
            {
                RenderSettings.fogDensity += Time.deltaTime * _interpolationSpeed;
                if(RenderSettings.fogDensity >= _fogDensity)
                {
                    RenderSettings.fogDensity = _fogDensity;
                }
            }
            else
            {
                RenderSettings.fogDensity -= Time.deltaTime * _interpolationSpeed;
                if(RenderSettings.fogDensity <= _fogDensity)
                {
                    RenderSettings.fogDensity = _fogDensity;
                }
            }
        }
    }

    public void SetDensity(float density) => _fogDensity = density;
}
