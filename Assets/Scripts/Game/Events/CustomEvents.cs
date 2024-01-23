using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Custom events made to be used via the EventBus are stored here. */

// Sent when the 'Q' key is pressed in the PlayerController.
public class ToggleInventoryEvent
{
    public ToggleInventoryEvent() { }
}

// Sent when the user interacts with an object and finds an item
// or when they interact with an NPC and they are given an item.
public class AddItemEvent
{
    public Item item;

    public AddItemEvent(Item _item) 
    {
        item = _item;
    }
}

// Sent when the user interacts with an object and begins a battle.
public class StartBattleEvent
{
    public Enemy enemy;

    public StartBattleEvent(Enemy _enemy)
    {
        enemy = _enemy;
    }
}

// Sent when the status of battle has changed.
public class BattleStateChange
{
    public BattleState battleState;
    public BattleStateChange(BattleState _battleState)
    {
        battleState = _battleState;
    }
}