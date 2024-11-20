using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStayCenteredOnParent : MonoBehaviour
{
    public bool stayCentered {get; private set;}

    void Start() => GameManager.Instance.SetPlyaerStayCenteredScript(this);

    void FixedUpdate()
    {
        if(stayCentered)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void SetStayCentered(bool value) => stayCentered = value;
}
