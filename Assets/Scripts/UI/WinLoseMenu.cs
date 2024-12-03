using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinLoseMenu : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _mainMenuButtonImage;
    [SerializeField] private Image _restartButtonImage;
    [SerializeField] private TMP_Text _mainMenuButtonText;
    [SerializeField] private TMP_Text _restartButtonText;
    [SerializeField] private TMP_Text _menuSpecificText;
    [SerializeField] private float _fadeInSpeed = 1.5f;
    private Color _colorCache;
    [SerializeField] private Color _buttonColor;
    [SerializeField] private Color _textColor;
    private bool _fadedIn;

    void OnEnable()
    {
        if(_backgroundImage == null) Debug.LogError("Background image is not assigned");
        _colorCache = _backgroundImage.color;

        _mainMenuButtonImage.color = _buttonColor;
        _restartButtonImage.color = _buttonColor;

        _mainMenuButtonText.color = _textColor;
        _restartButtonText.color = _textColor;
        _menuSpecificText.color = _textColor;
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
