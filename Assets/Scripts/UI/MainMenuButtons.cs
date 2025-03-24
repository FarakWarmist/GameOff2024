using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TimeFormatterUtil;
using System.IO;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button _playButton; 
    [SerializeField] private Button _settingsButton; 
    [SerializeField] private Button _quitButton;
    [SerializeField] private TMP_Text _recordTimeText;
    [SerializeField] private GameObject _settingsMenu;

    [SerializeField] private TMP_Text _playButtonText;
    [SerializeField] private TMP_Text _settingsButtonText;
    [SerializeField] private TMP_Text _quitButtonText;

    void Start()
    {
        DataContainer loadedData = SaveManager.Instance.Load();

        _playButtonText.text = Localization.GetString("play_button");
        _settingsButtonText.text = Localization.GetString("settings_button");
        _quitButtonText.text = Localization.GetString("quit_button");
        if (loadedData != null)
        {
            if(loadedData.bestTime == 0)
            {
                _recordTimeText.text = "";
            }
            else
            {
                _recordTimeText.text = Localization.GetString("record_time") + " " + TimeFormatter.GetTextFromTime(loadedData.bestTime);
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
        if(_settingsMenu.activeSelf == false) _settingsMenu.SetActive(true);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
