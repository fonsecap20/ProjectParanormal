using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public float Duration = 5.0f;
    public float SpeedMultiplier = 1.0f;

    [HideInInspector] public List<GameObject> _spawnedObjects = new List<GameObject>();

    public virtual void StartMinigame()
    {
        Debug.Log($"Start {gameObject.name} Minigame");

        _spawnedObjects.Clear();
    }

    public virtual IEnumerator PlayMinigame()
    {
        Debug.Log($"Play {gameObject.name} Minigame");

        yield return new WaitForSeconds(Duration);

        EndMinigame();
        yield return null;
    }

    public virtual void EndMinigame()
    {
        Debug.Log($"End {gameObject.name} Minigame");

        foreach (GameObject _go in _spawnedObjects)
        {
            Destroy(_go);
        }

        _spawnedObjects.Clear();
    }
}
