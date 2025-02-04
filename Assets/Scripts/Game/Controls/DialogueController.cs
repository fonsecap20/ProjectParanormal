using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : Controller
{
    public override void Move(InputValue inputValue)
    {
        base.Move(inputValue);
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

        EventBus.Publish<ShowNextDialogueEvent>(new ShowNextDialogueEvent());
    }
}
