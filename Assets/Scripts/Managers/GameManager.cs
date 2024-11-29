using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    private int buildIndex;

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
    public bool playerCaught {get; private set;}
    public bool isGameOver {get; private set;}

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;

    public void SetPlayerMovementScript(PlayerMovement pMS) => playerMovementScr = pMS;
    public void SetPlayerInventoryScript(PlayerInventory pIS) => playerInventoryScr = pIS;
    public void SetPlayerInteractionScript(PlayerInteraction pIS) => playerInteractionScr = pIS;
    public void SetPlayerGameObject(GameObject gO) => playerGameObject = gO;
    public void SetPlayerStayCenteredScript(PlayerStayCenteredOnParent pSCOP) => playerStayCenteredOnParentScr = pSCOP;

    void Start()
    {
        buildIndex = SceneManager.Instance.GetBuildIndex();
        if(buildIndex != 0)
            Cursor.lockState = CursorLockMode.Locked;

        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData == null)
        {
            SaveManager.Instance.Save(0.5f, 0.5f, 0);
        }
    }

    void Update()
    {
        CursorLocking();

        if(buildIndex != 0 && isGameOver == false)
            EscapeToPause();
    }

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
        if(playerCaught)
            playerStayCenteredOnParentScr.SetStayCentered(true);
    }

    public void SetGameOver(bool value) => isGameOver = value;
    public void SetPlayerCaught(bool value) => playerCaught = value;

    private void CursorLocking()
    {
        if(SceneManager.Instance.GetBuildIndex() == 0) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void EscapeToPause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.TogglePause();
        }
    }
}
