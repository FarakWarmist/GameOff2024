using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueData", menuName = "NPCDialogueData", order = 0)]
public class NPCDialogueData : ScriptableObject
{
    public NPCConversationData[] conversations;
}
