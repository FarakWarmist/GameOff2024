using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private float _fadeInSpeed = 1.5f;
    private Color _colorCache; 
    private bool _fadedIn;

    void OnEnable()
    {
        if(_backgroundImage == null) Debug.LogError("Background image is not assigned");
        _colorCache = _backgroundImage.color;
    }

    void Update()
    {
        if(_fadedIn == false)
        {
            float colorAlpha = _backgroundImage.color.a;
            colorAlpha += (Time.deltaTime * _fadeInSpeed);

            if(colorAlpha >= 1f)
            {
                colorAlpha = 1f;
                _fadedIn = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            _colorCache.a = colorAlpha;

            _backgroundImage.color = _colorCache;

            if(_fadedIn)
                UIManager.Instance.ActivateWinLoseButtons();
        }
    }
}
