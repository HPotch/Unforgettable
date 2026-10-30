using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public UnityEvent onPressed, onReleased, onHover, onHoverExit;
    private Collider2D _collider;
    private bool _pressed = false, _hovered;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (RayManager.Instance.mouseOver == _collider)
        {
            if (!_hovered) Hover();
            if (Mouse.current.leftButton.wasPressedThisFrame) Press();
        } else if (_hovered) HoverExit();
        
        if (_pressed && Mouse.current.leftButton.wasReleasedThisFrame) Release();
    }

    private void Hover()
    {
        _hovered = true;
        onHover.Invoke();
    }

    private void HoverExit()
    {
        _hovered = false;
        onHoverExit.Invoke();
    }

    private void Press()
    {
        _pressed = true;
        onPressed.Invoke();
    }

    private void Release()
    {
        _pressed = false;
        onReleased.Invoke();
    }
}
