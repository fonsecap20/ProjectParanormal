using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    // Static instance to allow global access
    public static DialogueManager Instance { get; private set; }

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

    [Header("References")]
    [SerializeField] private GameObject _dialogueCanvas;
    [SerializeField] private GameObject _dialogueContainer;
    [SerializeField] private GameObject _dialogueBoxPrefab;

    [Header("Variables")]
    [SerializeField] private int _maxDialogueBoxCount = 4;

    private Queue<GameObject> _activeDialogueBoxes = new Queue<GameObject>();
    private IsDialogue _currentDialogueBoxController;
    private Conversation _currentConversation;
    private int _currentConversationIndex = 0;

    // Subscriptions
    Subscription<StartConversationEvent> _startConversationEvent;
    Subscription<ShowNextDialogueEvent> _showNextDialogueEvent;

    private void Start()
    {
        ClearDialogueContainer();
        _startConversationEvent = EventBus.Subscribe<StartConversationEvent>(StartConversation);
        _showNextDialogueEvent = EventBus.Subscribe<ShowNextDialogueEvent>(ShowNextDialogue);
    }

    private void StartConversation(StartConversationEvent e)
    {
        ControllerManager.Instance.SwitchActiveController(ControllerType.DialogueController);

        _currentConversation = e.conversation;
        _currentConversationIndex = 0;

        ClearDialogueContainer();
        ToggleDialogueCanvas(true);
        ShowNextDialogue(new ShowNextDialogueEvent());
    }

    private void ToggleDialogueCanvas(bool on)
    {
        if (on)
        {
            _dialogueCanvas.SetActive(true);
        }
        else
        {
             _dialogueCanvas.SetActive(false);
        }
    }

    private void ShowNextDialogue(ShowNextDialogueEvent e)
    {
        if (_currentConversation == null)
        {
            return;
        }

        if (_currentDialogueBoxController != null &&
            !_currentDialogueBoxController.IsDone())
        {
            _currentDialogueBoxController.ForceFinishDialogue();

            return;
        }

        if (_currentConversationIndex >= _currentConversation._dialogue.Count)
        {
            _currentConversation = null;
            ToggleDialogueCanvas(false);
            EventBus.Publish<EndConversationEvent>(new EndConversationEvent());

            return;
        }

        SpawnAndFillDialogue(_currentConversation._dialogue[_currentConversationIndex]);
        _currentConversationIndex++;
    }

    private void SpawnAndFillDialogue(Dialogue _newDialogue)
    {
        GameObject newDialogueBox = Instantiate(_dialogueBoxPrefab, _dialogueContainer.transform);
        IsDialogue newDialogueBoxController = newDialogueBox.GetComponent<IsDialogue>();

        newDialogueBoxController._dialogueSO = _newDialogue;
        newDialogueBoxController.FillInDialogueObject();

        _activeDialogueBoxes.Enqueue(newDialogueBox);
        _currentDialogueBoxController = newDialogueBoxController;

        // Check if any chat boxes are off-screen and remove them.
        if (_activeDialogueBoxes.Count > _maxDialogueBoxCount)
        {
            GameObject oldDialogueBox = _activeDialogueBoxes.Dequeue();
            Destroy(oldDialogueBox);
        }
    }

    private void ClearDialogueContainer()
    {
        for (int i = 0; i < _dialogueContainer.transform.childCount; i++) 
        {
            Destroy(_dialogueContainer.transform.GetChild(i).gameObject);
        }

        _activeDialogueBoxes.Clear();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<StartConversationEvent>(_startConversationEvent);
    }
}
