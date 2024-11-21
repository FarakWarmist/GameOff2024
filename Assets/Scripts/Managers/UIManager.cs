using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Image _staminaBarImage;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _loseMenu;
    [SerializeField] private GameObject[] _winLoseButtons = new GameObject[2];
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

    public void UpdateDialogueText(string text)
    {
        if(_dialogueText.transform.parent.gameObject.activeSelf == false) SetDialogueActivity(true);

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
}
