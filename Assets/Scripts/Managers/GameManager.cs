using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 playerPosition {get; private set;}
    public Quaternion playerRotation {get; private set;}
    public PlayerMovement playerMovementScr {get; private set;}

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;
    public void SetPlayerRotation(Quaternion rot) => playerRotation = rot;
    public void SetPlayerMovementScript(PlayerMovement pMS) => playerMovementScr = pMS;
}
