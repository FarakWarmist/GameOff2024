using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeBox : MonoBehaviour
{
    [SerializeField] private int[] _code = new int[3];
    private int[] _numbers = new int[3];
    public bool _isOpened {get; private set;}
    [SerializeField] private MeshRenderer _meshRenderer;

    public void UpdateNumber(int numberIndex, int newValue)
    {
        _numbers[numberIndex] = newValue;
        if(IsCorrectCode())
        {
            //Open the safe box
            _meshRenderer.material.color = Color.green;
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
