using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : Controller
{
    [Header("Movement")]
    [SerializeField] private Vector2 _startingPos = Vector2.zero;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    private Rigidbody2D _rb;
    private Vector2 _movementInput;

    private void Start()
    {
        SetFlame();
    }

    private void FixedUpdate()
    {
        if (_movementInput.magnitude > 0)
        {
            // Apply force in movement direction with acceleration
            _rb.AddForce(_movementInput * _acceleration, ForceMode2D.Force);
        }
        else
        {
            // Apply deceleration to slow down naturally
            _rb.velocity = Vector2.Lerp(_rb.velocity, Vector2.zero, _deceleration * Time.fixedDeltaTime);
        }

        // Limit max speed to prevent infinite acceleration
        if (_rb.velocity.magnitude > _maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
        }
    }

    public void SetFlame()
    {
        transform.position = _startingPos;
        _rb = GetComponent<Rigidbody2D>();
    }

    public override void Move(InputValue inputValue)
    {
        base.Move(inputValue);

        _movementInput = inputValue.Get<Vector2>();
    }

    public override void ToggleInventory()
    {
        base.ToggleInventory();
    }


}
