using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public UnityEvent onPressed;
    public UnityEvent onReleased;
    private Collider2D _collider;
    private bool _pressed = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (RayManager.Instance.mouseOver == _collider && Mouse.current.leftButton.wasPressedThisFrame)
            Pressed();
        if (_pressed && Mouse.current.leftButton.wasReleasedThisFrame)
            onReleased.Invoke();
    }

    private void Pressed()
    {
        _pressed = true;
        onPressed.Invoke();
    }
}
