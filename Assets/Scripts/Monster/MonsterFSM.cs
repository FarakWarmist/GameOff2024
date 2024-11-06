using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterFSM : MonoBehaviour
{
    NavMeshAgent _navAgent;
    [SerializeField] private float _speed = 12.5f;
    enum MonsterState
    {
        Idle, //Not moving a lot and just standing
        Chasing, //Going towards the player
        Roaming, //Walking slowly around the place, maybe going to specific places from time to time
        Investigating, //Goes toward a sound or thing that caughts its attention
        SearchingPlayer//Goes to the last place it saw the player
    };

    [SerializeField] private MonsterState _currentMonsterState;

    //* IDLE VARIABLES
    [Header("IDLE")]
    [SerializeField] private float _idleTimeMin = 4f;
    [SerializeField] private float _idleTimeMax = 12f;
    [SerializeField] private float _idleDistance = 3.5f;
    private float _idleTimer;

    //*ROAMING VARIABLES

    //* INVESTIGATING VARIABLES
    [Header("INVESTIGATING")]
    [SerializeField] private float _investigationTime;
    [SerializeField] private float _activateInvestigationTreshold = 2f;
    private bool _onInvestigationPoint;
    private float _investigationTimer;
    private Vector3 _investigationPosition;

    //* CHASING VARIABLES
    [SerializeField] private bool _seeingPlayer;

    //* SEARCHING PLAYER


    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        if(_navAgent == null) Debug.LogError("Nav Mesh Agent is NULL");

        _navAgent.speed = _speed;
        _idleTimer = Time.time + GetIdleTime();
        _investigationTimer = Time.time + _investigationTime;

        PlayerTestNoise.OnNoiseMade += Noise;
    }

    void OnDisable()
    {
        PlayerTestNoise.OnNoiseMade -= Noise;
    }

    void Update()
    {
        FSM();
    }

    private void FSM()
    {
        switch(_currentMonsterState)
        {
            case MonsterState.Idle:
                Idle();
            break;

            case MonsterState.Chasing:
                Chasing();
            break;

            case MonsterState.Roaming:
                Roaming();
            break;

            case MonsterState.Investigating:
                Investigating();
            break;

            case MonsterState.SearchingPlayer:
                SearchingPlayer();
            break;
        }
    }

    private void Idle()
    {
        if(_idleTimer <= Time.time)
        {
            Vector3 currentPos = transform.position;
            currentPos.x += UnityEngine.Random.Range(-_idleDistance, _idleDistance);
            currentPos.z += UnityEngine.Random.Range(-_idleDistance, _idleDistance);

            if(NavMesh.SamplePosition(currentPos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                _navAgent.SetDestination(currentPos);
                _idleTimer = Time.time + GetIdleTime();
            }
        }
    }

    private float GetIdleTime() => UnityEngine.Random.Range(_idleTimeMin, _idleTimeMax);


    private void Chasing()
    {

    }

    public void SetSeeingPlayer(bool value) => _seeingPlayer = value;

    private void Roaming()
    {

    }

    private void Investigating()
    {
        if(_onInvestigationPoint)
        {
            if(_investigationTimer <= Time.time)
            {
                _onInvestigationPoint = false;
                _currentMonsterState = MonsterState.Idle;
                Debug.Log("Stopped Investigating");
            }
        }
        else
        {
            float distance = Vector3.Distance(_navAgent.destination, transform.position);
            Debug.Log("Distance: " + distance);
            if(distance <= 2.5f)
            {
                Debug.Log("On investigation point");
                _onInvestigationPoint = true;
                _investigationTimer = Time.time + _investigationTime;
            }
        }
    }

    public void Noise(float noiseVol, Vector3 position)
    {
        float distance = Vector3.Distance(position, transform.position);

        Debug.Log("Noise Captured: " + noiseVol / distance);

        if(noiseVol / distance >= _activateInvestigationTreshold)
        {
            _currentMonsterState = MonsterState.Investigating;
            _navAgent.SetDestination(position);
            _onInvestigationPoint = false;
        }
    }

    private void SearchingPlayer()
    {

    }
}
