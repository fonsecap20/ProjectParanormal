using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Custom events made to be used via the EventBus are stored here. */

// Sent when the 'Q' key is pressed in the PlayerController.
public class ToggleInventoryEvent
{
    public ToggleInventoryEvent() { }
}

// Sent when the user interacts with an object and finds an item
// or when they interact with an NPC and they are given an item.
public class AddItemEvent
{
    public Item item;

    public AddItemEvent(Item _item) 
    {
        item = _item;
    }
}