using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] MonsterFSM _monsterFSMScr;
    [SerializeField] private LayerMask _lM;
    [SerializeField] private bool _debug;
    [SerializeField] private bool _playerOnDetectionArea;

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
}
