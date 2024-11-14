using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

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
    [SerializeField] float _enemyTransitionDuration;

    private BattleState _battleState = BattleState.None;
    private Enemy currentEnemy;
    private int currentSolutionIndex = 0;

    // Subscriptions
    Subscription<StartBattleEvent> _battleStartEvent;
    Subscription<SubmitItemEvent> _submitItemEvent;

    void Start()
    {
        _battleStartEvent = EventBus.Subscribe<StartBattleEvent>(StartBattle);
        _submitItemEvent = EventBus.Subscribe<SubmitItemEvent>(CheckItem);
    }

    private void StartBattle(StartBattleEvent e)
    {
        Debug.Log("Starting fight with " + e.enemy.name);   
        _battleState = BattleState.Start;

        _player.GetComponent<PlayerController>().enabled = false;
        currentEnemy = e.enemy;

        StartCoroutine(BattleTransition());
    }

    private IEnumerator BattleTransition()
    {
        _battleCanvas.SetActive(true);

        // Fade to black.
        yield return FadeImage(_blackScreen, 0f, 1f, _transitionDuration / 2);

        // Move player to the middle of the player circle.
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

    private void CheckItem(SubmitItemEvent e)
    {
        bool correct = currentEnemy.solution[currentSolutionIndex] == e.item; 

        if (correct)
        {
            EnemyTransition();

            Debug.Log("Spawn Enemy");

        }
        else
        {
            _player.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

    private void EnemyTransition()
    {
        // Close the suitcase.
        EventBus.Publish<ToggleInventoryEvent>(new ToggleInventoryEvent(""));

        // Instantiate the enemy.
        GameObject spawnedEnemy = Instantiate(currentEnemy.enemyPrefab);

        // Move enemy to the middle of the enemy circle.
        spawnedEnemy.transform.position = _enemySide.transform.position;

        // Turn on enemy circle.
        _enemySide.SetActive(true);

        // Make spawned enemy and enemy circle transparent.
        SpriteRenderer enemySR = spawnedEnemy.GetComponent<SpriteRenderer>();
        SpriteRenderer enemySideSR = _enemySide.GetComponent<SpriteRenderer>();
        enemySR.color = new Color(enemySR.color.r, enemySR.color.g, enemySR.color.b, 0);
        enemySideSR.color = new Color(enemySideSR.color.r, enemySideSR.color.g, enemySideSR.color.b, 0);

        // Fade in enemy and enemy circle.
        StartCoroutine(FadeSprite(enemySR, 0f, 1f, _enemyTransitionDuration));
        StartCoroutine(FadeSprite(enemySideSR, 0f, 1f, _enemyTransitionDuration));
    }

    IEnumerator FadeSprite(SpriteRenderer sr, float startAlpha, float endAlpha, float duration)
    {
        Color color = sr.color;
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            sr.color = color;
            yield return null;
        }

        // Ensure the final alpha value is set
        color.a = endAlpha;
        sr.color = color;
    }
}
