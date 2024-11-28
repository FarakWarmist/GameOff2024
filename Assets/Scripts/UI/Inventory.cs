using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>
{
    [SerializeField] private ItemSlot[] _itemSlots = new ItemSlot[5];
    [SerializeField] private ItemData _specialKeyItemData;
    private ItemData[] _itemData = new ItemData[5];
    public int _selectedSlot {get; private set;} = 0;

    void Start()
    {
        _itemSlots[_selectedSlot].Selection(true);
    }

    public void UpdateInventorySlot(int index, ItemData itemData = null)
    {
        if(itemData == null)
            _itemSlots[index].RemoveItemFromSlot();
        else
            _itemSlots[index].AssignItemToSlot(itemData);
    }

    public int GetSelectedItemId()
    {
        int res = -1;

        if(_itemSlots[_selectedSlot].IsEmpty() == false)
        {
            res = _itemSlots[_selectedSlot].itemData.itemId;
        }

        return res;
    }

    public void DropItem(Vector3 position)
    {
        if(_itemSlots[_selectedSlot].IsEmpty() == false)
        {
            GameManager.Instance.playerInventoryScr.ToggleItem(false, _itemSlots[_selectedSlot].itemData.itemId);

            Instantiate(_itemSlots[_selectedSlot].itemData.itemPrefab, position, Quaternion.identity);
            UpdateInventorySlot(_selectedSlot, null);
        }
    }

    public void ChangeSlot(bool up)
    {
        _itemSlots[_selectedSlot].Selection(false);
        int prevSelectedSlot = _selectedSlot;

        if(up)
        {
            _selectedSlot++;
            if(_selectedSlot >= _itemSlots.Length)
                _selectedSlot = 0;
        }
        else
        {
            _selectedSlot--;
            if(_selectedSlot < 0)
                _selectedSlot = _itemSlots.Length - 1;
        }

        _itemSlots[_selectedSlot].Selection(true);

        if(_itemSlots[_selectedSlot].itemData != null || _itemSlots[prevSelectedSlot].itemData != null)
        {
            AudioManager.Instance.PlaySFX("HandleItem");
        }
    }

    public void Collect(ItemData itemData, GameObject go)
    {
        if(_itemSlots[_selectedSlot].IsEmpty())
        {
            if(itemData.itemId == 6 || itemData.itemId == 7 || itemData.itemId == 8)
            {
                List<int> keyFragments = new List<int>();
                for(int e = 0; e < _itemSlots.Length; e++)
                {
                    if(_itemSlots[e].IsEmpty() == false)
                        if(_itemSlots[e].itemData.itemId == 6 || _itemSlots[e].itemData.itemId == 7 || _itemSlots[e].itemData.itemId == 8)
                            keyFragments.Add(e);
                }

                if(keyFragments.Count >= 2)
                {
                    _itemSlots[_selectedSlot].AssignItemToSlot(_specialKeyItemData);
                    for(int o = 0; o < keyFragments.Count; o++)
                        _itemSlots[keyFragments[o]].RemoveItemFromSlot();
                }
                else
                    _itemSlots[_selectedSlot].AssignItemToSlot(itemData);

            }
            else
                _itemSlots[_selectedSlot].AssignItemToSlot(itemData);

            Destroy(go);
            AudioManager.Instance.PlaySFX("HandleItem");
        }
        else
        {
            for(int i = 0; i < _itemSlots.Length; i++)
            {
                if(_itemSlots[i].IsEmpty())
                {
                    if(itemData.itemId == 6 || itemData.itemId == 7 || itemData.itemId == 8)
                    {
                        List<int> keyFragments = new List<int>();
                        for(int e = 0; e < _itemSlots.Length; e++)
                        {
                            if(_itemSlots[e].IsEmpty() == false)
                                if(_itemSlots[e].itemData.itemId == 6 || _itemSlots[e].itemData.itemId == 7 || _itemSlots[e].itemData.itemId == 8) 
                                    keyFragments.Add(e);
                        }

                        if(keyFragments.Count >= 2)
                        {
                            _itemSlots[i].AssignItemToSlot(_specialKeyItemData);
                            foreach(ItemSlot iS in _itemSlots)
                            {
                                if(iS.IsEmpty() == false)
                                {
                                    if(iS.itemData.itemId == 6 || iS.itemData.itemId == 7 || iS.itemData.itemId == 8)
                                        iS.RemoveItemFromSlot();
                                }
                            }
                        }
                        else
                            _itemSlots[i].AssignItemToSlot(itemData);
                    }
                    else
                        _itemSlots[i].AssignItemToSlot(itemData);

                    if(_itemSlots[_selectedSlot].itemData != null)
                        GameManager.Instance.playerInventoryScr.ToggleItem(false, _itemSlots[_selectedSlot].itemData.itemId);
                        
                    _itemSlots[_selectedSlot].Selection(false);
                    _itemSlots[i].Selection(true);
                    _selectedSlot = i;
                    Destroy(go);
                    AudioManager.Instance.PlaySFX("HandleItem");
                    return;
                }
            }

            Debug.Log("No space in the inventory");
        }
    }
}
