using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();
            if(door != null) door.OpenDoor();
        }
    }
}
