using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _effectsAudioSource;
    [SerializeField] private AudioClip[] _soundEffects;
    private Dictionary<string, AudioClip> _soundsEffectsByName = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioClip[] _footStepsSoundEffects;
    [SerializeField] private AudioClip[] _randomAmbianceAudioClips;
    [SerializeField] private AudioClip[] _openCloseDoorAudioClips;

    [SerializeField] private float _randomAmbianceMinTime = 60;
    [SerializeField] private float _randomAmbianceMaxTime = 180;
    private float _randomAmbianceTimer;

    void Start()
    {
        InitializeSoundEffectsByName();

        _randomAmbianceTimer = Time.time + Random.Range(_randomAmbianceMinTime, _randomAmbianceMaxTime);
    }

    void Update()
    {
        if(_randomAmbianceTimer <= Time.time)
        {
            int rand = Random.Range(0, _randomAmbianceAudioClips.Length);
            _effectsAudioSource.PlayOneShot(_randomAmbianceAudioClips[rand], 0.25f); //! Change this when the save system is done
            _randomAmbianceTimer = Time.time + Random.Range(_randomAmbianceMinTime, _randomAmbianceMaxTime);
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
            break;

            case "Music":
                _musicAudioSource.volume = volume;
            break;

            default:
                Debug.LogError("Volume affected name not recognized");
            break;
        }
    }

    private AudioClip GetAudioClip(string audioClipName)
    {
        if(audioClipName == "Step")
        {
            int rand = Random.Range(0, _footStepsSoundEffects.Length);
            return _footStepsSoundEffects[rand];
        }

        if(audioClipName == "OpenCloseDoor")
        {
            int rand = Random.Range(0, _openCloseDoorAudioClips.Length);
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
