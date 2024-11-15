using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HidingPlace : Interactable
{

    [SerializeField] private float _noiseWhenHiding = 6.5f;
    PlayerMovement pMS;
    [SerializeField] private Transform _hidingPoint;
    [SerializeField] private Transform _leavingPoint;
    [SerializeField] private float _interpolationTime = 0.25f;
    private Vector3 playerPositionWhenEntering;
    private float enteringStartTime;
    private float exitingStartTime;
    private bool _isExiting;
    private bool _playerHiding;
    private bool _positionInterpolated;

    public override void Interact(int itemIndex, int slotIndex)
    {
        if(_playerHiding == false)
        {
            if(pMS == null) pMS = GameManager.Instance.playerMovementScr;

            playerPositionWhenEntering = GameManager.Instance.playerPosition;
            enteringStartTime = Time.time;

            NoiseManager.Instance.MakeNoise(_noiseWhenHiding, transform.position);

            pMS.SetCanMove(false);
            pMS.SetPlayerColliderTrigger(true);
            _playerHiding = true;
        }
    }

    void Update()
    {
        if(_playerHiding)
        {
            if(_positionInterpolated)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(pMS == null) pMS = GameManager.Instance.playerMovementScr;
                    exitingStartTime = Time.time;
                    _isExiting = true;
                    _playerHiding = false;
                    _positionInterpolated = false;
                }
            }
            else
            {
                float fracComplete = (Time.time - enteringStartTime) / _interpolationTime;
                pMS.SetPlayerPosition(Vector3.Slerp(playerPositionWhenEntering, _hidingPoint.position, fracComplete));
                if((enteringStartTime + _interpolationTime) < Time.time)
                {
                    _positionInterpolated = true;
                    GameManager.Instance.SetIsHiding(true);
                }
            }
        }

        if(_positionInterpolated == false && _isExiting)
        {
            float fracComplete = (Time.time - exitingStartTime) / _interpolationTime;
            pMS.SetPlayerPosition(Vector3.Slerp(_hidingPoint.position, _leavingPoint.position, fracComplete));
            if((exitingStartTime + _interpolationTime) < Time.time)
            {
                pMS.SetPlayerColliderTrigger(false);
                pMS.SetCanMove(true);
                _isExiting = false;
                GameManager.Instance.SetIsHiding(false);
            }
        }
    }

}
