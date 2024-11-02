using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private Image _staminaBarImage;
    public void UpdateStaminaBar(float stamina, float maxStamina)
    {
        if(stamina == maxStamina)
            _staminaBarImage.transform.parent.gameObject.SetActive(false);
        else
        {
            _staminaBarImage.transform.parent.gameObject.SetActive(true);
            _staminaBarImage.fillAmount = stamina / maxStamina;
            Debug.Log("Fill Amount: " + stamina / maxStamina);
        }
    }
}
