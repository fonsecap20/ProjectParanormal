using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Enemy Info")]
    public string _name;
    public GameObject _enemyPrefab;
    public GameObject _defendMinigamePrefab;
    public GameObject _punishMinigamePrefab;
    public List<Item> _solution;

    [Header("Conversations")]
    public List<Conversation> _wrongAnswerDialogue = new List<Conversation>();
    public List<Conversation> _rightAnswerDialogue = new List<Conversation>();
}