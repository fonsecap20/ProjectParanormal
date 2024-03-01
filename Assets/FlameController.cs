using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlameController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed;
    [SerializeField] private float _sensitivity;

    private Vector2 _movementInput;
    private Vector2 _movePos;

    [Header("Bounds")]
    [SerializeField] private Vector2 _startingPos = Vector2.zero;
    [SerializeField] private GameObject _battleBox;
    [SerializeField] private float _boundOffset = 0.1f;

    private float _maxX = 3;
    private float _maxY = 3;
    private float _minX = -3;
    private float _minY = -3;

    private void Start()
    {
        SetFlame();
        SetBounds();
    }

    private void FixedUpdate()
    {
        _movePos.x += _movementInput.x * _sensitivity;
        _movePos.y += _movementInput.y * _sensitivity;

        _movePos.x = Mathf.Clamp(_movePos.x, _minX, _maxX);
        _movePos.y = Mathf.Clamp(_movePos.y, _minY, _maxY);

        transform.position = Vector2.Lerp(transform.position, _movePos, _speed * Time.deltaTime);
    }

    public void SetFlame()
    {
        transform.position = _startingPos;
        _movePos = _startingPos;
    }

    private void SetBounds()
    {
        // Ensure imageRectTransform is assigned
        if (_battleBox == null)
        {
            Debug.LogError("Battle box not assigned!");
            return;
        }

        // Get the Renderer component attached to the battle box
        Renderer renderer = _battleBox.GetComponent<Renderer>();

        // Check if renderer is null
        if (renderer != null)
        {
            // Get the bounds of the battlebox
            Bounds bounds = renderer.bounds;

            // Assign bounds
            _minX = bounds.min.x + _boundOffset;
            _maxX = bounds.max.x - _boundOffset;
            _minY = bounds.min.y + _boundOffset;
            _maxY = bounds.max.y - _boundOffset;
            
        }
        else
        {
            Debug.LogError("Renderer component not found!");
        }

        // Output bounds
        Debug.Log("Min X: " + _minX);
        Debug.Log("Max X: " + _maxX);
        Debug.Log("Min Y: " + _minY);
        Debug.Log("Max Y: " + _maxY);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }
}
