using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fetch : Minigame
{
    [SerializeField] private GameObject _ball;

    public override void StartMinigame()
    {
        base.StartMinigame();

        _spawnedObjects.Add(Instantiate(_ball, transform));
        StartCoroutine(PlayMinigame());
    }

    public override IEnumerator PlayMinigame()
    {
        return base.PlayMinigame();
    }
    public override void EndMinigame()
    {
        base.EndMinigame();
    }
}
