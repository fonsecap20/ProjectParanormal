using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : Controller
{
    public override void Move(InputValue inputValue)
    {
        base.Move(inputValue);

        InventoryManager.Instance.MoveSelection(inputValue);
    }

    public override void ToggleInventory()
    {
        base.ToggleInventory();

        if (!canToggleInventory) { return; }

        InventoryManager.Instance.ToggleInventory("Let me take a look...");
        ControllerManager.Instance.SwitchActiveController(ControllerType.OverworldController);
    }

    public override void Submit()
    {
        base.Submit();

        InventoryManager.Instance.SubmitSelectedItem();
    }
}
