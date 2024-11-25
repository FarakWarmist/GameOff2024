using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafeBox : MonoBehaviour
{
    [SerializeField] private int[] _code = new int[3];
    [SerializeField] private Transform _keySpawnPosition;
    [SerializeField] private GameObject _key3Prefab;
    private int[] _numbers = new int[3];
    public bool _isOpened {get; private set;}
    private bool _isOpening;
    [SerializeField] private float _rotationSpeed = 2;
    private float _rotationAmount;
    private float _rotationMax = 90f;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _door;

    void Start()
    {
        for(int i = 0; i < _code.Length; i++)
        {
            _code[i] = Random.Range(0, 10);
        }
        GameManager.Instance.SetSafeBoxCode(_code[0], _code[1], _code[2]);
    }

    void Update()
    {
        if(_isOpening)
        {
            _rotationAmount += _rotationSpeed * Time.deltaTime;
            _door.transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);

            if(_rotationAmount >= _rotationMax)
                _isOpening = false;
        }
    }

    public void UpdateNumber(int numberIndex, int newValue)
    {
        _numbers[numberIndex] = newValue;
        if(IsCorrectCode())
        {
            //Open the safe box
            _isOpening = true;
            Instantiate(_key3Prefab, _keySpawnPosition.position, Quaternion.identity);
            _isOpened = true;
        }
    }

    public bool IsCorrectCode()
    {
        bool res = true;

        for(int i = 0; i < _numbers.Length; i++)
        {
            if(_numbers[i] != _code[i])
            {
                res = false;
                break;
            }
        }

        return res;
    }
}
