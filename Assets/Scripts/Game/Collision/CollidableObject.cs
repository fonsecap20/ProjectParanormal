using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Parent class for all collidable objects. They will check if they are colliding with an object
   within its filter every frame invoking the OnCollided function for each gameobject if so. */
public class CollidableObject : MonoBehaviour
{
    [SerializeField]
    private ContactFilter2D _filter;

    private Collider2D _collider;
    private List<Collider2D> _collidedObjects = new List<Collider2D>(1);

    protected virtual void Start()
    {
        _collider = GetComponent<Collider2D>();

        if ( _collider == null )
        {
            _collider = gameObject.AddComponent<Collider2D>();
        }
    }

    protected virtual void Update()
    {
        _collider.OverlapCollider(_filter, _collidedObjects);

        foreach(var o in _collidedObjects)
        {
            OnCollided(o.gameObject);
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        //Debug.Log("Colliding with " + collidedObject.name);
    }
}
