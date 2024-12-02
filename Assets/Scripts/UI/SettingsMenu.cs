using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectsSlider;
    [SerializeField] private Slider _mouseSensitivitySlider;
    [SerializeField] private TMP_Text _musicValueText;
    [SerializeField] private TMP_Text _soundEffectsValueText;
    [SerializeField] private TMP_Text _mouseSensitivityValueText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _quitButton;


    void Start()
    {
        if(_musicSlider == null) Debug.LogError("Music Slider is NULL");
        if(_soundEffectsSlider == null) Debug.LogError("Sound Effects Slider is NULL");

        if(_restartButton != null) _restartButton.onClick.AddListener(() => Restart());
        if(_mainMenuButton != null) _mainMenuButton.onClick.AddListener(() => MainMenu());
        if(_quitButton != null) _quitButton.onClick.AddListener(() => Quit());

        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _musicSlider.value = loadedData.musicVolume;
            _soundEffectsSlider.value = loadedData.sEffectsVolume;
            _mouseSensitivitySlider.value = loadedData.mouseSensitivity;

            _musicValueText.text = "0." + Math.Truncate(loadedData.musicVolume * 100).ToString();
            _soundEffectsValueText.text = "0." + Math.Truncate(loadedData.sEffectsVolume * 100).ToString();
            _mouseSensitivityValueText.text = "0." + Math.Truncate(loadedData.mouseSensitivity * 100).ToString();
        }
    }

    public void ChangeMusicVolume()
    {
        AudioManager.Instance.SetVolume("Music", _musicSlider.value);
        SaveManager.Instance.Save(_musicSlider.value, "MUSIC");
        _musicValueText.text = "0." + Math.Truncate(_musicSlider.value * 100).ToString();
    }

    public void ChangeSoundEffectsVolume()
    {
        AudioManager.Instance.SetVolume("Effects", _soundEffectsSlider.value);
        SaveManager.Instance.Save(_soundEffectsSlider.value, "EFFECTS");
        _soundEffectsValueText.text = "0." + Math.Truncate(_soundEffectsSlider.value * 100).ToString();
    }

    public void ChangeMouseSensitivity()
    {
        SaveManager.Instance.SaveMouseSensitivity(_mouseSensitivitySlider.value);
        _mouseSensitivityValueText.text = "0." + Math.Truncate(_mouseSensitivitySlider.value * 100).ToString();
    }

    private void Restart()
    {
        Time.timeScale = 1;
        SceneManager.Instance.RestartScene();
    }

    private void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.Instance.ChangeSceneByIndex(0);
    }

    private void Quit()
    {
        Application.Quit();
    }
}
