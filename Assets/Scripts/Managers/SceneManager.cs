using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    public void ChangeSceneByIndex(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

    public void RestartScene() => ChangeSceneByIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
}
