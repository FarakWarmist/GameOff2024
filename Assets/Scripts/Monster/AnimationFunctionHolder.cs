using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationFunctionHolder : MonoBehaviour
{
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
        Debug.Log("Distance: " + dist + "| T: " + Time.time);
        if(dist < _maxDistance)
        {
            if(dist < _minDistance)
            {
                return 1;
            }
            else
            {
                float x = _maxDistance - _minDistance;
                float res = dist / x;
                return res;
            }
        }
        else return 0;
    }
}
