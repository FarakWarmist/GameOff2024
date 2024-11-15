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
            if(itemData.itemId == 12)
                GameManager.Instance.playerInventoryScr.ToggleLantern(true);
    }

    public void RemoveItemFromSlot()
    {
        _image.gameObject.SetActive(false);
        itemData = null;
    }

    public void Selection(bool value)
    {
        if(value)
        {
            if(itemData != null)
                if(itemData.itemId == 12)
                    GameManager.Instance.playerInventoryScr.ToggleLantern(true);

            transform.localScale = Vector3.one * 1.05f;
        }
        else
        {
            if(itemData != null)
                if(itemData.itemId == 12)
                    GameManager.Instance.playerInventoryScr.ToggleLantern(false);

            transform.localScale = Vector3.one;
        }
    }

    public bool IsEmpty() => itemData == null;
}
