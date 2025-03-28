using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private Vector3 _movementVector;
    private bool _isGrounded;
    private bool _canMove = true;
    [SerializeField] private CapsuleCollider _playerCollider;
    [Header("PHYSICS")]
    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _maxFallingSpeedStairs = -1;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canRun;
    [SerializeField] private float _runningSpeedMultiplier;
    private bool _running;
    [Header("CAMERA")]
    [SerializeField] private Camera _playerCamera;
    const float maxSensitivity = 240;
    [SerializeField] private float _horizontalSensitivity;
    [SerializeField] private float _verticalSensitivity;
    [SerializeField] private float _minVerticalRot;
    [SerializeField] private float _maxVerticalRot;
    private float _horizontalRot;
    private float _verticalRot;
    private Vector2 _cameraRotation;

    [Header("HEAD BOB")]
    [SerializeField] private float _normalCameraYPos = 0.45f;
    [SerializeField] private float _omegaYWalking = 5.0f;
    [SerializeField] private float _omegaYRunning = 6.5f;
    [SerializeField] private float _headBobYOffset = 1.25f;
    [SerializeField] private float _amplitudeYWalking = 0.1f;
    [SerializeField] private float _amplitudeYRunning = 0.2f;
    [SerializeField] private float _interpolationTime = 0.025f;
    [SerializeField] private float _minCamYLocalPos = -0.55f;
    private bool _madeStepSound;
    private Vector3 _cameraPosition;
    private float _startingCameraYPos;
    float index;
    [SerializeField] private float _normalYPosInterpolationTime;
    private float _startTime;
    private bool _isStartTimeSet;

    [Header("GROUND DETECTION")]
    [SerializeField] private float _rayOffsetCenter;
    [SerializeField] private float _rayOffsetY;
    [SerializeField] private float _rayDistance;
    private Vector3[] _rayOrigins = new Vector3[5];
    private RaycastHit[] _raycastHits = new RaycastHit[5];
    private Ray[] _rays = new Ray[5];
    private List<int> _currentCollisionInstanceIds = new List<int>();
    [SerializeField] private bool _isGroundedRay;
    [SerializeField] private Vector3 _groundNormal;
    [Header("STAMINA")]
    [SerializeField] private float _stamina = 100f;
    private float _maxStamina;
    [SerializeField] private float _staminaSpentWhileRunning = 1f;
    [SerializeField] private float _staminaRecovery = 2f;
    [SerializeField] private float _staminaRecoveryTime = 2f;
    private float _staminaRecoveryTimer;
    [Header("MAKING NOISE")]
    [SerializeField] private float _noiseWhenWalking = 7.5f;
    [SerializeField] private float _noiseWhenRunning = 15f;

    void Start()
    {
        Initialize();
        _maxStamina = _stamina;
        _staminaRecoveryTimer = Time.time + _staminaRecoveryTime;

        _startingCameraYPos = Camera.main.transform.position.y;
        _cameraPosition.x = Camera.main.transform.position.x;
        _cameraPosition.z = Camera.main.transform.position.z;

        GameManager.Instance.SetPlayerMovementScript(this);
        GameManager.Instance.SetPlayerGameObject(this.gameObject);

        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _horizontalSensitivity = maxSensitivity * loadedData.mouseSensitivity;
            _verticalSensitivity = _horizontalSensitivity;
        }

        SaveManager.OnMouseSensitivityChanged += ChangeMouseSensitivity;
    }

    void OnDisable()
    {
        _rb.velocity = Vector3.zero;
        SaveManager.OnMouseSensitivityChanged -= ChangeMouseSensitivity;
    }

    private void ChangeMouseSensitivity(float value)
    {
        _horizontalSensitivity = maxSensitivity * value;
        _verticalSensitivity = _horizontalSensitivity;
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        if(_rb == null)
        {
            Debug.LogError("Rigidbody is NULL");
        }
    }
    void Update()
    {
        // if(_canMove)
        // {
        //     _isGroundedRay = GroundDetection();
        //     DebugRaycasts();
        //     CalculateMovement();
        // }
        // LookRotation();
        // Debug.Log("Movement Vector: " + _movementVector + " | Grounded: " + _isGrounded);
        GetInput();
        LookRotation();
        StaminaHandler();
    }

    void FixedUpdate()
    {
        if(_canMove)
        {
            _isGroundedRay = GroundDetection();

            CalculateMovement();
        }
        // LookRotation();
    }

    private void GetInput()
    {
        // _movementVector.x = Input.GetAxis("Horizontal") * _speed;
        // _movementVector.z = Input.GetAxis("Vertical") * _speed;

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _running = true;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _running = false;
        }
    }

    private void CalculateMovement()
    {
        _movementVector.x = Input.GetAxis("Horizontal") * _speed;
        _movementVector.z = Input.GetAxis("Vertical") * _speed;

        HeadBob();

        // if(Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     _running = true;
        // }
        
        // if(Input.GetKeyUp(KeyCode.LeftShift))
        // {
        //     _running = false;
        // }

        if(_isGrounded && _isGroundedRay)
        {
            // if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            // {
            //     _movementVector.y = _jumpForce;
            // }

            if(_running && _canRun && _stamina > 0)
            {
                _movementVector.x *= _runningSpeedMultiplier;
                _movementVector.z *= _runningSpeedMultiplier;
            }
            
            _movementVector.y = 0;
        }
        else
        {
            if(_movementVector.y <= _maxFallingSpeed)
            {
                _movementVector.y = _maxFallingSpeed;
            }
            else
            {
                _movementVector.y -= _gravity;
            }

            if(_groundNormal.y < 0.95f) _movementVector.y = _maxFallingSpeedStairs;
        }
        
        _movementVector *= Time.fixedDeltaTime;
            
        _rb.velocity = transform.TransformDirection(_movementVector);
        _rb.velocity.Normalize();

        GameManager.Instance.SetPlayerPosition(transform.position);

        MakeNoise();
    }

    private void HeadBob()
    {
        if(_movementVector.x != 0 || _movementVector.z != 0)
        {
            _isStartTimeSet = false;
            CameraSine();
        }
        else
        {
            if(_isStartTimeSet == false)
            {
                _startTime = Time.time;
                _isStartTimeSet = true;
            }

            if(Camera.main.transform.localPosition.y != _normalCameraYPos)
            {
                // Debug.Log("Cam Pos Y: " + Camera.main.transform.localPosition.y + " | Normal Cam Pos Y: " + _normalCameraYPos + " | T: " + Time.time);
                // if(Camera.main.transform.localPosition.y < (_normalCameraYPos + 0.01f) || Camera.main.transform.localPosition.y > (_normalCameraYPos - 0.01f))
                // {
                //     Debug.Log("Snap | T: " + Time.time);
                //     Vector3 camPos = Camera.main.transform.localPosition;
                //     camPos.y = _normalCameraYPos;
                //     Camera.main.transform.localPosition = camPos;
                // }
                float fracComplete = (Time.time - _startTime) / _normalYPosInterpolationTime;

                Vector3 currentCamVector = Camera.main.transform.localPosition;
                Vector3 normalVector = new Vector3(currentCamVector.x, _normalCameraYPos, currentCamVector.z);

                if(fracComplete > 0.1f || fracComplete < 0.1f)
                    Camera.main.transform.localPosition = Vector3.Slerp(currentCamVector, normalVector, fracComplete);

                // Debug.Log("Frac: " + fracComplete + " | Cam Local Pos: " + Camera.main.transform.localPosition + " | T: " + Time.time);
            }
        }
    }

    private void CameraSine()
    {
        index += Time.deltaTime;

        float y;
        if(_running)
            y = Mathf.Abs (_amplitudeYRunning*Mathf.Sin (_omegaYRunning*index));
        else
            y = Mathf.Abs (_amplitudeYWalking*Mathf.Sin (_omegaYWalking*index));


        y += (transform.position.y + _headBobYOffset);
        _cameraPosition.y = y;
        _cameraPosition.x = transform.localPosition.x;
        _cameraPosition.z = transform.localPosition.z;
        Camera.main.transform.position = _cameraPosition;

        // Debug.Log("Local Cam Y: " + Camera.main.transform.localPosition.y + " | T: " + Time.time);

        if(Camera.main.transform.localPosition.y < _minCamYLocalPos && _madeStepSound == false)
        {
            AudioManager.Instance.PlaySFX("Step");
            _madeStepSound = true;
        }
        else if(_madeStepSound && Camera.main.transform.localPosition.y > _minCamYLocalPos)
            _madeStepSound = false;

    }

    private void MakeNoise()
    {
        if(_movementVector.x != 0 || _movementVector.z != 0)
        {
            if(_running)
                NoiseManager.Instance.MakeNoise(_noiseWhenRunning, transform.position);
            else
                NoiseManager.Instance.MakeNoise(_noiseWhenWalking, transform.position);
        }
    }

    private void StaminaHandler()
    {
        bool isMoving = _movementVector.x != 0 || _movementVector.z != 0;

        if(_running && isMoving)
        {
            _stamina -= _staminaSpentWhileRunning * Time.deltaTime;
            if(_stamina < 0) _stamina = 0;
            _staminaRecoveryTimer = Time.time + _staminaRecoveryTime;
            UIManager.Instance.UpdateStaminaBar(_stamina, _maxStamina);
        }
        else
        {
            if(_staminaRecoveryTimer <= Time.time)
            {
                if(_stamina != _maxStamina)
                {
                    _stamina += _staminaRecovery * Time.deltaTime;
                    if(_stamina > _maxStamina)
                    {
                        _stamina = _maxStamina;
                    }
                    UIManager.Instance.UpdateStaminaBar(_stamina, _maxStamina);
                }
            }
        }

        if(_stamina < 0) _stamina = 0;
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Ground"))
        {
            _currentCollisionInstanceIds.Add(other.transform.GetInstanceID());
        }

        if(_currentCollisionInstanceIds.Count > 0)
        {
            // _movementVector.y = 0;
            _isGrounded = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if(other.transform.CompareTag("Ground"))
        {
            int collisionInstanceId = other.transform.GetInstanceID();

            if(_currentCollisionInstanceIds.Count > 0)
            {
                if(_currentCollisionInstanceIds.Contains(collisionInstanceId))
                {
                    _currentCollisionInstanceIds.Remove(collisionInstanceId);
                }
            }
        }

        if(_currentCollisionInstanceIds.Count <= 0)
        {
            _isGrounded = false;
        }
    }

    private void LookRotation()
    {
        _horizontalRot = Input.GetAxis("Mouse X") * _horizontalSensitivity * Time.deltaTime;
        _verticalRot -= Input.GetAxis("Mouse Y") * _verticalSensitivity * Time.deltaTime;

        transform.Rotate(0, _horizontalRot, 0);

        _verticalRot = Mathf.Clamp(_verticalRot, _minVerticalRot, _maxVerticalRot);

        _cameraRotation.x = _verticalRot;
        _cameraRotation.y += _horizontalRot;

        _playerCamera.transform.eulerAngles = _cameraRotation;
    }

    private bool GroundDetection()
    {
        for(int i = 0; i < _rayOrigins.Length; i++)
        {
            _rayOrigins[i] = transform.position;
            _rayOrigins[i].y += _rayOffsetY;
        }

        _rayOrigins[1].x += _rayOffsetCenter;
        _rayOrigins[2].x -= _rayOffsetCenter;
        _rayOrigins[3].z += _rayOffsetCenter;
        _rayOrigins[4].z -= _rayOffsetCenter;

        for(int i = 0; i < _rays.Length; i++)
        {
            _rays[i].origin = _rayOrigins[i];
            _rays[i].direction = Vector3.down;

            if(Physics.Raycast(_rays[i], out _raycastHits[i]))
            {
                if(_raycastHits[i].transform.CompareTag("Ground") && _raycastHits[i].distance < _rayDistance)
                {
                    if(i == 0) _groundNormal = _raycastHits[i].normal;
                    // Debug.Log("Distance: " + ((_raycastHits[i].collider.transform.position.y - _rayOrigins[i].y) * -1).ToString());
                    return true;
                }
            }
        }

        // Debug.Log("Ground Detection: false | Time: " + Time.time);
        _groundNormal.y = 1;
        return false;
    }

    private void DebugRaycasts()
    {
        Vector3[] tempRayOrigins = new Vector3[5];

        for(int i = 0; i < tempRayOrigins.Length; i++)
        {
            tempRayOrigins[i] = transform.position;
            tempRayOrigins[i].y += _rayOffsetY;
        }

        tempRayOrigins[1].x += _rayOffsetCenter;
        tempRayOrigins[2].x -= _rayOffsetCenter;
        tempRayOrigins[3].z += _rayOffsetCenter;
        tempRayOrigins[4].z -= _rayOffsetCenter;

        for(int i = 0; i < _rayOrigins.Length; i++)
        {
            Debug.DrawRay(tempRayOrigins[i], Vector2.down * _rayDistance, Color.magenta);
        }
    }

    public void SetPlayerColliderTrigger(bool value) => _playerCollider.isTrigger = value;
    public void SetCanMove(bool value)
    {
        _canMove = value;
        if(_canMove == false)
        {
            _movementVector = Vector3.zero;
            _rb.velocity = Vector3.zero;
        }
    }
    public void SetPlayerPosition(Vector3 pos) => this.transform.position = pos;
    public void SetPlayerRotation(Quaternion rot) => this.transform.rotation = rot;

    public void ResetCameraRotation() => Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
}