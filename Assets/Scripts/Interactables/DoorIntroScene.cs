using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorIntroScene : Interactable
{
    [SerializeField] private Animator _animator;
    public override void Interact(int itemIndex, int slotIndex)
    {
        _animator.SetTrigger("TransitionScene");
        GameManager.Instance.DisablePlayer();
        GameManager.Instance.DisablePlayerGO();
    }
}
