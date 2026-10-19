using System;
using UnityEngine;

public class BlockPart : MonoBehaviour
{
    private Block _parentBlock;

    private SpriteRenderer _sr;
    
    public GridPart GetNearestGridPart()
    {
        Transform grid = Grid.Instance.transform;
        foreach (GridPart child in grid.GetComponentsInChildren<GridPart>())
        {
            if (Vector2.Distance(transform.position, child.transform.position) < DiagonalLength)
                return child;
        }

        return null;
    }

    private void Awake()
    {
        _parentBlock = transform.parent.GetComponent<Block>();
        if (!_parentBlock) Debug.LogWarning("Parent of BlockPart has no Block component");
        _sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.5f);
    }

    private const float DiagonalLength = 0.707106781f; // sqrt(1^2 + 1^2) / 2

    public void CheckSnap()
    {
        if (_parentBlock.Rotating) return;
        GridPart nearestGrid = GetNearestGridPart();
        if (nearestGrid)
        {
            _parentBlock.SnapToPosition(nearestGrid.transform.position - (transform.position - _parentBlock.transform.position));
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1f);
        }
        else
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.5f);
    }
}