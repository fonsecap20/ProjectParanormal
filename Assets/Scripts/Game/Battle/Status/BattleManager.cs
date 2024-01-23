using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    Start, // Start of the battle.
    PlayerTurn, // Player is selecting an item.
    EnemyTurn, // Enemy is attacking.
    FinishedRound, // Enemy has stopped attacking. 
    Won, // Enemy is defeated.
    Lost // Player is defeated.
}

public class BattleManager : MonoBehaviour
{
    // Subscriptions
    Subscription<StartBattleEvent> _battleStartEvent;

    void Start()
    {
        _battleStartEvent = EventBus.Subscribe<StartBattleEvent>(StartBattle);
    }

    private void StartBattle(StartBattleEvent s)
    {
        Debug.Log("Starting fight with " + s.enemy.name);
    }
}
