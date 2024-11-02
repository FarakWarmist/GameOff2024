using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    private ItemData _itemData = null;

    public void AssignItemToSlot(ItemData item)
    {
        _image.sprite = item.itemImage;
        _image.gameObject.SetActive(true);
        _itemData = item;
    }

    public void RemoveItemFromSlot()
    {
        _image.gameObject.SetActive(false);
        _itemData = null;
    }

    public void Selection(bool value)
    {
        if(value)
        {
            transform.localScale = Vector3.one * 1.05f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    public bool IsEmpty() => _itemData == null;
}
