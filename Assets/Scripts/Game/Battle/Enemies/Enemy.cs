using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    public GameObject enemyPrefab;
    public new string name;
    public int health;
    public Sprite icon;
}
