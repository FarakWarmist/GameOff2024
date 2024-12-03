using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int buildIndex;
    [SerializeField] private Transform _droppingPoint;
    [SerializeField] private bool _objectOnRange;
    private GameObject _collectableGameobject;
    [SerializeField] private GameObject[] _itemModels;
    private Dictionary<int, GameObject> _itemByItemId = new Dictionary<int, GameObject>();
    [Header("Raycasting")]
    [SerializeField] private bool _debug;
    [SerializeField] private LayerMask _lM;
    [SerializeField] private float _rangeForCollection = 2f;

    void Start()
    {
        InitializeItemByItemIdDictionary();
        buildIndex = SceneManager.Instance.GetBuildIndex();

        GameManager.Instance.SetPlayerInventoryScript(this);
    }

    void Update()
    {
        if(buildIndex == 2)
        {
            CheckForCollectables();
            ObjectPicking();
            MouseScroll();
            ChangeSlotWithNumbers();
            // DropItem();
        }
    }

    private void ObjectPicking()
    {
        if(_objectOnRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Collectable collectableScr = _collectableGameobject.GetComponent<Collectable>();
                Inventory.Instance.Collect(collectableScr.GetItemData(), _collectableGameobject);
            }
        }
    }

    private void MouseScroll()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            // Debug.Log("Up | T: " + Time.time);
            UIManager.Instance.ChangeSlot(true);
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            // Debug.Log("Down | T: " + Time.time);
            UIManager.Instance.ChangeSlot(false);
        }
    }

    private void ChangeSlotWithNumbers()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.ChangeSlotWithIndex(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.Instance.ChangeSlotWithIndex(1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIManager.Instance.ChangeSlotWithIndex(2);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            UIManager.Instance.ChangeSlotWithIndex(3);
        }

        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            UIManager.Instance.ChangeSlotWithIndex(4);
        }
    }

    private void DropItem()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Inventory.Instance.DropItem(_droppingPoint.position);
        }
    }

    private void CheckForCollectables()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayChecker, out RaycastHit hit, Mathf.Infinity, _lM))
        {
            // Debug.Log("Distance: " + Vector3.Distance(Camera.main.transform.position, hit.point) + " | T: " + Time.time);
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

        UIManager.Instance.SetCollectableOnRange(_objectOnRange);

        if(_debug) DebugRay();
    }

    private void DebugRay()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position,  rayChecker.direction * _rangeForCollection, Color.green);
    }

    public void ToggleItem(bool value, int itemId) => _itemByItemId[itemId].SetActive(value);

    private void InitializeItemByItemIdDictionary()
    {
        _itemByItemId[1] = _itemModels[0];
        _itemByItemId[2] = _itemModels[1];
        _itemByItemId[3] = _itemModels[2];
        _itemByItemId[4] = _itemModels[3];
        _itemByItemId[5] = _itemModels[4];

        _itemByItemId[6] = _itemModels[5];
        _itemByItemId[7] = _itemModels[6];
        _itemByItemId[8] = _itemModels[7];

        _itemByItemId[11] = _itemModels[8];
        _itemByItemId[12] = _itemModels[9];

    }
}
