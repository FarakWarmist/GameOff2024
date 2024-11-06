using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Vector3 playerPosition {get; private set;}

    public void SetPlayerPosition(Vector3 pos) => playerPosition = pos;
}
