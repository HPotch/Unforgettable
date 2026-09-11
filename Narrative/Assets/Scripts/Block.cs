using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color color;
    [SerializeField] private AnimationCurve rotateCurve;
    [SerializeField] private float rotateTime;
    [SerializeField] private int startRotation;
    [SerializeField] private bool update;

    [NonSerialized] private Movable movable;

    private Coroutine _rotateRoutine;
    
    private void Awake()
    {
        UpdateBlock();
        movable = GetComponent<Movable>(); if (!movable) Debug.LogWarning("Block has no Movable component");
        transform.rotation = Quaternion.Euler(0, 0, 90f * startRotation);
    }

    private void Update()
    {
        if (update) UpdateBlock(); // Editor

        if (Mouse.current.rightButton.wasPressedThisFrame && movable.PickedUp)
        {
            _rotateRoutine ??= StartCoroutine(RotateRoutine());
        }
    }

    private void UpdateBlock()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.TryGetComponent(out SpriteRenderer sr);
            if (!sr) continue;
           
            // Update color
            sr.color = color;
            
            // Update positions
            Vector2 pos = child.localPosition;
            child.localPosition = new Vector3(Mathf.Round(pos.x),  Mathf.Round(pos.y), 0);
        }
    }

    private IEnumerator RotateRoutine()
    {
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 0, 90f);
        float _time = 0f;

        while (_time < rotateTime)
        {
            _time += Time.deltaTime;
            float t = rotateCurve.Evaluate(Mathf.Clamp01(_time / rotateTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            yield return null;
        }
    
        transform.rotation = targetRotation;
        _rotateRoutine = null;
    }
}
