using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class AnimationFunctionHolder : MonoBehaviour
{
    private float _musicVolume;
    private float _effectsVolume;
    [SerializeField] private float _maxDistance = 12f;
    [SerializeField] private float _minDistance = 3f;
    public void FadeToLose()
    {
        UIManager.Instance.ActivateLoseMenu();
        AudioManager.Instance.PlaySFX("MonsterBite");
    }

    public void Growl()
    {
        AudioManager.Instance.PlaySFX("MonsterGrowl");
    }

    public void MonsterStep()
    {
        AudioManager.Instance.PlaySFX("MonsterStep", GetVolume());
    }

    private float GetVolume()
    {
        float dist = Vector3.Distance(GameManager.Instance.playerPosition, GameManager.Instance.monsterPosition);
        // Debug.Log("Distance: " + dist + "| T: " + Time.time);
        if(dist < _maxDistance)
        {
            if(dist < _minDistance)
            {
                return 1;
            }
            else if(dist > _maxDistance)
            {
                return 0;
            }
            else
            {
                float x = _maxDistance - _minDistance;
                float res = 1 - (dist / x);
                res *= 1.175f;
                if(res >= 1) res = 1;
                return res * _effectsVolume;
            }
        }
        else return 0;
    }

    public void ChangeSoundEffectsVolume(float volume)
    {
        _effectsVolume = volume;
    }

    public void ChangeMusicVolume(float volume)
    {
        _musicVolume = volume;
    }
}
