using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private bool fadeInOnStart = true, fadeOutOnStart = false, startHidden = true, useWaitAtFadeOut = false, alsoScale = false;
    [SerializeField] private UnityEvent fadedIn, fadedOut;
    [SerializeField] private List<SpriteRenderer> includeSprites;
    [SerializeField] private List<Image> includeImages;
    [SerializeField] private List<TMP_Text> includeText;
    

    // Private variables
    private SpriteRenderer _sr;
    private Image _img;
    private Color _startColor = Color.white;
    private Coroutine _fadeRoutine;
    private Vector2 _startScale;
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        bool useImg = !_sr;
        _img = GetComponent<Image>();
        if (useImg ? _img : _sr) _startColor = (useImg ? _img.color : _sr.color);
        if (startHidden) SetColor(new Color(_startColor.r, _startColor.g, _startColor.b, 0f));
        _startScale = transform.localScale;
    }
    
    public void FadeIn()
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(true));
    }
    
    public void FadeOut()
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(FadeRoutine(false));
    }

    private void Start()
    {
        if (fadeInOnStart) FadeIn();
        else if (fadeOutOnStart) FadeOut();
    }

    private IEnumerator FadeRoutine(bool fadeIn = true)
    {
        // Setup
        float timer = 0f;
        float wait = fadeIn || useWaitAtFadeOut ? waitTime : 0f;
        
        // Set to start colour
        SetColor(fadeIn 
            ? new Color(_startColor.r, _startColor.g, _startColor.b, 0f)
            : _startColor);
        SetScale(fadeIn ? Vector3.zero : _startScale);
        
        // Animation loop
        while (timer < fadeTime + wait)
        {
            timer += Time.deltaTime;
            if (timer > wait)
            {
                float t = Mathf.Clamp01((timer - wait) / fadeTime);
                float a = fadeIn
                    ? Mathf.Lerp(0f, _startColor.a, t)
                    : Mathf.Lerp(_startColor.a, 0f, t);
                SetColor(new Color(_startColor.r, _startColor.g, _startColor.b, a));

                SetScale(Vector2.Lerp(
                    fadeIn ? Vector2.zero : _startScale,
                    fadeIn ? _startScale : Vector2.zero,
                    t));
            }
            yield return null;
        }
        
        // Trigger event
        if (fadeIn) fadedIn.Invoke();
        else fadedOut.Invoke();

        // Set to end colour
        if (_sr) SetColor(fadeIn
            ? _startColor
            : new Color(_startColor.r, _startColor.g, _startColor.b, 0f));
        SetScale(fadeIn ? _startScale : Vector3.zero);
        
        // Exit cleanly
        _fadeRoutine = null;
    }

    private void SetColor(Color col)
    {
        if (_sr) _sr.color = col;
        if (_img) _img.color = col;
        foreach (SpriteRenderer sr in includeSprites)
            sr.color = col;
        foreach (Image img in includeImages)
            img.color = col;
        foreach (TMP_Text tmp in includeText)
            tmp.color = col;
    }

    private void SetScale(Vector2 scale)
    {
        if (alsoScale) transform.localScale = scale;
    }
}
