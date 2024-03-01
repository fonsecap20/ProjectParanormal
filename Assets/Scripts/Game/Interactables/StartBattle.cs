using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This child of the InteractableObject class will override the Interact function to 
   Initiate battle with the given enemy.*/

public class StartBattle : InteractableObject
{
    [SerializeField]
    private GameObject _enemy;
    protected override void Interact()
    {
        if (_enemy == null) { return; }

        EventBus.Publish<StartBattleEvent>(new StartBattleEvent(_enemy));
        this.enabled = false;
    }
}
