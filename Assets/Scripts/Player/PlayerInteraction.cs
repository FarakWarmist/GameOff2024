using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private bool _objectOnRange;
    [SerializeField] private GameObject _interactableGameobject;
    [SerializeField] private float _rangeForInteraction = 2;
    [SerializeField] private LayerMask _lM;
    [SerializeField] private bool _debug;

    void Start() => GameManager.Instance.SetPlayerInteractionScript(this);

    void Update()
    {
        CheckForCollectables();
        ObjectInteraction();
    }

    private void ObjectInteraction()
    {
        if(_objectOnRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Interactable interactableScr = _interactableGameobject.GetComponent<Interactable>();
                if(interactableScr != null)
                    interactableScr.Interact(Inventory.Instance.GetSelectedItemId(), Inventory.Instance._selectedSlot);
            }
        }
    }

    private void CheckForCollectables()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(rayChecker, out RaycastHit hit, Mathf.Infinity, _lM))
        {
            // Debug.Log("Distance: " + Vector3.Distance(Camera.main.transform.position, hit.point) + " | T: " + Time.time);
            if(Vector3.Distance(Camera.main.transform.position, hit.point) < _rangeForInteraction)
            {
                _objectOnRange = true;
                _interactableGameobject = hit.transform.gameObject;
            }
            else
                _objectOnRange = false;
        }
        else
            _objectOnRange = false;

        UIManager.Instance.SetInteractableOnRange(_objectOnRange);

        if(_debug) DebugRay();
    }

    private void DebugRay()
    {
        Ray rayChecker = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position,  rayChecker.direction * _rangeForInteraction, Color.green);
    }
}
