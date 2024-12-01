using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoSingleton<SceneManager>
{
    public void ChangeSceneByIndex(int sceneIndex) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);

    public void RestartScene() => ChangeSceneByIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);

    public int GetBuildIndex() => UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

    public void ChangeSceneWithDelay(float seconds, int index)
    {
        StartCoroutine(ChangeSceneWithDelayRoutine(seconds, index));
    }

    private IEnumerator ChangeSceneWithDelayRoutine(float seconds, int index)
    {
        yield return new WaitForSeconds(seconds);
        ChangeSceneByIndex(index);
    }
}
