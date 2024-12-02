using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private bool _onMainMenu;
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _characterNameText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;
    [SerializeField] private GameObject[] _winLoseButtons = new GameObject[2];
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private BlackScreen _blackScreenScr;
    [Header("Toggle UI")]
    [SerializeField] private GameObject _timerGO;
    [SerializeField] private GameObject _staminaBarGO;
    [SerializeField] private GameObject _inventoryGo;
    [Header("Crosshair")]
    [SerializeField] private RectTransform _crosshairRectTransform;
    [SerializeField] private float _crosshairNormalScale = 0.75f;
    [SerializeField] private float _crosshairMaxScale = 1f;
    [SerializeField] private float _crosshairInterpolationSpeed = 1.25f;
    [SerializeField] private RectTransform _interactWithEMenu;
    [SerializeField] private float _interactWithEInterpolationSpeed = 2.15f;
    private bool _interactableOnRange;
    private bool _collectableOnRange;

    void Update()
    {
        if(_onMainMenu) return;

        CrosshairAndInteractWithEAnimations();
    }

    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
        if(stamina == maxStamina)
            _staminaBarImage.transform.parent.gameObject.SetActive(false);
        else
        {
            _staminaBarImage.transform.parent.gameObject.SetActive(true);
            _staminaBarImage.fillAmount = stamina / maxStamina;
            // Debug.Log("Fill Amount: " + stamina / maxStamina);
        }
    }

    public void ChangeSlot(bool up) => _inventory.ChangeSlot(up);

    public void ChangeSlotWithIndex(int index) => _inventory.ChangeSlotWithIndex(index);

    public void UpdateDialogueText(string text, string characterName)
    {
        if(_dialogueText.transform.parent.gameObject.activeSelf == false) SetDialogueActivity(true);

        _characterNameText.text = characterName;

        _dialogueText.text = text;
    }

    public string GetCurrentDialogueText() => _dialogueText.text;

    public void SetDialogueActivity(bool value) => _dialogueText.transform.parent.gameObject.SetActive(value);

    public void UpdateTimerText(string text) => _timerText.text = text;

    public void ActivateWinMenu() => _winMenu.SetActive(true);

    public void ActivateLoseMenu() => _loseMenu.SetActive(true);

    public void ActivateWinLoseButtons()
    {
        foreach(GameObject b in _winLoseButtons)
            b.SetActive(true);
    }

    public void SetCollectableOnRange(bool value) => _collectableOnRange = value;
    public void SetInteractableOnRange(bool value) => _interactableOnRange = value;

    public void TogglePause()
    {
        if(_pauseMenu.activeSelf == false)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void ToggleUI(bool value)
    {
        _timerGO.SetActive(value);
        _inventoryGo.SetActive(value);
        _staminaBarGO.SetActive(value);
    }

    private void CrosshairAndInteractWithEAnimations()
    {
        if(_interactableOnRange || _collectableOnRange)
        {
            if(_crosshairRectTransform.localScale.x != _crosshairMaxScale)
            {
                float scale = _crosshairRectTransform.localScale.x;
                scale += Time.deltaTime * _crosshairInterpolationSpeed;
                if(scale >= _crosshairMaxScale)
                    scale = _crosshairMaxScale;

                _crosshairRectTransform.localScale = Vector3.one * scale;
            }

            if(_interactWithEMenu.localScale.x != 1)
            {
                if(_interactWithEMenu.transform.gameObject.activeSelf == false) 
                    _interactWithEMenu.transform.gameObject.SetActive(true);

                float scale = _interactWithEMenu.localScale.x;
                scale += Time.deltaTime * _interactWithEInterpolationSpeed;
                if(scale >= 1)
                    scale = 1;

                _interactWithEMenu.localScale = Vector3.one * scale;
            }
        }
        else
        {
            if(_crosshairRectTransform.localScale.x != _crosshairNormalScale)
            {
                float scale = _crosshairRectTransform.localScale.x;
                scale -= Time.deltaTime * _crosshairInterpolationSpeed;
                if(scale <= _crosshairNormalScale)
                    scale =_crosshairNormalScale;

                _crosshairRectTransform.localScale = Vector3.one * scale;
            }

            if(_interactWithEMenu.transform.gameObject.activeSelf != false)
            {
                float scale = _interactWithEMenu.localScale.x;
                scale -= Time.deltaTime * _interactWithEInterpolationSpeed;
                if(scale <= 0)
                {
                    scale = 0;
                    _interactWithEMenu.localScale = Vector3.one * scale;
                    _interactWithEMenu.transform.gameObject.SetActive(false);
                }

                _interactWithEMenu.localScale = Vector3.one * scale;
            }
        }
    }

    public void FadeTo(bool fadeIn = true) => _blackScreenScr.FadeTo(fadeIn);
}
