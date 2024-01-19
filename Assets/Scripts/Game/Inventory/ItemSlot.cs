using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    //=======ITEM DATA======//
    [SerializeField]
    private Item _item;

    //====ITEM SLOT====//
    public bool _isFull;
    [SerializeField]
    private Image _itemImage;
    [SerializeField]
    private GameObject _selectedPanel;

    public void AddItem(Item item)
    {
        _item = item;
        _itemImage.sprite = item.icon;
        _isFull = true;
    }

    public Item GetItemInfo()
    {
        return _item;
    }

    public void SelectItem()
    {
        _selectedPanel.SetActive(true);
    }

    public void DeselectItem()
    {
        _selectedPanel.SetActive(false);
    }


}
