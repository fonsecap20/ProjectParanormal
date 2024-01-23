using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth = 1;
    private int _currentHealth = 1;

    // Subscriptions
    Subscription<StartBattleEvent> _startBattleEvent;

    protected virtual void Start()
    {
        _startBattleEvent = EventBus.Subscribe<StartBattleEvent>(Initialize);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Heal(1);
        }
    }

    protected virtual void Initialize(StartBattleEvent s)
    {
        _currentHealth = _maxHealth;

        Debug.Log("Set " + gameObject.name + "'s health to " +  _currentHealth + " out of " + _maxHealth);
    }

    protected virtual void Heal(int healAmount)
    {
        _currentHealth += healAmount;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }

        Debug.Log(gameObject.name + " was healed by " + healAmount + " resulting in " + _currentHealth + " out of " + _maxHealth);
    }
    
    protected virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        Debug.Log(gameObject.name + "was damaged by " + damage + " resulting in " + _currentHealth + " out of " + _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has died.");
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<StartBattleEvent>(_startBattleEvent);
    }
}
