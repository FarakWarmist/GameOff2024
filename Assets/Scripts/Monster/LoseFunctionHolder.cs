using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseFunctionHolder : MonoBehaviour
{
    public void FadeToLose()
    {
        UIManager.Instance.ActivateLoseMenu();
    }
}
