using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [Header("Basic Elements")]
    public Sprite _profile;
    public string _name;
    [TextArea] public string _dialogue;

    [Header("Personality")]
    public float _timeBetweenLetters = 0.1f;
}
