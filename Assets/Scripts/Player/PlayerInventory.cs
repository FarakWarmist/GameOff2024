using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private bool _objectOnRange;
    private GameObject _collectableGameobject;
    [Header("Raycasting")]
    [SerializeField] private bool _debug;
    [SerializeField] private LayerMask _lM;
    [SerializeField] private float _rangeForCollection = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckForCollectables();
        CursorLocking();

        if(_objectOnRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Collectable collectableScr = _collectableGameobject.GetComponent<Collectable>();
                Inventory.Instance.Collect(collectableScr.GetItemData(), _collectableGameobject);
            }
        }

        if(Input.mouseScrollDelta.y > 0)
        {
            Debug.Log("Up | T: " + Time.time);
            UIManager.Instance.ChangeSlot(true);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Down | T: " + Time.time);
            UIManager.Instance.ChangeSlot(false);
        }
    }

    private void CursorLocking()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void CheckForCollectables()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayChecker, out RaycastHit hit, Mathf.Infinity, _lM))
        {
            Debug.Log("Distance: " + Vector3.Distance(Camera.main.transform.position, hit.point) + " | T: " + Time.time);
            if(Vector3.Distance(Camera.main.transform.position, hit.point) < _rangeForCollection)
            {
                _objectOnRange = true;
                _collectableGameobject = hit.transform.gameObject;
            }
            else
                _objectOnRange = false;
        }
        else
            _objectOnRange = false;

        if(_debug) DebugRay();
    }

    private void DebugRay()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position,  rayChecker.direction * _rangeForCollection, Color.green);
    }
}
