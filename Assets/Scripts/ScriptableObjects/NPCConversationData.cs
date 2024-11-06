using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCConversationData", menuName = "NPCConversationData", order = 0)]
public class NPCConversationData : ScriptableObject 
{
    public string[] conversationLines;
}
