using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkingArea : MonoBehaviour
{
    private NPC nPCScr;

    void Start()
    {
        nPCScr = transform.parent.transform.GetComponent<NPC>();
        if(nPCScr == null) Debug.LogError("NPC Script is NULL");
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.SetDialogueActivity(false);
            nPCScr.LeaveTalkingArea();
        }
    }
}
