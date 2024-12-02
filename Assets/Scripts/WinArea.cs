using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Timer.Instance.StopTimer();

            UIManager.Instance.ActivateWinMenu();
            GameManager.Instance.DisablePlayer();
            GameManager.Instance.SetGameOver(true);

            DataContainer loadedData = SaveManager.Instance.Load();
            if(loadedData != null)
            {
                if(loadedData.bestTime != 0)
                {
                    if(loadedData.bestTime > Timer.Instance.GetTime())
                    {
                        SaveManager.Instance.SaveBestTime(Timer.Instance.GetTime());
                    }
                }
                else
                    SaveManager.Instance.SaveBestTime(Timer.Instance.GetTime());
            }
            else
                SaveManager.Instance.Save(0.5f, 0.5f, Timer.Instance.GetTime(), 0.5f);
        }
    }
}
