using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 playerPosition {get; private set;}
    public PlayerMovement playerMovementScr {get; private set;}
    public bool isHiding {get; private set;}
    public static event Action<bool> OnHidingChanged;

    public Vector3 monsterPosition {get; private set;}

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;
    public void SetPlayerMovementScript(PlayerMovement pMS) => playerMovementScr = pMS;
    public void SetIsHiding(bool value)
    {
        isHiding = value;
        if(OnHidingChanged != null) OnHidingChanged(isHiding);
    }

    public void SetMonsterPosition(Vector3 pos) => monsterPosition = pos;
}
