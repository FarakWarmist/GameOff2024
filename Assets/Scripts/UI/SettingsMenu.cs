using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectsSlider;
    [SerializeField] private Slider _mouseSensitivitySlider;
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
        }
    }

    public void ChangeMusicVolume()
    {
        AudioManager.Instance.SetVolume("Music", _musicSlider.value);
        SaveManager.Instance.Save(_musicSlider.value, "MUSIC");
    }

    public void ChangeSoundEffectsVolume()
    {
        AudioManager.Instance.SetVolume("Effects", _soundEffectsSlider.value);
        SaveManager.Instance.Save(_soundEffectsSlider.value, "EFFECTS");
    }

    public void ChangeMouseSensitivity()
    {
        SaveManager.Instance.SaveMouseSensitivity(_mouseSensitivitySlider.value);
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
