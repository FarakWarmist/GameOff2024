using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.ActivateWinMenu();
            GameManager.Instance.DisablePlayer();
            GameManager.Instance.SetGameOver(true);
        }
    }
}
