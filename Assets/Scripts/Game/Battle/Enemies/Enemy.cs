using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public new string name;
    public GameObject enemyPrefab;
    public GameObject defendMinigamePrefab;
    public GameObject punishMinigamePrefab;
    public List<Item> solution;
}