using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseMenuButtons : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.Instance.RestartScene();
    }

    public void MainMenu()
    {
        SceneManager.Instance.ChangeSceneByIndex(0);
    }
}
