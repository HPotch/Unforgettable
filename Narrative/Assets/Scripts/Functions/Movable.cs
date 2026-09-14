using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Movable : MonoBehaviour
{
    [Header("Settings")]
    public bool actuallyMove = true;
    
    public UnityEvent beforeMove;
    public UnityEvent afterMove;
    public UnityEvent pickUp;
    public UnityEvent putDown;
   
    [NonSerialized] public bool PickedUp = false;
    [NonSerialized] public Vector2 targetPosition;
    
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
        if (mouseOver.transform != transform && mouseOver.transform.parent != transform) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        PickedUp = true;
        _offset = new Vector2(transform.position.x, transform.position.y) - _rayManager.mouseWorldPos;
        pickUp.Invoke();
    }

    private void PutDown()
    {
        if (!Mouse.current.leftButton.wasReleasedThisFrame) return;
        if (PickedUp) putDown.Invoke();
        PickedUp = false;
    }

    private void Move()
    {
        if (!PickedUp) return;
        beforeMove.Invoke();
        targetPosition = _rayManager.mouseWorldPos + _offset;
        if (actuallyMove) transform.position = targetPosition;
        afterMove.Invoke();
    }
}
