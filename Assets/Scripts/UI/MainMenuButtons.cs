using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    [SerializeField] private Button _settingsButton; 
    [SerializeField] private Button _quitButton;

    void Start()
    {
        if(_playButton == null) Debug.LogError("Play Button is NULL");
        if(_settingsButton == null) Debug.LogError("Settings Button is NULL");
        if(_quitButton == null) Debug.LogError("Quit Button is NULL");

        _playButton.onClick.AddListener(() => Play());
        _settingsButton.onClick.AddListener(() => Settings());
        _quitButton.onClick.AddListener(() => Quit());
    }

    private void Play()
    {
        SceneManager.Instance.ChangeSceneByIndex(1);
    }

    private void Settings()
    {
        Debug.Log("Open Settings");
    }

    private void Quit()
    {
        Application.Quit();
    }
}
