using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image _image;
    public ItemData itemData {get; private set;} = null;

    public void AssignItemToSlot(ItemData item)
    {
        _image.sprite = item.itemImage;
        _image.gameObject.SetActive(true);
        itemData = item;

        if(itemData != null)
            GameManager.Instance.playerInventoryScr.ToggleItem(true, itemData.itemId);
    }

    public void RemoveItemFromSlot()
    {
        _image.gameObject.SetActive(false);
        if(itemData != null) GameManager.Instance.playerInventoryScr.ToggleItem(false, itemData.itemId);
        itemData = null;
    }

    public void Selection(bool value)
    {
        if(value)
        {
            if(itemData != null)
                GameManager.Instance.playerInventoryScr.ToggleItem(true, itemData.itemId);

            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            if(itemData != null)
                GameManager.Instance.playerInventoryScr.ToggleItem(false, itemData.itemId);

            transform.localScale = Vector3.one;
        }
    }

    public bool IsEmpty() => itemData == null;
}
