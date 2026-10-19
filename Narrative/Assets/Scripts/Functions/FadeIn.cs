using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Fade : MonoBehaviour
{
    [SerializeField] private float waitTime = 0f;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private bool fadeInOnStart = true, fadeOutOnStart = false, startHidden = true, useWaitAtFadeOut = false;
    [SerializeField] private UnityEvent fadedIn, fadedOut;

    // Private variables
    private SpriteRenderer _sr;
    private Color _startColor = Color.white;
    private Coroutine _fadeRoutine;
    
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
    
    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        if (_sr) _startColor = _sr.color;
        if (startHidden) _sr.color = new Color(_startColor.r, _startColor.g, _startColor.b, 0f);
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
        if (_sr) _sr.color = fadeIn 
                ? new Color(_startColor.r, _startColor.g, _startColor.b, 0f)
                : _startColor;
        
        // Animation loop
        while (timer < fadeTime + wait)
        {
            timer += Time.deltaTime;
            if (timer > wait)
            {
                float a = fadeIn
                    ? Mathf.Lerp(0f, _startColor.a, Mathf.Clamp01((timer - wait) / fadeTime))
                    : Mathf.Lerp(_startColor.a, 0f, Mathf.Clamp01((timer - wait) / fadeTime));
                if (_sr) _sr.color = new Color(_startColor.r, _startColor.g, _startColor.b, a);
            }
            yield return null;
        }
        
        // Trigger event
        if (fadeIn) fadedIn.Invoke();
        else fadedOut.Invoke();

        // Set to end colour
        if (_sr) _sr.color = fadeIn
            ? _startColor
            : new Color(_startColor.r, _startColor.g, _startColor.b, 0f);
        
        // Exit cleanly
        _fadeRoutine = null;
    }
}
