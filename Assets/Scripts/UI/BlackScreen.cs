using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    [SerializeField] private Image _blackImage;
    [SerializeField] private float _fadingSpeed = 2.5f;
    Color _colorHolder;
    bool _fading;
    bool _fadingIn;

    void Start()
    {
        _colorHolder = _blackImage.color;
    }

    void Update()
    {
        if(_fading)
        {
            if(_fadingIn)
            {
                float opacity = _colorHolder.a;
                opacity += Time.deltaTime * _fadingSpeed;
                Debug.Log("Opacity: " + opacity);
                if(opacity >= 1)
                {
                    opacity = 1;
                    _fading = false;
                }
                _colorHolder.a = opacity;
                _blackImage.color = _colorHolder;
            }
            else
            {
                float opacity = _colorHolder.a;
                opacity -= Time.deltaTime * _fadingSpeed;
                if(opacity <= 0)
                {
                    opacity = 0;
                    _fading = false;
                }
                _colorHolder.a = opacity;
                _blackImage.color = _colorHolder;
            }
        }
    }

    public void FadeTo(bool fadeIn = true)
    {
        _fading = true;
        _fadingIn = fadeIn;
    }
}
