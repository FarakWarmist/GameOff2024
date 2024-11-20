using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 playerPosition {get; private set;}
    public PlayerMovement playerMovementScr {get; private set;}
    public PlayerInventory playerInventoryScr {get; private set;}
    public PlayerInteraction playerInteractionScr {get; private set;}
    public PlayerStayCenteredOnParent playerStayCenteredOnParentScr {get; private set;}

    public GameObject playerGameObject {get; private set;}
    public bool isHiding {get; private set;}
    public static event Action<bool> OnHidingChanged;
    public int[] _safeBoxCode {get; private set;} = new int[3];

    public Vector3 monsterPosition {get; private set;}

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;

    public void SetPlayerMovementScript(PlayerMovement pMS) => playerMovementScr = pMS;
    public void SetPlayerInventoryScript(PlayerInventory pIS) => playerInventoryScr = pIS;
    public void SetPlayerInteractionScript(PlayerInteraction pIS) => playerInteractionScr = pIS;
    public void SetPlayerGameObject(GameObject gO) => playerGameObject = gO;
    public void SetPlyaerStayCenteredScript(PlayerStayCenteredOnParent pSCOP) => playerStayCenteredOnParentScr = pSCOP;

    public void SetIsHiding(bool value)
    {
        isHiding = value;
        if(OnHidingChanged != null) OnHidingChanged(isHiding);
    }

    public void SetMonsterPosition(Vector3 pos) => monsterPosition = pos;

    public void SetSafeBoxCode(int code1, int code2, int code3)
    {
        _safeBoxCode[0] = code1;
        _safeBoxCode[1] = code2;
        _safeBoxCode[2] = code3;
    }

    public void DisablePlayer()
    {
        playerMovementScr.enabled = false;
        playerInventoryScr.enabled = false;
        playerInteractionScr.enabled = false;
        playerStayCenteredOnParentScr.SetStayCentered(true);
    }
}
