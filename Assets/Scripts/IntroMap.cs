using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMap : MonoBehaviour
{
    public void PlayDoorSound()
    {

    }

    public void PlayWooshSound()
    {

    }

    public void TransitionWithFade()
    {
        UIManager.Instance.FadeTo(true);
        SceneManager.Instance.ChangeSceneWithDelay(0.5f, 2);
    }
}
