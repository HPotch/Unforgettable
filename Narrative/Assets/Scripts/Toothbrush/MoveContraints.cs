using UnityEngine;

public class MoveConstraints : MonoBehaviour
{
    public static MoveConstraints Instance;

    private BoxCollider2D _collider;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        _collider = GetComponent<BoxCollider2D>();
    }

    public Vector2 min;
    public Vector2 max;

    private void Start()
    {
        min = _collider.bounds.min;
        max = _collider.bounds.max;
    }
}