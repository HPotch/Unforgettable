// Keeping the same object from being animated is not working!

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateValue : MonoBehaviour
{
    private List<object> animating = new List<object>();

    public void AnimateV3(Vector3 target, float time = 1f, AnimationCurve curve = null, Action<float> setter1 = null, Action<float> setter2 = null, Action<float> setter3 = null, Vector3 start = default)
    {
        AnimateFloat(target.x, time, curve, setter1, start.x);
        AnimateFloat(target.y, time, curve, setter2, start.y);
        AnimateFloat(target.z, time, curve, setter3, start.z);
    }
    
    public void AnimateV2(Vector2 target, float time = 1f, AnimationCurve curve = null, Action<float> setter1 = null, Action<float> setter2 = null, Vector2 start = default)
    {
        AnimateFloat(target.x, time, curve, setter1, start.x);
        AnimateFloat(target.y, time, curve, setter2, start.y);
    }
    
    public void AnimateFloat(float target, float time = 1f, AnimationCurve curve = null, Action<float> setter = null, float start = 0f)
    {
        if (setter == null)
        {
            Debug.LogError("No setter passed");
            return;
        }
        if (animating.Contains(setter.Target)) return; // Cannot animate setter that is already being animated
        animating.Add(setter.Target);
        
        curve ??= AnimationCurve.Linear(0, 0, 1, 1); // Default curve if none is passed
        
        StartCoroutine(AnimationRoutine(target, time, curve, setter, start));
    }

    private IEnumerator AnimationRoutine(float target, float time, AnimationCurve curve, Action<float> setter, float start)
    {
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float t =  curve.Evaluate(timer / time);
            setter(Mathf.LerpUnclamped(start, target, t));
            yield return null;
        }
        
        setter(target);
        animating.Remove(setter.Target);
    }
}
