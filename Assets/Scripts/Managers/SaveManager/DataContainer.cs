using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataContainer
{
    public float musicVolume = 0.5f;
    public float sEffectsVolume = 0.5f;
    public float bestTime;

    public DataContainer(float mVol, float eVol, bool bloom, bool vignette)
    {
        musicVolume = mVol;
        sEffectsVolume = eVol;
    }

    public DataContainer()
    {
        
    }
}
