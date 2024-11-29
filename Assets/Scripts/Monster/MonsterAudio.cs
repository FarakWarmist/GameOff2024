using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    [SerializeField] private AnimationFunctionHolder _animationFunctionHolder;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _audioSource.volume = loadedData.musicVolume;
        }

        AudioManager.OnMusicVolumeChanged += ChangeMusicVolume;
        AudioManager.OnSoundEffectsVolumeChanged += ChangeSoundEffectsVolume;
    }

    void OnDisable()
    {
        AudioManager.OnMusicVolumeChanged -= ChangeMusicVolume;
        AudioManager.OnSoundEffectsVolumeChanged -= ChangeSoundEffectsVolume;
    }

    private void ChangeMusicVolume(float volume)
    {
        _audioSource.volume = volume;
        _animationFunctionHolder.ChangeMusicVolume(volume);
    }

    private void ChangeSoundEffectsVolume(float volume)
    {
        _animationFunctionHolder.ChangeSoundEffectsVolume(volume);
    }
}
