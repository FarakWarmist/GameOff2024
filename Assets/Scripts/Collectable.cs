using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    public void Collect()
    {

    }

    public ItemData GetItemData() => _itemData;
}
