using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : CollidableObject
{
    protected override void OnCollided(GameObject collidedObject)
    {
        EventBus.Publish<PlayerInteractStatusUpdate>(new PlayerInteractStatusUpdate(true));

        //Debug.Log("Can interact with " +  gameObject.name);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
