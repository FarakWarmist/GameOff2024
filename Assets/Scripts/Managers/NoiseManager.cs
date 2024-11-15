using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NoiseManager : MonoSingleton<NoiseManager>
{
    public static event Action<float, Vector3> OnNoiseMade;

    public void MakeNoise(float volume, Vector3 position)
    {
        if(OnNoiseMade != null) OnNoiseMade(volume, position);
    }
}
