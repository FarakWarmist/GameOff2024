using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TimeFormatterUtil;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    [SerializeField] private Button _settingsButton; 
    [SerializeField] private Button _quitButton;
    [SerializeField] private TMP_Text _recordTimeText;

    void Start()
    {
        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            if(loadedData.bestTime == 0)
            {
                _recordTimeText.text = "";
            }
            else
            {
                _recordTimeText.text = "Record Time: " + TimeFormatter.GetTextFromTime(loadedData.bestTime);
            }
        }

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
