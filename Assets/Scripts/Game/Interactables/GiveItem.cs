using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This child of the InteractableObject class will override the Interact function to: 
   Add the given item to the player's inventory. */
public class GiveItem : InteractableObject
{
    [SerializeField]
    private Item _item;
    protected override void Interact()
    {
        if (_item == null) { return; }

        EventBus.Publish<AddItemEvent>(new AddItemEvent(_item));
        this.enabled = false;
    }
}
