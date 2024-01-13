using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Custom events made to be used via the EventBus are stored here.
public class PlayerInteractStatusUpdate
{
    public bool _canInteract = false;

    public PlayerInteractStatusUpdate(bool canInteract) {  _canInteract = canInteract; }
}
