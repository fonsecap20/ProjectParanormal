using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryCanvas;

    //====TOP INVENTORY INFO====//
    [Header("Top Inventory References")]
    [SerializeField]
    private Text _itemName;
    [SerializeField]
    private Text _itemDescription;
    [SerializeField]
    private Image _itemIcon;
    [SerializeField]
    private Text _prompt;

    //======ITEM SLOTS INFO======//
    [Header("Bottom Inventory References")]
    [SerializeField]
    private ItemSlot[] _itemSlots;
    private int _selectedItemSlotIndex;

    [Header("Text Settings")]
    [SerializeField]
    private float _promptLetterDelay;

    // Subscriptions
    Subscription<ToggleInventoryEvent> _toggleInventoryEvent;
    Subscription<AddItemEvent> _addItemEvent;

    void Start()
    {
        _toggleInventoryEvent = EventBus.Subscribe<ToggleInventoryEvent>(ToggleInventory);
        _addItemEvent = EventBus.Subscribe<AddItemEvent>(AddItem);
    }

    private void ToggleInventory(ToggleInventoryEvent t)
    {
        //Debug.Log("Toggling Inventory");
        if (_inventoryCanvas == null) { return; }

        _inventoryCanvas.SetActive(!_inventoryCanvas.activeSelf);

        if (_inventoryCanvas.activeSelf)
        {
            FindObjectOfType<PlayerController>().enabled = false;
            _itemSlots[0].SelectItem();
            _selectedItemSlotIndex = 0;
            ShowItem(_itemSlots[0].GetItemInfo());
            Prompt(t.prompt);
        }
        else
        {
            _itemSlots[_selectedItemSlotIndex].DeselectItem();
            FindObjectOfType<PlayerController>().enabled = true;
        }
    }

    private void AddItem(AddItemEvent a)
    {
        //Debug.Log("Adding item called '" + a.name + "' with description '" + a.description + "' that looks like '" + a.icon.name + ".'");
        for (int i = 0; i < _itemSlots.Length; i++) 
        {
            if (_itemSlots[i]._isFull == false)
            {
                _itemSlots[i].AddItem(a.item);
                Debug.Log($"Added {a.item.name} item.");
                return;
            }
        }
    }

    private void ShowItem(Item item)
    {
        _itemName.text = item.name;
        _itemDescription.text = item.description;
        _itemIcon.sprite = item.icon;
    }

    private void Prompt(string prompt)
    {
        StopAllCoroutines();

        _prompt.text = "";

        StartCoroutine(DelayedText(prompt));
    }

    private IEnumerator DelayedText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _prompt.text += text[i];
            yield return new WaitForSeconds(_promptLetterDelay);
        }
    }

    private void OnMove(InputValue inputValue)
    {
        if(!_inventoryCanvas.activeSelf)
        {
            return;
        }

        //Debug.Log("Changing selected item from: " + _selectedItemSlotIndex.ToString());

        Vector2 _selectionInput = inputValue.Get<Vector2>().normalized;
        int _selectedItemIndexChange = _selectedItemSlotIndex;

        if (_selectionInput.x > 0)
        {
            _selectedItemIndexChange = (_selectedItemSlotIndex + 1) % _itemSlots.Length;
        }
        else if (_selectionInput.x < 0)
        {
            _selectedItemIndexChange = (_selectedItemSlotIndex - 1 + 10) % _itemSlots.Length;
        }
        else if (_selectionInput.y > 0)
        {
            _selectedItemIndexChange = (_selectedItemSlotIndex + 5) % _itemSlots.Length;
        }
        else if (_selectionInput.y < 0)
        {
            _selectedItemIndexChange = (_selectedItemSlotIndex - 5 + 10) % _itemSlots.Length;
        }

        _itemSlots[_selectedItemSlotIndex].DeselectItem();
        _selectedItemSlotIndex = _selectedItemIndexChange;
        _itemSlots[_selectedItemSlotIndex].SelectItem();
        ShowItem(_itemSlots[_selectedItemSlotIndex].GetItemInfo());
    }

    private void OnSubmit()
    {
        if (!_inventoryCanvas.activeSelf)
        {
            return;
        }

        EventBus.Publish<SubmitItemEvent>(new SubmitItemEvent(_itemSlots[_selectedItemSlotIndex].GetItemInfo()));

        Debug.Log($"Submitted the {_itemSlots[_selectedItemSlotIndex].GetItemInfo().name} item.");
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ToggleInventoryEvent>(_toggleInventoryEvent);
        EventBus.Unsubscribe<AddItemEvent>(_addItemEvent);
    }
}
