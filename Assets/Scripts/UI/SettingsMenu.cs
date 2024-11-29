using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundEffectsSlider;


    void Start()
    {
        if(_musicSlider == null) Debug.LogError("Music Slider is NULL");
        if(_soundEffectsSlider == null) Debug.LogError("Sound Effects Slider is NULL");

        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _musicSlider.value = loadedData.musicVolume;
            _soundEffectsSlider.value = loadedData.sEffectsVolume;
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
}
