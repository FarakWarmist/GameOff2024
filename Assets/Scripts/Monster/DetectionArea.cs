using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] MonsterFSM _monsterFSMScr;
    [SerializeField] private LayerMask _lM;
    [SerializeField] private bool _debug;
    [SerializeField] private bool _playerOnDetectionArea;
    [SerializeField] private float _grabbingPlayerDistance = 1.5f;
    [SerializeField] private Transform _playerPositionTransform;
    [SerializeField] private Animator _animator;
    private bool _playerCaught;
    private bool _interpolatePlayerTransform;
    private float _interpolationTime;

    void Update()
    {
        if(_playerOnDetectionArea)
        {
            CheckSeeingPlayer();
        }
        else
        {
            _monsterFSMScr.SetSeeingPlayer(false);
        }

        if(_playerCaught == false && Vector3.Distance(GameManager.Instance.playerPosition, transform.parent.transform.position) < _grabbingPlayerDistance)
        {
            GrabPlayer();
            _playerCaught = true;
        }

        if(_playerCaught && _interpolatePlayerTransform)
        {
            _interpolationTime += Time.deltaTime;

            GameManager.Instance.playerGameObject.transform.localPosition = Vector3.Slerp(
                GameManager.Instance.playerGameObject.transform.localPosition,
                Vector3.zero,
                _interpolationTime);

            GameManager.Instance.playerGameObject.transform.localRotation = Quaternion.Slerp(
                GameManager.Instance.playerGameObject.transform.localRotation,
                Quaternion.Euler(Vector3.zero),
                _interpolationTime);

            if(GameManager.Instance.playerGameObject.transform.localPosition == Vector3.zero && GameManager.Instance.playerGameObject.transform.localRotation == Quaternion.Euler(Vector3.zero))
            {
                _interpolatePlayerTransform = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerOnDetectionArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerOnDetectionArea = false;
        }
    }

    private void CheckSeeingPlayer()
    {
        if(_debug) DebugRay();

        if(Physics.Raycast(transform.position,
                            GameManager.Instance.playerPosition - transform.position,
                            out RaycastHit hit,
                            Mathf.Infinity,
                            _lM ))
        {
            if(hit.collider.CompareTag("Player"))
            {
                _monsterFSMScr.SetSeeingPlayer(true);
            }
            else
            {
                _monsterFSMScr.SetSeeingPlayer(false);
            }
        }
        else
        {
            _monsterFSMScr.SetSeeingPlayer(false);
        }
    }

    private void DebugRay()
    {
        Debug.DrawRay(transform.position, GameManager.Instance.playerPosition - transform.position, Color.blue);
    }

    private void GrabPlayer()
    {
        GameManager.Instance.DisablePlayer();
        GameManager.Instance.playerMovementScr.ResetCameraRotation();
        _monsterFSMScr.SetPlayerCaught(true);
        //*Interpolate the player position to the player spot on the monster
        GameManager.Instance.playerGameObject.transform.parent = _playerPositionTransform;
        _interpolatePlayerTransform = true;
        _interpolationTime = 0;
        //*Activate the monster attacking animation(eating the player)
        _animator.SetTrigger("Attack");
        //*Game over screen on animation finish
    }
}
