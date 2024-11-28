using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctionHolder : MonoBehaviour
{
    public void FadeToLose()
    {
        UIManager.Instance.ActivateLoseMenu();
        AudioManager.Instance.PlaySFX("MonsterBite");
    }

    public void Growl()
    {
        AudioManager.Instance.PlaySFX("MonsterGrowl");
    }
}
