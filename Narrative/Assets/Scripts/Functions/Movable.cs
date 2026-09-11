using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movable : MonoBehaviour
{
    [NonSerialized] public bool PickedUp = false;
    private Vector2 _offset;

    private RayManager _rayManager;

    private void Start()
    {
        _rayManager = RayManager.Instance;
    }
    
    private void Update()
    {
        PickUp();
        PutDown();
        Move();
    }

    private void PickUp()
    {
        Collider2D mouseOver = _rayManager.mouseOver;
        if (!mouseOver) return;
        if (mouseOver.transform == transform || mouseOver.transform.parent == transform)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                PickedUp = true;
                _offset = new Vector2(transform.position.x, transform.position.y) - _rayManager.mouseWorldPos;
            }
        }
    }

    private void PutDown()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame) PickedUp = false;
    }

    private void Move()
    {
        if (PickedUp) transform.position = _rayManager.mouseWorldPos + _offset;
    }
}
