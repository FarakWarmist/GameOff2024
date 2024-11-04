using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private bool _locked;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _startingYRot;
    [SerializeField] private float _openedRotationY;
    [SerializeField] private bool _opening;
    [SerializeField] private bool _closing;

    void Start()
    {
        _startingYRot = transform.parent.transform.rotation.eulerAngles.y;
        if(_startingYRot == 0) _startingYRot = 360;
        _openedRotationY = _startingYRot - 90;
        if(_openedRotationY < 0) _openedRotationY = 1f;
    }

    void Update()
    {
        if(_opening)
        {
            transform.parent.transform.Rotate(Vector3.down * _rotationSpeed * Time.deltaTime);
            if(transform.parent.transform.rotation.eulerAngles.y <= _openedRotationY)
            {
                transform.parent.transform.rotation = Quaternion.Euler(0, _openedRotationY, 0);
                _opening = false;
            }
        }

        if(_closing)
        {
            Debug.Log("Euler: " + transform.parent.transform.rotation.eulerAngles.y);
            transform.parent.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);

            float rotY = _startingYRot;
            if(rotY == 360) rotY = 359f;

            if(transform.parent.transform.rotation.eulerAngles.y >= rotY)
            {
                transform.parent.transform.rotation = Quaternion.Euler(0, rotY, 0);
                _closing = false;
            }
        }
    }

    public override void Interact(int itemIndex, int slotIndex)
    {
        //Open the door
        Debug.Log("Interacted with door | T: " + Time.time);

        if(_opening == false && _closing == false)
        {
            if(_locked)
            {
                if(itemIndex == 1)
                {
                    Inventory.Instance.UpdateInventorySlot(slotIndex, null);
                    OpenOrClose();
                    _locked = false;
                }
            }
            else
            {
                OpenOrClose();
            }
        }

    }

    private void OpenOrClose()
    {
        float rotY = transform.parent.transform.rotation.eulerAngles.y;
        if(rotY == 0) rotY = 360f; 

        if(rotY == _startingYRot)
            _opening = true;
        else
            _closing = true;
    }

}
