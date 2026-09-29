using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Polaroid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform polaroid;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer dot;
    [SerializeField] private SpriteRenderer button;
    
    [Header("Settings")]
    [SerializeField] private AnimationCurve polaroidEnterCurve;
    [SerializeField] private float polaroidEnterTime = 3f;
    [SerializeField] private float polaroidRotationCircles = 12f;
    [SerializeField] private float polaroidScaleStart = 0.2f;
    [SerializeField] private float polaroidYStart = -6f;

    public UnityEvent onEntered;
    public UnityEvent onExited;

    private bool _entered = false, _animating = false;
    
    private void Awake()
    {
        polaroid.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        dot.gameObject.SetActive(false);
    }
    
    public void ShowPolaroid()
    {
        if (_entered || _animating) return;
        StartCoroutine(AnimateRoutine());
    }

    public void HidePolaroid()
    {
        if (!_entered || _animating) return;
        StartCoroutine(AnimateRoutine(false));
    }

    private IEnumerator AnimateRoutine(bool enter = true)
    {
        _animating = true;
        
        float timer = 0f;
        // All values that I change are floats, therefore, I can make Vector2s where x = start and y = end. This way, I can easily flip the values when exiting.
        Vector2 scale = new Vector2(polaroidScaleStart, polaroid.localScale.x);
        Vector2 y = new Vector2(polaroid.position.y - polaroidYStart, polaroid.position.y);
        Vector2 rotation = new Vector2(0f, 360f * polaroidRotationCircles);
        Vector2 darkening = new Vector2(0f, background.color.a);
        Vector2 lighting = new Vector2(0f, dot.color.a);
        Vector2 buttonSize = new Vector2(0f, 0.2f);

        // Invert if exiting
        if (!enter)
        {
            scale = InvertV2(scale); y = InvertV2(y); rotation = InvertV2(rotation); darkening = InvertV2(darkening); lighting = InvertV2(lighting); buttonSize = InvertV2(buttonSize);
        }
        
        // Set active
        polaroid.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        dot.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        // Animation loop
        while (timer < polaroidEnterTime)
        {
            float t = polaroidEnterCurve.Evaluate(Mathf.Clamp01(timer / polaroidEnterTime));

            polaroid.transform.eulerAngles = new Vector3(0f, Mathf.LerpUnclamped(rotation.x, rotation.y, t), 0f);
            polaroid.transform.position = new Vector3(0f, Mathf.LerpUnclamped(y.x, y.y, t), 0f);
            polaroid.transform.localScale = Vector3.one * Mathf.LerpUnclamped(scale.x, scale.y, t);
            background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.Lerp(darkening.x, darkening.y, t));
            dot.color = new Color(dot.color.r, dot.color.g, dot.color.b, Mathf.Lerp(lighting.x, lighting.y, t));
            button.transform.localScale = Vector3.one * Mathf.LerpUnclamped(buttonSize.x, buttonSize.y, t);
           
            timer += Time.deltaTime;
            yield return null;
        }

        // Exit cleanly
        polaroid.transform.eulerAngles = Vector3.zero;
        polaroid.transform.position = new Vector3(0f, y.y, 0f);
        polaroid.transform.localScale = Vector3.one * scale.y;
        background.color = new Color(background.color.r, background.color.g, background.color.b, darkening.y);
        dot.color = new Color(dot.color.r, dot.color.g, dot.color.b, lighting.y);
        button.transform.localScale = Vector3.one * buttonSize.y;

        // Disable objects on exit
        if (!enter)
        {
            polaroid.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            dot.gameObject.SetActive(false);
            button.gameObject.SetActive(false);
        }
        
        // Trigger event
        if (enter) onEntered.Invoke();
        else onExited.Invoke();

        _animating = false;
        _entered = enter;
    }
    
    private Vector2 InvertV2(Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }
}
