using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMonsterCloseCues : MonoBehaviour
{
    [SerializeField] private float _startingFXChange = 8;
    [SerializeField] private FilmGrain _filmGrain;
    [SerializeField] private LensDistortion _lensDistortion;
    void Start()
    {
        GameObject globalVolumeObj = GameObject.FindGameObjectWithTag("GlobalVolume");
        if(globalVolumeObj != null)
        {
            VolumeProfile volumeProfile = globalVolumeObj.GetComponent<UnityEngine.Rendering.Volume>()?.profile;
            if(!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

            // You can leave this variable out of your function, so you can reuse it throughout your class.
            UnityEngine.Rendering.Universal.FilmGrain filmGrain;
            LensDistortion lensDistortion;

            if(!volumeProfile.TryGet(out filmGrain)) throw new System.NullReferenceException(nameof(filmGrain));
            if(!volumeProfile.TryGet(out lensDistortion)) throw new System.NullReferenceException(nameof(lensDistortion));

            _filmGrain = filmGrain;
            _lensDistortion = lensDistortion;

        }
    }

    void Update()
    {
        float distance = Vector3.Distance(GameManager.Instance.playerPosition, GameManager.Instance.monsterPosition);

        if(distance <= _startingFXChange)
        {
            float percentage = 1f - distance / _startingFXChange;
            percentage += 0.15f;

            if(percentage > 0.2f)
                _filmGrain.intensity.Override(percentage);
            else
                _filmGrain.intensity.Override(0.2f);

            if(_lensDistortion.intensity.value < 0.7f)
            {
                _lensDistortion.intensity.Override(_lensDistortion.intensity.value + 0.6f * Time.deltaTime);
                if(_lensDistortion.intensity.value > 0.7f)
                {
                    _lensDistortion.intensity.Override(0.7f);
                }
            }
        }
        else
        {
            _filmGrain.intensity.Override(0.2f);

            if(_lensDistortion.intensity.value > 0)
            {
                _lensDistortion.intensity.Override(_lensDistortion.intensity.value - 0.6f * Time.deltaTime);
                if(_lensDistortion.intensity.value < 0f)
                {
                    _lensDistortion.intensity.Override(0);
                }
            }
        }
    }
}
