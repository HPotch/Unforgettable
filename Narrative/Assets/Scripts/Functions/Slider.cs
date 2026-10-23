using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Slider : MonoBehaviour
{
    [NonSerialized] public float Value = 0f;
    public UnityEvent OnValueChanged;
    
    [Header("Settings")]
    [SerializeField] private float startValue;
    [SerializeField] private float weight = 20f;
    [SerializeField] private bool useSnap = false;
    public float snapPercent = 5f;

    [Header("References")]
    [SerializeField] private Collider2D slider;
    [SerializeField] private Transform max;

    // Private variables
    private Vector2 _bounds;
    private bool _selected = false;
    private Vector2 _preSnapPos;
    private float _previousValue = -1f;

    public void SetValue(float val)
    {
        if (_previousValue < 0f)
        {
            _bounds.x = slider.transform.localPosition.x;
            _bounds.y = max.localPosition.x;
        }
        
        val = Mathf.Clamp01(val);
        _previousValue = val;
        Value = val;
        slider.transform.localPosition = new Vector2(Mathf.Lerp(_bounds.x, _bounds.y, val), 0f);
        _preSnapPos = slider.transform.position;
    }
    
    private void Awake()
    {
        if (_previousValue >= 0f) return;
        _bounds.x = slider.transform.localPosition.x;
        _bounds.y = max.localPosition.x;
        slider.transform.localPosition = new Vector2(Mathf.Lerp(_bounds.x, _bounds.y, startValue), 0f);
        _preSnapPos = slider.transform.position;
        Value = startValue;
        _previousValue = Value;
    }

    private void Update()
    {
        // Select
        if (RayManager.Instance.mouseOver == slider && Mouse.current.leftButton.wasPressedThisFrame)
            _selected = true;
        // Deselect
        if (_selected && Mouse.current.leftButton.wasReleasedThisFrame)
            _selected = false;

        if (!_selected) return;
        // Move slider
        slider.transform.position = _preSnapPos;
        slider.transform.position = Vector2.Lerp(slider.transform.position, RayManager.Instance.mouseWorldPos, weight == 0 ? 1f : Time.deltaTime / weight);
        slider.transform.localPosition = new Vector2(Mathf.Clamp(slider.transform.localPosition.x, _bounds.x, _bounds.y), 0f);
            
        // Set value
        Value = (slider.transform.localPosition.x - _bounds.x) / Mathf.Abs(_bounds.x - _bounds.y);
            
        // Snap using that value
        _preSnapPos = slider.transform.position;
        if (useSnap)
        {
            float t = (Mathf.Round((Value * 100f) / snapPercent) / 100f) * snapPercent;
            slider.transform.localPosition = new Vector2(Mathf.Lerp(_bounds.x, _bounds.y, t),0f);
            Value = (slider.transform.localPosition.x - _bounds.x) / Mathf.Abs(_bounds.x - _bounds.y);
        }

        // OnMoved
        if (!Mathf.Approximately(_previousValue, Value))
            OnValueChanged.Invoke();
        
        _previousValue = Value;
    }
}
