using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControllerType
{
    BattleController,
    DialogueController,
    InventoryController,
    OverworldController,
    None
}

public class ControllerManager : MonoBehaviour
{
    #region Singleton
    // Static instance to allow global access
    public static ControllerManager Instance { get; private set; }

    // Awake is called before Start
    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // Destroy duplicate instances
            Destroy(gameObject);
            return;
        }

        // Assign this instance to the static property
        Instance = this;

        //// Optionally, make this object persistent across scenes
        //DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private List<Controller> controllers = new List<Controller>();

    private Controller _activeController;

    private void Start()
    {
        SwitchActiveController(ControllerType.OverworldController);
    }

    private void OnMove(InputValue inputValue)
    {
        if (_activeController == null) { return; }
        _activeController.Move(inputValue);
    }

    private void OnToggleInventory()
    {
        if (_activeController == null) { return; }
        _activeController.ToggleInventory();
    }

    private void OnSubmit()
    {
        if (_activeController == null) { return; }
        _activeController.Submit();
    }

    public void SwitchActiveController(ControllerType _newControllerType, bool _canToggleInventory = true)
    {
        if (_newControllerType == ControllerType.None)
        {
            // No inputs should be responded to.
            _activeController = null;
            return;
        }

        Controller new_controller = controllers.FirstOrDefault(c => c._controllerType == _newControllerType);
        new_controller.canToggleInventory = _canToggleInventory;

        if (new_controller != null)
        {
            _activeController = new_controller;
            Debug.Log($"Found new controller of type {_newControllerType}");
        }
        else
        {
            Debug.LogError($"No controller of type {_newControllerType} could be found");
        }
    }
}
