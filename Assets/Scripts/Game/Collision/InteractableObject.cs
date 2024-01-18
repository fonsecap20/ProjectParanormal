using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class consists of collision logic and adds the interact function. 
   While the player is colliding with this gameobject, they can invoke its
   Interact() function. */
public class InteractableObject : CollidableObject
{
    protected override void OnCollided(GameObject collidedObject)
    {
        //Debug.Log("Can interact with " +  gameObject.name);
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
