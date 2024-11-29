using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Controller : MonoBehaviour
{
    public ControllerType _controllerType;
    public bool canToggleInventory = true;

    public virtual void Move(InputValue inputValue)
    {
        Debug.Log($"Receiving movement input from {_controllerType.HumanName()}");
    }

    public virtual void ToggleInventory()
    {
        Debug.Log($"Toggling inventory from {_controllerType.HumanName()}");
    }

    public virtual void Submit()
    {
        Debug.Log($"Receiving submission request from {_controllerType.HumanName()}");
    }
}
