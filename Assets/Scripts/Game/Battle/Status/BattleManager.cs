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
    Dialogue, // Player and Enemy are speaking.
    EnemyTurn, // Enemy is attacking.
    FinishedRound, // Enemy has stopped attacking. 
    Won, // Enemy is defeated.
    Lost // Player is defeated.
}

public class BattleManager : MonoBehaviour
{
    #region Singleton
    // Static instance to allow global access
    public static BattleManager Instance { get; private set; }

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


    // State Variables.
    private BattleState _battleState = BattleState.None;
    private Conversation _currentConversation;
    private int _currentSolutionIndex = 0;

    // Enemy Variables.
    private Enemy _currentEnemySO;
    private GameObject _enemy;
    private GameObject _currentMinigamePrefab;
    private Minigame _currentMinigame;

    // Subscriptions
    Subscription<StartBattleEvent> _battleStartEvent;
    Subscription<SubmitItemEvent> _submitItemEvent;
    Subscription<EndConversationEvent> _endConversationEvent;

    void Start()
    {
        _battleStartEvent = EventBus.Subscribe<StartBattleEvent>(StartBattle);
        _submitItemEvent = EventBus.Subscribe<SubmitItemEvent>(CheckItem);
        _endConversationEvent = EventBus.Subscribe<EndConversationEvent>(EnemyTurn); 
    }

    #region Battle State
    private void StartBattle(StartBattleEvent e)
    {
        Debug.Log("Starting fight with " + e.enemy.name);
        ControllerManager.Instance.SwitchActiveController(ControllerType.None);
        
        _battleState = BattleState.Start;
        _currentEnemySO = e.enemy;
        StartCoroutine(BattleTransition());
    }

    private IEnumerator BattleTransition()
    {
        _battleCanvas.SetActive(true);

        // Fade to black.
        yield return FadeImage(_blackScreen, 0f, 1f, _transitionDuration / 2);

        // Move player to the middle of the player circle.
        _player.transform.position = _playerSide.transform.position;

        // Turn on player and enemy circle.
        _playerSide.SetActive(true);
        _enemySide.SetActive(true);

        // Wait for a short duration at full opacity
        yield return new WaitForSeconds(_timeFadedOut);

        // Fade in.
        yield return FadeImage(_blackScreen, 1f, 0f, _transitionDuration / 2);

        // Open inventory with first prompt.
        InventoryManager.Instance.ToggleInventory("Come forth! I know what holds you to our world!");
        ControllerManager.Instance.SwitchActiveController(ControllerType.InventoryController, false);

        PlayerTurn();
    }

    private void PlayerTurn()
    {
        _battleState = BattleState.PlayerTurn;

        Debug.Log("Reached state of " + _battleState.HumanName());
    }

    private void CheckItem(SubmitItemEvent e)
    {
        bool correct = _currentEnemySO._solution[_currentSolutionIndex] == e.item; 

        if (correct)
        {
            if (_currentSolutionIndex == 0)
            {
                EnemyTransition();

                Debug.Log("Spawn Enemy");
            }
            else
            {
                //_enemy.GetComponent<EnemyHealth>().TakeDamage(1);
            }

            SelectMinigame(true);
            Dialogue(true);

        }
        else
        {
            _player.GetComponent<PlayerHealth>().TakeDamage(1);

            SelectMinigame(false);
            Dialogue(false);
        }
    }

    private void EnemyTransition()
    {
        // Close the suitcase.
        InventoryManager.Instance.ToggleInventory("");

        // Instantiate the enemy.
        GameObject spawnedEnemy = Instantiate(_currentEnemySO._enemyPrefab);
        _enemy = spawnedEnemy;

        // Move enemy to the middle of the enemy circle.
        spawnedEnemy.transform.position = _enemySide.transform.position;

        // Make spawned enemy transparent.
        SpriteRenderer enemySR = spawnedEnemy.GetComponent<SpriteRenderer>();
        enemySR.color = new Color(enemySR.color.r, enemySR.color.g, enemySR.color.b, 0);

        // Fade in enemy.
        StartCoroutine(FadeSprite(enemySR, 0f, 1f, _enemyTransitionDuration));
    }

    private void SelectMinigame(bool _correct)
    {
        if (_correct)
        {
            _currentMinigamePrefab = _currentEnemySO._defendMinigamePrefab;
        }
        else
        {
            _currentMinigamePrefab = _currentEnemySO._punishMinigamePrefab;
        }
    }

    private void Dialogue(bool _correct)
    {
        List<Conversation> correctConversations = _currentEnemySO._rightAnswerDialogue;
        List<Conversation> wrongConversations = _currentEnemySO._wrongAnswerDialogue;


        if (_correct)
        {
            _currentConversation = correctConversations[_currentSolutionIndex];
            _currentSolutionIndex++;
        }
        else
        {
            _currentConversation = wrongConversations[Random.Range(0, wrongConversations.Count)];
        }

        EventBus.Publish<StartConversationEvent>(new StartConversationEvent(_currentConversation));
    }

    private void EnemyTurn(EndConversationEvent e)
    {
        ControllerManager.Instance.SwitchActiveController(ControllerType.BattleController);
        _battleState = BattleState.EnemyTurn;
        _battleBox.SetActive(true);

        _currentMinigame = Instantiate(_currentMinigamePrefab, _battleBox.transform).GetComponent<Minigame>();
        _currentMinigame.StartMinigame();

        Debug.Log("Enemy Turn");
    }

    #endregion

    #region Helper Functions
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
    #endregion
}
