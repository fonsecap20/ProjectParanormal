using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Lightweight alternative to Unity animator system. Can animate UI images or spritesRenders.
/// Drag and drop a selection into the inspector 'spriteList' field.
/// </summary>
public class SpriteAnimator : MonoBehaviour
{
    public float animTime
    {
        get{ return (float)spriteList.Count/(float)framesPerSecond; }
    }
    public int framesPerSecond = 24;
    public bool loop = false;
    public bool destroyAfterAnimation = false;
    public bool playOnStart = false;
    public bool randomStartFrame = false;
    [Tooltip("Optional time between loops")] public float loopDownTime = 0f;
    [SerializeField] public List<Sprite> spriteList;
    private Coroutine currRoutine = null;
    public bool isPlaying => currRoutine != null;
    Image _img;
    SpriteRenderer _sr;
    private Color startColor;

    private void Start() {
        _img = GetComponent<Image>();
        _sr = GetComponent<SpriteRenderer>();

        if(playOnStart)
            Play();

        if (destroyAfterAnimation)
                Destroy(gameObject, animTime);

        startColor = _img ? _img.color : _sr.color;
    }

    private void OnEnable() {
        if(playOnStart)
            Play();
        
        if(_img) 
            _img.color = startColor;
        else if(_sr)  
            _sr.color = startColor;
    }

    private void OnDisable() {
        if (currRoutine != null)
            StopCoroutine(currRoutine);
        currRoutine = null;
    }

    public void Play() {
        if (currRoutine != null)
        {
            Stop();
        }
        currRoutine = StartCoroutine(PlayCR());
    }

    public void Stop() {
        if (currRoutine != null)
        {
            StopCoroutine(currRoutine);
            currRoutine = null;
        }
    }

    public void SetNewAnimation(List<Sprite> _spriteList)
    {
        Stop();
        spriteList = _spriteList;
        if(playOnStart)
            Play();
    }

    private IEnumerator PlayCR() {
        if(spriteList.Count == 0) yield break;
        float frameLength = 1/(float)framesPerSecond;
        
        int startFrame = 0;
        if(randomStartFrame)
            startFrame = Random.Range(0, spriteList.Count); 


        // Loop animation
        do
        {
            // Cycle through frames
            for(int i = startFrame; i < spriteList.Count; i++)
            {
                if(_img) { _img.sprite = spriteList[i]; }
                if(_sr)  { _sr.sprite = spriteList[i]; }
                yield return new WaitForSecondsRealtime(frameLength);
            }
            startFrame = 0;

            // Set sprite to transparent and wait for loopDowntime 
            if(loopDownTime > 0)
            {
                //var transparent = startColor;
                //transparent.a = 0;;

                //if(_img) 
                //    _img.color = transparent;
                //else if(_sr)  
                //    _sr.color = transparent;

                yield return new WaitForSecondsRealtime(loopDownTime);
                
                //if(_img) 
                //    _img.color = startColor;
                //else if(_sr)  
                //    _sr.color = startColor;
            }

        } while (loop);
        currRoutine = null; 
    }

}
