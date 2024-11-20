using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : MonoBehaviour
{
    NavMeshAgent _navAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed = 12.5f;
    enum MonsterState
    {
        Idle, //It moves from time to time, and goes to specific places sometimes
        Chasing, //Going towards the player
        Investigating, //Goes toward a sound or thing that caughts its attention
        SearchingPlayer//Goes to the last place it saw the player
    };

    [SerializeField] private MonsterState _currentMonsterState;
    private bool _playerCaught;

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
    [SerializeField] private GameObject _roamingPlacesObject;
    [SerializeField] private List<Transform> _roamingPlaces = new List<Transform>();
    [SerializeField] private float _minTimeToRoam = 35f;
    [SerializeField] private float _maxTimeToRoam = 120f;
    [SerializeField] private bool _isRoaming;
    private float _roamingTimer;

    //* CHASING VARIABLES
    [SerializeField] private bool _seeingPlayer;
    [SerializeField] private bool _sawPlayerHiding;

    //* SEARCHING PLAYER
    [SerializeField] private float _searchingTime = 10f;
    private float _searchingTimer;
    [SerializeField] private float _graceTime = 5f;
    private float _graceTimer;
    private float _graceTimer2;
    private bool _hasGrace = false;
    private bool _onSearchingPoint;


    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        if(_navAgent == null) Debug.LogError("Nav Mesh Agent is NULL");

        _navAgent.speed = _speed;
        _idleTimer = Time.time + GetIdleTime();
        _investigationTimer = Time.time + _investigationTime;
        _roamingTimer = Time.time + UnityEngine.Random.Range(_minTimeToRoam, _maxTimeToRoam);

        GameManager.OnHidingChanged += PlayerHidingChanged;
        // PlayerTestNoise.OnNoiseMade += Noise;
        NoiseManager.OnNoiseMade += Noise;

        foreach(Transform t in _roamingPlacesObject.GetComponentInChildren<Transform>())
        {
            _roamingPlaces.Add(t);
        }
    }

    void OnDisable()
    {
        // PlayerTestNoise.OnNoiseMade -= Noise;
        NoiseManager.OnNoiseMade -= Noise;
        GameManager.OnHidingChanged -= PlayerHidingChanged;
    }

    void Update()
    {
        if(_playerCaught == false)
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

            case MonsterState.Investigating:
                Investigating();
            break;

            case MonsterState.SearchingPlayer:
                SearchingPlayer();
            break;
        }

        _animator.SetBool("Moving", _navAgent.velocity.x != 0 || _navAgent.velocity.z != 0);

        if(_seeingPlayer && _currentMonsterState != MonsterState.Chasing)
        {
            _currentMonsterState = MonsterState.Chasing;
            _hasGrace = false;
        }

        GameManager.Instance.SetMonsterPosition(transform.position);
    }

    private void Idle()
    {
        if(_roamingTimer <= Time.time && _isRoaming == false)
        {
            _isRoaming = true;
            
            _navAgent.SetDestination(_roamingPlaces[UnityEngine.Random.Range(0, _roamingPlaces.Count)].position);
        }

        if(_isRoaming == false)
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
        else
        {
            float distance = Vector3.Distance(_navAgent.destination, transform.position);
            if(distance <= 2.5f)
            {
                _isRoaming = false;
                _roamingTimer = Time.time + UnityEngine.Random.Range(_minTimeToRoam, _maxTimeToRoam);
            }
        }
    }

    private float GetIdleTime() => UnityEngine.Random.Range(_idleTimeMin, _idleTimeMax);


    private void Chasing()
    {
        if(_seeingPlayer)
        {
            _navAgent.SetDestination(GameManager.Instance.playerPosition);
            _animator.SetBool("Running", true);
        }
        else
        {
            if(_sawPlayerHiding)
            {
                _navAgent.SetDestination(GameManager.Instance.playerPosition);
                _animator.SetBool("Running", true);
            }
            else
            {
                _currentMonsterState = MonsterState.SearchingPlayer;
                _graceTimer = Time.time + _graceTime;
                _graceTimer2 = Time.time + 1;
                _hasGrace = true;
                _animator.SetBool("Running", true);
            }
        }
    }

    public void SetSeeingPlayer(bool value) => _seeingPlayer = value;

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
            // Debug.Log("Distance: " + distance);
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
        if(_hasGrace)
        {
            if(_graceTimer2 <= Time.time)
            {
                _navAgent.SetDestination(GameManager.Instance.playerPosition);
                _graceTimer2 = Time.time + 1;
            }

            if(_graceTimer <= Time.time)
            {
                _hasGrace = false;
            }
        }
        else
        {
            if(_onSearchingPoint)
            {
                if(_searchingTimer <= Time.time)
                {
                    _onSearchingPoint = false;
                    _currentMonsterState = MonsterState.Idle;
                }
            }
            else
            {
                float distance = Vector3.Distance(_navAgent.destination, transform.position);

                if(distance <= 2.5f)
                {
                    _onSearchingPoint = true;
                    _animator.SetBool("Running", false);
                    _searchingTimer = Time.time + _searchingTime;
                }
            }
        }

    }

    private void PlayerHidingChanged(bool value)
    {
        _sawPlayerHiding = _seeingPlayer;
    }

    public void SetPlayerCaught(bool value)
    {
        _playerCaught = value;
        if(value == false)
        {
            _navAgent.SetDestination(transform.position);
            _navAgent.isStopped = true;
        }
        else
        {
            _navAgent.isStopped = false;
        }
    }
}
