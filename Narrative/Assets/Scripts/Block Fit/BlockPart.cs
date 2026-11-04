using UnityEngine;

public class BlockPart : MonoBehaviour
{
    private Block _parentBlock;

    private SpriteRenderer _sr;
    
    public GridPart GetNearestGridPart()
    {
        Transform grid = BlockGrid.Instance.transform;
        GridPart nearest = null;
        float minDistance = float.MaxValue;

        // Loop through all parts
        foreach (GridPart child in grid.GetComponentsInChildren<GridPart>())
        {
            float distance = Vector2.Distance(transform.position, child.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = child;
            }
        }

        // If in a 1x1 square, return the gridpart
        if (nearest != null &&
            Mathf.Abs(transform.position.x - nearest.transform.position.x) <= 0.5f &&
            Mathf.Abs(transform.position.y - nearest.transform.position.y) <= 0.5f)
            return nearest;

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

    public bool CheckSnap(bool snapParent = true)
    {
        if (_parentBlock.Rotating) return false;
        GridPart nearestGrid = GetNearestGridPart();
        if (nearestGrid)
        {
            if (snapParent)
            {
                _parentBlock.SnapToPosition(nearestGrid.transform.position - (transform.position - _parentBlock.transform.position));
                return true;
            }
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 1f);
        }
        else
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, 0.5f);

        return false;
    }
}