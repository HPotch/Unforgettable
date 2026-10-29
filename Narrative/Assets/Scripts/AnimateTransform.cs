using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class AnimateTransform : MonoBehaviour
{
    [SerializeField] private float animationTime = 3f, waitTime = 0f;
    [SerializeField] private AnimationCurve positionCurve, rotationCurve, scaleCurve;
    [SerializeField] private Vector3 positionAmount, rotationAmount, scaleAmount;
    [SerializeField] private bool executeOnAwake = false;
    [SerializeField] private UnityEvent onStart, onEnd;

    private Vector3 _startPos, _startRot, _startScale;
    
    private void Awake()
    {
        if (executeOnAwake) StartCoroutine(AnimateRoutine());
        _startPos = transform.localPosition;
        _startRot = transform.localEulerAngles;
        _startScale = transform.localScale;
    }

    public void Animate()
    {
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        onStart.Invoke();
        yield return new WaitForSeconds(waitTime);
        float timer = 0f;
        
        SetTransform(0f);
        
        while (timer < animationTime)
        {
            timer += Time.deltaTime;
            
            float t = timer / animationTime;
            SetTransform(t);
            
            yield return null;
        }

        SetTransform(1f);
        onEnd.Invoke();
    }

    private void SetTransform(float t)
    {
        transform.localPosition = Vector3.Lerp(_startPos, positionAmount, positionCurve.Evaluate(t));
        transform.localEulerAngles = Vector3.Lerp(_startRot, rotationAmount, rotationCurve.Evaluate(t));
        transform.localScale = Vector3.Lerp(_startScale, scaleAmount, scaleCurve.Evaluate(t));
    }
}
