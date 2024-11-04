using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SafeBoxNumber : Interactable
{
    [SerializeField] private int _safeBoxNumberIndex;
    [SerializeField] private SafeBox _safeBoxScr;
    [SerializeField] private TMP_Text _numberText;
    private int _currentNumber;

    void Start()
    {
        _currentNumber = Random.Range(0, 10);
        _numberText.text = _currentNumber.ToString();
    }

    public override void Interact(int itemIndex, int slotIndex)
    {
        if(_safeBoxScr._isOpened == false)
        {
            _currentNumber++;
            if(_currentNumber > 9) _currentNumber = 0;
            _numberText.text = _currentNumber.ToString();
            _safeBoxScr.UpdateNumber(_safeBoxNumberIndex, _currentNumber);
        }
    }
}
