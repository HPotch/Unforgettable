using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    public bool Rotating;
    
    [Header("Settings")]
    [SerializeField] private Color color;
    [SerializeField] private AnimationCurve rotateCurve;
    [SerializeField] private float rotateTime;
    [SerializeField] private AnimationCurve moveBackCurve;
    [SerializeField] private float moveBackTime;
    [SerializeField] private int startRotation;
    [SerializeField] private bool update;
    [SerializeField] private float moveSmoothness = 10f;

    [NonSerialized] private Movable movable;

    private Vector2 _previousPosition;
    private float _previousRotation;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    public void SnapToPosition(Vector2 position)
    {
        transform.position = position;
    }
    
    private void Awake()
    {
        UpdateBlock();
        update = false;
        movable = GetComponent<Movable>(); if (!movable) Debug.LogWarning("Block has no Movable component");
        movable.actuallyMove = false;
        transform.rotation = Quaternion.Euler(0, 0, 90f * startRotation);
        _startPosition = transform.position;
        _previousPosition = _startPosition;
        _previousRotation = transform.eulerAngles.z;
        
        movable.afterMove.AddListener(Move);
        movable.putDown.AddListener(PutDown);
        movable.pickUp.AddListener(PickUp);
    }

    private void Update()
    {
        if (update) UpdateBlock(); // Editor

        if ((Mouse.current.rightButton.wasPressedThisFrame 
        || Keyboard.current.spaceKey.wasPressedThisFrame ) 
        && movable.PickedUp)
            if (!Rotating)
            {
                StartCoroutine(RotateRoutine());
                Rotating = true;
            }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            _targetPosition = _startPosition;
            transform.rotation = Quaternion.Euler(0, 0, 90f * startRotation);
            _previousPosition = _startPosition;
            StartCoroutine(MoveBack());
        }
    }

    private void Move()
    {
        _targetPosition = movable.targetPosition;
        Vector2 positionBeforeMove = transform.position;
        transform.position = _targetPosition;
        
        // Snap
        foreach (BlockPart child in transform.GetComponentsInChildren<BlockPart>())
            child.CheckSnap();

        // If not snapped, move smoothly
        if (new Vector2(transform.position.x, transform.position.y) == _targetPosition)
            transform.position = Vector2.Lerp(positionBeforeMove, _targetPosition, moveSmoothness * Time.deltaTime);
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
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = transform.rotation * Quaternion.Euler(0, 0, 90f);
        float time = 0f;

        while (time < rotateTime)
        {
            time += Time.deltaTime;
            float t = rotateCurve.Evaluate(Mathf.Clamp01(time / rotateTime));
            transform.rotation = Quaternion.SlerpUnclamped(startRot, targetRot, t);
            yield return null;
        }
        
        // At this point, the block could hover over the board without being snapped to anything, which results in bugs.
        
        // Exit cleanly
        transform.rotation = targetRot;
        Rotating = false;
    }

    private void PutDown()
    {
        // Move as far to the camera as the rest
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        foreach (SpriteRenderer child in GetComponentsInChildren(typeof(SpriteRenderer)))
            child.sortingOrder = 1; // Set sprites as high as the rest
        
        if (!Rotating) CheckGrid();
    }

    private void CheckGrid()
    {
        Vector2 positionBeforeCheck = transform.position;
        transform.position = _targetPosition;
        
        List<GridPart> fillParts = new List<GridPart>();
        bool allowed = true;
        foreach (BlockPart child in GetComponentsInChildren(typeof(BlockPart)))
        {
            GridPart nearestGrid = child.GetNearestGridPart();
            if (!nearestGrid) continue;
            
            if (nearestGrid.Filled)
            {
                allowed = false;
                break;
            }
            fillParts.Add(nearestGrid);
        }

        transform.position = positionBeforeCheck;
        
        if (allowed)
            foreach (GridPart gridPart in fillParts)
                gridPart.Filled = true;
        else
        {
            _targetPosition = _previousPosition;
            transform.eulerAngles = new Vector3(0,0, _previousRotation);
            CheckGrid();
            StartCoroutine(MoveBack());
            return;
        }
        
        Grid.Instance.CheckGrid();
    }
    
    private IEnumerator MoveBack()
    {
        Vector2 startPosition = transform.position;
        float time = 0f;

        while (time < moveBackTime)
        {
            time += Time.deltaTime;
            float t = moveBackCurve.Evaluate(Mathf.Clamp01(time / moveBackTime));
            transform.position = Vector2.LerpUnclamped(startPosition, _targetPosition, t);
            yield return null;
        }
    
        transform.position = _targetPosition;
    }

    private void PickUp()
    {
        // Set previous position & rotation
        _previousPosition = transform.position;
        _previousRotation = transform.eulerAngles.z;
        
        // Move closer to the camera than the rest
        transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
        
        foreach (Transform child in GetComponentsInChildren(typeof(Transform)))
        {
            // Remove from grid
            BlockPart blockPart = child.GetComponent<BlockPart>();
            if (blockPart)
            {
                GridPart nearestGrid = blockPart.GetNearestGridPart();
                if (nearestGrid) nearestGrid.Filled = false;
            }
            
            // Set sprites higher than the rest
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            if (sr) sr.sortingOrder = 2;
        }
    }
}
