using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestNoise : MonoBehaviour
{
    public static event Action<float, Vector3> OnNoiseMade;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(OnNoiseMade != null) OnNoiseMade(20, transform.position);
        }
    }
}
