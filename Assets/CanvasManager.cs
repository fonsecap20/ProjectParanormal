using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _inventoryCanvas;

    // Subscriptions
    Subscription<ToggleInventoryEvent> _toggleInventoryEvent;

    void Start()
    {
        _toggleInventoryEvent = EventBus.Subscribe<ToggleInventoryEvent>(ToggleInventory);
    }

    private void ToggleInventory(ToggleInventoryEvent toggleInventoryEvent)
    {
        //Debug.Log("Toggling Inventory");
        if (_inventoryCanvas == null) { return; }

        _inventoryCanvas.SetActive(!_inventoryCanvas.activeSelf);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ToggleInventoryEvent>(_toggleInventoryEvent);
    }
}
