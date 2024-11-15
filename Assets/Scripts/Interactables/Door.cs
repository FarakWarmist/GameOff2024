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
    [SerializeField] private int _indexNeededToOpen = 1;

    void Start()
    {
        _startingYRot = transform.parent.transform.rotation.eulerAngles.y;
        if(_startingYRot == 0) _startingYRot = 360f;
        _openedRotationY = _startingYRot - 90;
    }

    void Update()
    {
        if(_opening)
        {
            Debug.Log("Euler: " + transform.parent.transform.rotation.eulerAngles.y);
            transform.parent.transform.Rotate(Vector3.down * _rotationSpeed * Time.deltaTime);

            if(_openedRotationY < 2)
            {
                float newOpenedRotation = 0;
                if(transform.parent.transform.rotation.eulerAngles.y <= 1f)
                {
                    transform.parent.transform.rotation = Quaternion.Euler(0, newOpenedRotation, 0);
                    _opening = false;
                }
            }
            else
            {
                if(transform.parent.transform.rotation.eulerAngles.y <= _openedRotationY)
                {
                    transform.parent.transform.rotation = Quaternion.Euler(0, _openedRotationY, 0);
                    _opening = false;
                }
            }

        }

        if(_closing)
        {
            // Debug.Log("Euler: " + transform.parent.transform.rotation.eulerAngles.y);
            transform.parent.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);

            if(_openedRotationY < 2)
            {
                if(transform.parent.transform.rotation.eulerAngles.y >= _startingYRot)
                {
                    transform.parent.transform.rotation = Quaternion.Euler(0, _startingYRot, 0);
                    _closing = false;
                }
            }
            else
            {
                if(transform.parent.transform.rotation.eulerAngles.y >= _startingYRot - 1)
                {
                    float startingYRotTransformed = _startingYRot;
                    if(_startingYRot == 360) startingYRotTransformed = 0;

                    transform.parent.transform.rotation = Quaternion.Euler(0, startingYRotTransformed, 0);
                    _closing = false;
                }
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
                if(itemIndex == _indexNeededToOpen)
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

        // Debug.LogWarning("rot: " + rotY + " | start: " + _startingYRot);
        if(rotY == _startingYRot)
            _opening = true;
        else
            _closing = true;
    }

    public void OpenDoor()
    {
        Debug.Log("Open Door | T: " + Time.time);
        if(_locked) return;
        
        if(_opening) return;

        float rotY = transform.parent.transform.rotation.eulerAngles.y;
        if(rotY == 0) rotY = 360f;

        if(rotY == _startingYRot) OpenOrClose();
    }

}
