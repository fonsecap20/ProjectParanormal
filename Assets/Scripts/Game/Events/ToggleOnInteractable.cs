using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnInteractable : MonoBehaviour
{
    private Renderer _renderer;

    // Subscriptions
    Subscription<PlayerInteractStatusUpdate> _interactStatus;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _interactStatus = EventBus.Subscribe<PlayerInteractStatusUpdate>(ToggleUpdate);
    }

    private void ToggleUpdate(PlayerInteractStatusUpdate s)
    {
        _renderer.enabled = s._canInteract;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlayerInteractStatusUpdate>(_interactStatus);
    }
}
