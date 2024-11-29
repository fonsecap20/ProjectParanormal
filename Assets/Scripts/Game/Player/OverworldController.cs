using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldController : Controller
{
    // Movement variables.
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Vector2 _movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            _movementInput,
            ref _movementInputSmoothVelocity,
            0.1f);

        _rigidbody.velocity = _smoothedMovementInput * _speed;
    }

    public override void Move(InputValue inputValue)
    {
        base.Move(inputValue);

        _movementInput = inputValue.Get<Vector2>();
    }

    public override void ToggleInventory()
    {
        base.ToggleInventory();

        InventoryManager.Instance.ToggleInventory("Let me take a look...");
        ControllerManager.Instance.SwitchActiveController(ControllerType.InventoryController);
    }

    private void OnDisable()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
