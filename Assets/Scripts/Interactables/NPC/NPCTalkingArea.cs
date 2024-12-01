using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkingArea : MonoBehaviour
{
    private NPC nPCScr;
    private NPCIntro nPCScrIntro;

    void Start()
    {
        nPCScr = transform.parent.transform.GetComponent<NPC>();
        if(nPCScr == null) nPCScrIntro = transform.parent.transform.GetComponent<NPCIntro>();
        if(nPCScr == null && nPCScrIntro == null) Debug.LogError("NPC Script is NULL");
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UIManager.Instance.SetDialogueActivity(false);
            if(nPCScr != null)
                nPCScr.LeaveTalkingArea();
            else
                nPCScrIntro.LeaveTalkingArea();
        }
    }
}
