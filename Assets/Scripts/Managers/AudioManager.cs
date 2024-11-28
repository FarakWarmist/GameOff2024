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

    void Start()
    {
        InitializeSoundEffectsByName();
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

        return _soundsEffectsByName[audioClipName];
    }


    private void InitializeSoundEffectsByName()
    {

    }
}
