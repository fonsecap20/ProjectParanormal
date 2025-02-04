using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsDialogue : MonoBehaviour
{
    [Header("Scriptable Object Reference")]
    public Dialogue _dialogueSO;

    [Header("Dialogue Fields")]
    [SerializeField] Image _profile;
    [SerializeField] Text _name;
    [SerializeField] Text _dialogue;

    public void FillInDialogueObject()
    {
        _profile.sprite = _dialogueSO._profile;
        _name.text = _dialogueSO._name;

        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        string fullDialogue = _dialogueSO._dialogue;

        _dialogue.text = "";

        for (int i = 0; i < fullDialogue.Length; i++)
        {
            _dialogue.text += fullDialogue[i];
            yield return new WaitForSeconds(_dialogueSO._timeBetweenLetters);
        }

        yield return null;
    }

    public void ForceFinishDialogue()
    {
        StopAllCoroutines();

        _dialogue.text = _dialogueSO._dialogue;
    }

    public bool IsDone()
    {
        return _dialogue.text == _dialogueSO._dialogue;
    }
}
