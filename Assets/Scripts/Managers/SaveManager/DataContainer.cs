using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataContainer
{
    public float musicVolume = 0.5f;
    public float sEffectsVolume = 0.5f;
    public float bestTime = 0;
    public float mouseSensitivity = 0.5f;

    public DataContainer(float mVol, float eVol, float bestTime, float mouseSensitivity)
    {
        musicVolume = mVol;
        sEffectsVolume = eVol;
        this.bestTime = bestTime;
        this.mouseSensitivity = mouseSensitivity;
    }

    public DataContainer()
    {
        
    }
}
