using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Polaroid : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform polaroid;
    [SerializeField] private SpriteRenderer background, dot, button;
    
    [Header("Settings")]
    [SerializeField] private AnimationCurve polaroidEnterCurve;
    [SerializeField] private float polaroidEnterTime = 3f;
    [SerializeField] private float polaroidRotationCircles = 12f;
    [SerializeField] private float polaroidScaleStart = 0.2f;
    [SerializeField] private float polaroidYStart = -6f;

    public UnityEvent onEntered, onExited;

    private bool _entered = false, _animating = false;
    
    private struct AnimationData {
        public Vector2 scale, y, rotation, darkening, lighting, buttonSize;
    }
    
    
    private void Awake()
    {
        ActivateObjects(false);
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
        // Setup
        _animating = true;
        float timer = 0f;
        
        // Create start and end values
        // All values that I change are floats, therefore, I can make Vector2s where x = start and y = end. This way, I can easily flip the values when exiting.
        AnimationData animData = new AnimationData {
            scale = new Vector2(polaroidScaleStart, polaroid.localScale.x),
            y = new Vector2(polaroid.position.y - polaroidYStart, polaroid.position.y),
            rotation = new Vector2(0f, 360f * polaroidRotationCircles),
            darkening = new Vector2(0f, background.color.a),
            lighting = new Vector2(0f, dot.color.a),
            buttonSize = new Vector2(0f, 0.2f)
        };
        
        if (!enter) animData = InvertAnimData(animData); // Invert if exiting
        
        ActivateObjects(true); // Set active
        SetAnimations(animData, 0f); // Apply start values
        
        // Animation loop
        while (timer < polaroidEnterTime)
        {
            float t = polaroidEnterCurve.Evaluate(Mathf.Clamp01(timer / polaroidEnterTime));

            SetAnimations(animData, t);
           
            timer += Time.deltaTime;
            yield return null;
        }

        SetAnimations(animData, 1f); // Apply end values

        // Disable objects on exit
        if (!enter) ActivateObjects(false);
        
        // Trigger event
        if (enter) onEntered.Invoke();
        else onExited.Invoke();

        // Exit cleanly
        _animating = false;
        _entered = enter;
    }

    // Activates all the referenced objects
    private void ActivateObjects(bool activate = true)
    {
        polaroid.gameObject.SetActive(activate);
        background.gameObject.SetActive(activate);
        dot.gameObject.SetActive(activate);
        button.gameObject.SetActive(activate);
    }

    // Set the values of all the objects to the t value
    private void SetAnimations(AnimationData ad, float t = 0f)
    {
        polaroid.transform.eulerAngles = new Vector3(0f, Mathf.LerpUnclamped(ad.rotation.x, ad.rotation.y, t), 0f);
        polaroid.transform.position = new Vector3(0f, Mathf.LerpUnclamped(ad.y.x, ad.y.y, t), 0f);
        polaroid.transform.localScale = Vector3.one * Mathf.LerpUnclamped(ad.scale.x, ad.scale.y, t);
        background.color = new Color(background.color.r, background.color.g, background.color.b, Mathf.Lerp(ad.darkening.x, ad.darkening.y, t));
        dot.color = new Color(dot.color.r, dot.color.g, dot.color.b, Mathf.Lerp(ad.lighting.x, ad.lighting.y, t));
        button.transform.localScale = Vector3.one * Mathf.LerpUnclamped(ad.buttonSize.x, ad.buttonSize.y, t);
    }

    // Invert all the Vector2s (x becomes y and y becomes x)
    private AnimationData InvertAnimData(AnimationData ad)
    {
        return new AnimationData
        {
            scale = InvertV2(ad.scale),
            y = InvertV2(ad.y),
            rotation = InvertV2(ad.rotation),
            darkening = InvertV2(ad.darkening),
            lighting = InvertV2(ad.lighting),
            buttonSize = InvertV2(ad.buttonSize)
        };
    }
    // Invert a single Vector2
    private Vector2 InvertV2(Vector2 v)
    {
        return new Vector2(v.y, v.x);
    }
}
