using UnityEngine;
using UnityEngine.InputSystem;

public class Grid : MonoBehaviour
{
    public static Grid Instance;
    
    [SerializeField] private int width = 4;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private GameObject squarePrefab;

    public void CheckGrid()
    {
        foreach (GridPart child in GetComponentsInChildren(typeof(GridPart)))
        {
            if (!child.Filled) return;
        }
        print("Won!");
    }
    
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(gameObject);
        UpdateGrid();
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
            foreach (GridPart child in transform.GetComponentsInChildren<GridPart>())
                child.Filled = false;
    }
    
    private void UpdateGrid()
    {
        ClearChildren();
        Vector2 center = Vector2.one * ((spacing * (width - 1)) * 0.5f);
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject square = Instantiate(squarePrefab, transform);
                square.transform.position = new Vector2(spacing * x, spacing * y) - center;
            }
        }
    }

    private void ClearChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }
    }
}
