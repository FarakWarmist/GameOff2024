using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    private int buildIndex;
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _effectsAudioSource;
    [SerializeField] private AudioClip[] _soundEffects;
    private Dictionary<string, AudioClip> _soundsEffectsByName = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioClip[] _footStepsSoundEffects;
    [SerializeField] private AudioClip[] _monsterFootStepsSoundEffects;
    [SerializeField] private AudioClip[] _randomAmbianceAudioClips;
    [SerializeField] private AudioClip[] _openCloseDoorAudioClips;

    [SerializeField] private float _randomAmbianceMinTime = 60;
    [SerializeField] private float _randomAmbianceMaxTime = 180;
    private float _randomAmbianceTimer;

    private float _musicVolume = 0.5f;
    private float _soundEffectsVolume = 0.5f;
    public static event Action<float> OnMusicVolumeChanged;
    public static event Action<float> OnSoundEffectsVolumeChanged;

    void Start()
    {
        buildIndex = SceneManager.Instance.GetBuildIndex();

        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _musicVolume = loadedData.musicVolume;
            _soundEffectsVolume = loadedData.sEffectsVolume;

            _musicAudioSource.volume = _musicVolume;
            _effectsAudioSource.volume = _soundEffectsVolume;
        }

        InitializeSoundEffectsByName();

        _randomAmbianceTimer = Time.time + UnityEngine.Random.Range(_randomAmbianceMinTime, _randomAmbianceMaxTime);
    }

    void Update()
    {
        if(buildIndex != 2) return;

        if(_randomAmbianceTimer <= Time.time)
        {
            int rand = UnityEngine.Random.Range(0, _randomAmbianceAudioClips.Length);
            _effectsAudioSource.PlayOneShot(_randomAmbianceAudioClips[rand], 0.25f); //! Change this when the save system is done
            _randomAmbianceTimer = Time.time + UnityEngine.Random.Range(_randomAmbianceMinTime, _randomAmbianceMaxTime);
        }
    }

    public void PlaySFX(string soundEffectName, float intensity = -1)
    {
        if(intensity != -1)
        {
            _effectsAudioSource.PlayOneShot(GetAudioClip(soundEffectName), intensity);
        }
        else
        {
            _effectsAudioSource.PlayOneShot(GetAudioClip(soundEffectName));
        }
    }

    public void SetVolume(string affected, float volume)
    {
        switch(affected)
        {
            case "Effects":
                _effectsAudioSource.volume = volume;
                _soundEffectsVolume = volume;
                if(OnSoundEffectsVolumeChanged != null) OnSoundEffectsVolumeChanged(_soundEffectsVolume);
            break;

            case "Music":
                _musicAudioSource.volume = volume;
                _musicVolume = volume;
                if(OnMusicVolumeChanged != null) OnMusicVolumeChanged(_musicVolume);
            break;

            default:
                Debug.LogError("Volume affected name not recognized");
            break;
        }
    }

    public AudioClip GetAudioClip(string audioClipName)
    {
        if(audioClipName == "Step")
        {
            int rand = UnityEngine.Random.Range(0, _footStepsSoundEffects.Length);
            return _footStepsSoundEffects[rand];
        }

        if(audioClipName == "MonsterStep")
        {
            int rand = UnityEngine.Random.Range(0, _monsterFootStepsSoundEffects.Length);
            return _monsterFootStepsSoundEffects[rand];
        }

        if(audioClipName == "OpenCloseDoor")
        {
            int rand = UnityEngine.Random.Range(0, _openCloseDoorAudioClips.Length);
            return _openCloseDoorAudioClips[rand];
        }

        return _soundsEffectsByName[audioClipName];
    }


    private void InitializeSoundEffectsByName()
    {
        _soundsEffectsByName["Detection"] = _soundEffects[0];
        _soundsEffectsByName["UnlockDoor"] = _soundEffects[1];
        _soundsEffectsByName["HandleItem"] = _soundEffects[2];
        _soundsEffectsByName["NPCTalk"] = _soundEffects[3];
        _soundsEffectsByName["Hide"] = _soundEffects[4];
        _soundsEffectsByName["HideReversed"] = _soundEffects[5];
        _soundsEffectsByName["Jumpscare"] = _soundEffects[6];
        _soundsEffectsByName["MonsterGrowl"] = _soundEffects[7];
        _soundsEffectsByName["MonsterBite"] = _soundEffects[8];
    }
}
