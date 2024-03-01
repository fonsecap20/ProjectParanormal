using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    None, // Outside of battle.
    Start, // Start of the battle.
    PlayerTurn, // Player is selecting an item.
    EnemyTurn, // Enemy is attacking.
    FinishedRound, // Enemy has stopped attacking. 
    Won, // Enemy is defeated.
    Lost // Player is defeated.
}

public class BattleManager : MonoBehaviour
{
    [Header("Universal Variables")]
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _playerBattleIcon;
    [SerializeField] GameObject _battleBox;
    [SerializeField] GameObject _battleCanvas;

    [Header("Transition Variables")]
    [SerializeField] GameObject _playerSide;
    [SerializeField] GameObject _enemySide;
    [SerializeField] Image _blackScreen;
    [SerializeField] float _transitionDuration;
    [SerializeField] float _timeFadedOut;

    private BattleState _battleState = BattleState.None;
    private GameObject currentEnemy;

    // Subscriptions
    Subscription<StartBattleEvent> _battleStartEvent;

    void Start()
    {
        _battleStartEvent = EventBus.Subscribe<StartBattleEvent>(StartBattle);
    }

    private void StartBattle(StartBattleEvent s)
    {
        Debug.Log("Starting fight with " + s.enemy.name);   
        _battleState = BattleState.Start;

        _player.GetComponent<PlayerController>().enabled = false;
        currentEnemy = s.enemy;

        StartCoroutine(BattleTransition());
    }

    private IEnumerator BattleTransition()
    {
        _battleCanvas.SetActive(true);

        // Fade to black.
        yield return FadeImage(_blackScreen, 0f, 1f, _transitionDuration / 2);

        // Move non-battle player to the middle of the player circle.
        _player.transform.position = _playerSide.transform.position;

        // Turn on player circle.
        _playerSide.SetActive(true);

        // Wait for a short duration at full opacity
        yield return new WaitForSeconds(_timeFadedOut);

        // Fade in.
        yield return FadeImage(_blackScreen, 1f, 0f, _transitionDuration / 2);

        // Open inventory with first prompt.
        EventBus.Publish<ToggleInventoryEvent>(new ToggleInventoryEvent("Come forth! I know what holds you to our world!"));

        PlayerTurn();
    }

    IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        Color color = image.color;
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;
            yield return null;
        }

        // Ensure the final alpha value is set
        color.a = endAlpha;
        image.color = color;
    }

    private void PlayerTurn()
    {
        _battleState = BattleState.PlayerTurn;

        Debug.Log("Reached state of " + _battleState.HumanName());
    }
}
