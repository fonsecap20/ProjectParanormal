using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if ( _collidedObjects.Count == 0 ) { EventBus.Publish<PlayerInteractStatusUpdate>(new PlayerInteractStatusUpdate(false)); }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        //Debug.Log("Colliding with " + collidedObject.name);
    }
}
